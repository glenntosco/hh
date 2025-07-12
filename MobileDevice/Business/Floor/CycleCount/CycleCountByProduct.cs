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
    [ViewController("main.cycleCountByProduct")]
    public class CycleCountByProduct : ProductScanController
    {
        public override string Title => "Product cycle count";

        private LocationLookup _binLookupDetails;
        private readonly List<ProductOperation> _pendingOps = new List<ProductOperation>();

        private ProductAvailability _prodLocations;
        private Button _completeBtn;
        
        protected override async Task Init()
        {
            if (AssignedTask != null)
                await View.PushMessage($@"Cycle count [{AssignedTask.ReferenceNumber}]");
            _completeBtn = View.RemoveToolbar(_completeBtn);
            _pendingOps.Clear();
            _binLookupDetails = null;
            ProdDetails = null;
            ProdOperation = null;
            await AskProdToCount();
        }

        private async Task AskProdToCount()
        {
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(null, null, false);
                _prodLocations = await Singleton<Web>.Instance.GetInvokeAsync<ProductAvailability>($"hh/lookup/ProductContentsLookup?productId={ProdDetails.Id}&includeSticky=false&take=100");
            }, AskProdToCount);

            await ShowBins();
        }

        protected async Task ShowBins()
        {
            _completeBtn = View.RemoveToolbar(_completeBtn);
            if (!_prodLocations.Records.Any())
                await View.PushMessage("No inventory");
            else
            {
                if (_prodLocations.Records.Any())
                    await View.PushMessage(_prodLocations.GetCycleCountText(Lang.Translate), null, false);
                else
                {
                    await View.PushMessage($"Finished counting [{ProdDetails.Sku}]");
                    await Init();
                }
            }

            await AskBinLpn();
        }

        protected async Task AskBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _binLookupDetails = await BinContentsLookup(AskBinLpn, "Scan Bin/LPN...", true);
                if (_binLookupDetails.Contents.Where(c => c.ProductId == ProdDetails.Id).Any(c => c.ReservedQuantity > 0))
                    throw new ExceptionLocalized("Cannot cycle count bin with allocated stock");
            }, AskBinLpn);

            if(ProdDetails.IsDetailControlled)
                _completeBtn ??= View.AddToolbar("Complete", Complete);
            _pendingOps.Clear();
            await AskContents();
        }

        protected async Task AskContents()
        {
            FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

            if (ProdDetails.IsPacksizeControlled)
            {
                var packsize = await View.PromptPacksize("Packsize", ProdDetails.Packsizes);

                await View.PushMessage($"{Lang.Translate($"Packsize: [x{packsize.EachCount}]")}");

                ProdDetails.PacksizeId = packsize.Id;
                ProdDetails.Barcode = packsize.Barcode;
                ProdDetails.PacksizeName = packsize.Name;
                ProdDetails.EachCount = packsize.EachCount;
                ProdDetails.Height = packsize.Height;
                ProdDetails.Width = packsize.Width;
                ProdDetails.Length = packsize.Length;
                ProdDetails.Weight = packsize.Weight;
            }

            ProdOperation = new ProductOperation
            {
                ProductId = ProdDetails.Id,
                PacksizeId = ProdDetails.PacksizeId
            };

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await AskQuantity(true);
        }

        protected override Func<Task> ProductReady => Process;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => Complete;

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

                if (!ProdDetails.IsDetailControlled)
                    await Complete();
                else
                {
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/floor/CycleCountValidateByProduct?{_binLookupDetails.QueryUrl}", _pendingOps);
                    View.InactivateMessages();
                    await View.PushMessage($"Accepted!");
                }
            }
            catch (Exception ex)
            {
                _pendingOps.Remove(prodOp);
                await View.PushError(ex.Message, ProductReady);
            } 
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskContents();
            }
        }

        protected async Task Complete()
        {
            try
            {
                FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

                await View.PushMessage($@"{Lang.Translate("Complete?")}
{Lang.Translate($"[{_pendingOps.Sum(c => c.Quantity)}] pack(s)/unit(s)")}", null, false);
                var confirm = await View.PromptBool("Confirm", "Yes", "No");
                if (!confirm)
                {
                    await View.PopLastMessage();
                    await AskContents();
                    return;
                }

                if (!_pendingOps.Any())
                {
                    confirm = await View.PromptBool("Bin has no product?", "Yes", "No");
                    if (!confirm)
                    {
                        await View.PopLastMessage();
                        await AskContents();
                        return;
                    }
                }

                var url = $"hh/floor/CycleCountCommitByProduct?{_binLookupDetails.QueryUrl}";
                if (AssignedTask != null)
                    url += $"&taskId={AssignedTask.Id}";
                await Singleton<Web>.Instance.PostInvokeAsync(url, _pendingOps);

                var prodLoc = _prodLocations.Records.SingleOrDefault(c => c.LicensePlateId == _binLookupDetails.LicensePlateId && c.BinId == _binLookupDetails.BinId);
                if (prodLoc != null)
                    _prodLocations.Records.Remove(prodLoc);

                View.InactivateMessages();
                await View.PushMessage($"Counted!");
                await ShowBins();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Complete);
                await AskContents();
            }
        }
    }
}