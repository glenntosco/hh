using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Floor.CycleCount
{
    [ViewController("main.cycleCountByMarkedBins")]
    public class CycleCountByMarkedBins : ProductScanController
    {
        public override string Title => "Marked bin cycle count";

        private LocationLookup _binLookupDetails;
        private readonly List<ProductOperation> _pendingOps = new List<ProductOperation>();

        private List<BinDetail> _markedBins;
        private BinDetail _binToCount;

        private Button _completeBtn;
        
        protected override async Task Init()
        {
            _completeBtn = View.RemoveToolbar(_completeBtn);
            _pendingOps.Clear();

            _markedBins = await Singleton<Web>.Instance.GetInvokeAsync<List<BinDetail>>("odata/Bin?$filter=CountRequired eq true&$select=Id,BinCode");
            await AskBinLpn();
        }

        private async Task AskBinLpn()
        {
            if (!_markedBins.Any())
                await View.PushError(Lang.Translate($"There are no more bins that are marked for Cycle Count"));
            else
                await View.PushMessage($@"{Lang.Translate("Bins marked for Cycle Count")}
{string.Join("\n", _markedBins.OrderBy(c => c.BinCode).Select(c => c.BinCode))}");
            
            await LoopUntilGood(async () =>
            {
                _binLookupDetails = await BinContentsLookup(AskBinLpn, "Scan Bin/LPN...", true);
                if(AssignedTask != null && AssignedTask.ReferenceId != _binLookupDetails.Id)
                    throw new ExceptionLocalized($"Invalid bin [{_binLookupDetails.LocationCode}], expected [{AssignedTask.ReferenceNumber}]");
                if (_binLookupDetails.Contents.Any(c => c.ReservedQuantity > 0))
                    throw new ExceptionLocalized("Cannot cycle count bin with allocated stock");
                _binToCount = _markedBins.SingleOrDefault(c => c.BinCode == _binLookupDetails.LocationCode);
                if(_binToCount == null)
                    throw new ExceptionLocalized($"Bin [{_binLookupDetails.LocationCode}] is not marked for Cycle Count");
            }, AskBinLpn);
            _completeBtn ??= View.AddToolbar("Complete", Complete);
            await AskProduct();
        }
        
        protected async Task AskProduct()
        {
            await AskProductOp(true);
        }

        protected override Func<Task> ProductReady => Process;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => AskProduct;

        protected async Task Process()
        {
            var prodOp = _pendingOps
                             .Where(c => c.ProductId == ProdOperation.ProductId)
                             .Where(c => c.PacksizeId == ProdOperation.PacksizeId)
                             .Where(c => c.LotNumber == ProdOperation.LotNumber)
                             .Where(c => c.Expiry == ProdOperation.Expiry)
                             .SingleOrDefault(c => c.SerialNumber == ProdOperation.SerialNumber) ??
                         PropMapper<ProductOperation, ProductOperation>.From(ProdOperation);
            try
            {
                prodOp.Quantity = ProdOperation.Quantity;
                if (!_pendingOps.Contains(prodOp))
                    _pendingOps.Add(prodOp);
                await Singleton<Web>.Instance.PostInvokeAsync($"hh/floor/CycleCountValidateByBin?{_binLookupDetails.QueryUrl}", _pendingOps);
                View.InactivateMessages();
                await View.PushMessage($"Accepted!");
            }
            catch (Exception ex)
            {
                _pendingOps.Remove(prodOp);
                //await View.PopLastMessage();
                await View.PushError(ex.Message, ProductReady);
            } 
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskProduct();
            }
        }

        protected async Task Complete()
        {
            try
            {
                FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

                await View.PushMessage($@"{Lang.Translate("Complete?")}
{Lang.Translate($"[{_pendingOps.Select(c=>c.ProductId).Distinct().Count()}] SKUs")}
{Lang.Translate($"[{_pendingOps.Sum(c => c.Quantity)}] Units")}", null, false);
                var confirm = await View.PromptBool("Confirm", "Yes", "No");
                if (!confirm)
                {
                    await View.PopLastMessage();
                    await AskProduct();
                    return;
                }

                if (!_pendingOps.Any())
                {
                    confirm = await View.PromptBool("Empty bin?", "Yes", "No");
                    if (!confirm)
                    {
                        await View.PopLastMessage();
                        await AskProduct();
                        return;
                    }
                }

                var url = $"hh/floor/CycleCountCommitByBin?{_binLookupDetails.QueryUrl}";
                if (AssignedTask != null)
                    url += $"&taskId={AssignedTask.Id}";
                await Singleton<Web>.Instance.PostInvokeAsync(url, _pendingOps);
                View.InactivateMessages();
                await View.PushMessage($"Counted!");

                _markedBins.Remove(_binToCount);

                await AskBinLpn();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Complete);
                await AskProduct();
            }
        }
    }
}