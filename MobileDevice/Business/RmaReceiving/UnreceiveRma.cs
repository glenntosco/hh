using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.DataTransferObjects.Dto.Returns;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.RmaReceiving
{
    [ViewController("main.unReceiveRma")]
    public class UnreceiveRma : ProductScanController
    {
        public override string Title => "Un-receive RMA";
        private CustomerReturn _rma;
        private List<CustomerReturnLine> _rmaLines = new List<CustomerReturnLine>();

        private LocationLookup _fromBinLpnLookupDetails;
        protected override async Task Init()
        {
            await AskRma();
        }

        protected async Task AskRma()
        {
            await LoopUntilGood(async () =>
            {
                _rma = await RmaLookup();
                if (_rma.CustomerReturnState != CustomerReturnState.PartiallyReceived &&
                    _rma.CustomerReturnState != CustomerReturnState.Received)
                    throw new ExceptionLocalized($"Cannot Un-receive RMA [{_rma.CustomerReturnNumber}], invalid state [{_rma.CustomerReturnState}]");

                var message = $@"{Lang.Translate($"RMA [{_rma.CustomerReturnNumber}]")}
{Lang.Translate($"From [{_rma.CustomerCompanyName}]")}
{Lang.Translate($"Lines [{_rma.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                message += $"\n{Lang.Translate($"Units [{_rma.Lines.Sum(c => c.OutstandingQuantity)}]")}";

                await View.PushMessage(message, AskRma, false);
                return _rma;
            }, AskRma);

            if (_rma == null)
                await AskRma();

            await AskProduct();
        }

        protected async Task AskProduct()
        {
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProduct, _rma.ClientId);

                _rmaLines = _rma.Lines.Where(c => c.ReceivedQuantity + c.DamagedQuantity > 0).Where(c => c.ProductId == ProdDetails.Id).ToList();
                if (!_rmaLines.Any())
                    throw new ExceptionLocalized($"No received lines of [{ProdDetails.Sku}]");

                if (ProdDetails.PacksizeId != null)
                    _rmaLines = _rmaLines.Any(c => c.Packsize == ProdDetails.EachCount) ? _rmaLines.Where(c => c.Packsize == ProdDetails.EachCount).ToList() : _rmaLines.Where(c => c.LineNumber == _rmaLines.Min(c1 => c1.LineNumber)).ToList();
            }, AskProduct);

            ProdOperation = new ProductOperation
            {
                ProductId = ProdDetails.Id,
                PacksizeId = ProdDetails.PacksizeId
            };

            if (Singleton<Context>.Instance.PromptExpectedQuantityOnReceiving)
            {
                var message = string.Join("\n", _rmaLines.OrderBy(c => c.LineNumber).Select(c => Lang.Translate($"Line [{c.LineNumber}] Qty [{c.ReceivedQuantity}] received")));
                if (ProdDetails.IsPacksizeControlled)
                    message = string.Join("\n", _rmaLines.OrderBy(c => c.LineNumber).Select(c => $@"{Lang.Translate($"Line [{c.LineNumber}]")}
{Lang.Translate($"[{(int)(c.ReceivedQuantity / (c.Packsize ?? 1))}] pack(s) of [x{c.Packsize}] received")}"));
                
                message = $@"{Lang.Translate($"RMA [{_rma.CustomerReturnNumber}]")}
{message}";
                if (_rmaLines.Count > 1)
                    message += $"\n{Lang.Translate($"Total [{_rmaLines.Sum(c => c.ReceivedQuantity)}]")}";
                await View.PushMessage(message, null, false);
            }

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await AskQuantity();
        }

        protected override Func<Task> ProductReady => AskFromBinLpn;
        protected override Func<Task> SerialReady => AskFromBinLpn;
        protected override Func<Task> NoMoreSerials => AskProduct;

        private async Task AskFromBinLpn()
        {
            _fromBinLpnLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "UnReceiveRma");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                var originalEntered = ProdOperation.Quantity;
                foreach (var rmaLine in _rmaLines.OrderByDescending(c => c.LineNumber))
                {
                    if (ProdOperation.Quantity <= 0)
                        break;
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/UnReceiveRma?rmaLineId={rmaLine.Id}&{_fromBinLpnLookupDetails.QueryUrl}", ProdOperation);

                    var message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] adjusted!");
                    if (ProdDetails.PacksizeId != null)
                        message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] pack(s) of [x{ProdDetails.EachCount}] adjusted!");

                    await View.PushMessage(message, null, false);

                    rmaLine.ReceivedQuantity += ProdOperation.Quantity * (ProdDetails.EachCount ?? 1);
                    originalEntered -= ProdOperation.Quantity;

                    ProdOperation.Quantity = originalEntered;
                    if (rmaLine.OutstandingQuantity > 0)
                        continue;
                    if(rmaLine.ReceivedQuantity == 0)
                        _rmaLines.Remove(rmaLine);

                    await View.PushMessage($"RMA [{rmaLine.CustomerReturnNumber}] adjusted!");
                }
                View.InactivateMessages();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskProduct();
            }
        }
    }
}