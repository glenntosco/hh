using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.DataTransferObjects.Dto.Receiving;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.unReceivePo")]
    public class UnreceivePo : ProductScanController
    {
        public override string Title => "Un-receive PO";
        private List<PurchaseOrder> _pos = new List<PurchaseOrder>();
        private List<PurchaseOrderLine> _poLines = new List<PurchaseOrderLine>();

        private LocationLookup _fromBinLpnLookupDetails;
        protected override async Task Init()
        {
            await AskPo();
        }

        protected async Task AskPo()
        {
            _pos = await LoopUntilGood(async () =>
            {
                var pos = await PoContainerLookup();
                var result = new List<PurchaseOrder>();

                foreach (var po in pos.OrderBy(c => c.PurchaseOrderNumber))
                {
                    if (_pos.Any(c => c.Id == po.Id))
                        throw new ExceptionLocalized($"{(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}] already in batch");

                    if (po.PurchaseOrderState != PurchaseOrderState.PartiallyReceived &&
                        po.PurchaseOrderState != PurchaseOrderState.Received)
                        throw new ExceptionLocalized($"Cannot Un-receive {(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}], invalid state [{po.PurchaseOrderState}]");

                    var message = $@"{Lang.Translate($"{(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}]")}
{Lang.Translate($"From [{po.VendorCompanyName}]")}
{Lang.Translate($"Lines [{po.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                    message += $"\n{Lang.Translate($"Units [{po.Lines.Sum(c => c.OutstandingQuantity)}]")}";

                    await View.PushMessage(message, AskPo, false);
                    if (await View.PromptBool("Confirm", "Yes", "No"))
                        result.Add(po);
                    else
                        await View.PopLastMessage();
                }
                return result;
            }, AskPo);

            if (!_pos.Any())
                await AskPo();
            else
                await AskProduct();
        }

        protected async Task AskProduct()
        {
            await LoopUntilGood(async () =>
            {
                var clients = _pos.Select(c => c.ClientId).Distinct().ToList();
                Guid? clientId = null;
                if (clients.Count == 1)
                    clientId = clients.SingleOrDefault();

                ProdDetails = await ProductLookup(AskProduct, clientId);

                _poLines = _pos.SelectMany(c => c.Lines).Where(c => c.ReceivedQuantity > 0).Where(c => c.ProductId == ProdDetails.Id).ToList();
                if (!_poLines.Any())
                    throw new ExceptionLocalized($"No received lines of [{ProdDetails.Sku}]");

                if (_poLines.Select(c => c.PurchaseOrderNumber).Distinct().Count() > 1)
                {
                    var po = await View.PromptPicker("Refine: ", _poLines.Select(c => c.PurchaseOrderNumber).Distinct().OrderBy(c => c).ToList());
                    _poLines = _poLines.Where(c => c.PurchaseOrderNumber == po).ToList();
                }

                if (ProdDetails.PacksizeId != null)
                    _poLines = _poLines.Any(c => c.Packsize == ProdDetails.EachCount) ? _poLines.Where(c => c.Packsize == ProdDetails.EachCount).ToList() : _poLines.Where(c => c.LineNumber == _poLines.Min(c1 => c1.LineNumber)).ToList();
            }, AskProduct);

            ProdOperation = new ProductOperation
            {
                ProductId = ProdDetails.Id,
                PacksizeId = ProdDetails.PacksizeId
            };

            if (Singleton<Context>.Instance.PromptExpectedQuantityOnReceiving)
            {
                var message = string.Join("\n", _poLines.OrderBy(c => c.LineNumber).Select(c => Lang.Translate($"Line [{c.LineNumber}] Qty [{c.ReceivedQuantity}] received")));
                if (ProdDetails.IsPacksizeControlled)
                    message = string.Join("\n", _poLines.OrderBy(c => c.LineNumber).Select(c => $@"{Lang.Translate($"Line [{c.LineNumber}]")}
{Lang.Translate($"[{(int)(c.ReceivedQuantity / (c.Packsize ?? 1))}] pack(s) of [x{c.Packsize}] received")}"));
                if (_pos.Count > 1)
                    message = $@"{Lang.Translate($"PO [{_poLines.First().PurchaseOrderNumber}]")}
{message}";
                if (_poLines.Count > 1)
                    message += $"\n{Lang.Translate($"Total [{_poLines.Sum(c => c.ReceivedQuantity)}]")}";
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
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "UnReceivePo");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                var originalEntered = ProdOperation.Quantity;
                foreach (var poLine in _poLines.OrderByDescending(c => c.LineNumber))
                {
                    if (ProdOperation.Quantity <= 0)
                        break;
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/UnReceivePo?poLineId={poLine.Id}&{_fromBinLpnLookupDetails.QueryUrl}", ProdOperation);

                    var message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] adjusted!");
                    if (ProdDetails.PacksizeId != null)
                        message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] pack(s) of [x{ProdDetails.EachCount}] adjusted!");

                    await View.PushMessage(message, null, false);

                    poLine.ReceivedQuantity += ProdOperation.Quantity * (ProdDetails.EachCount ?? 1);
                    originalEntered -= ProdOperation.Quantity;

                    ProdOperation.Quantity = originalEntered;
                    if (poLine.OutstandingQuantity > 0)
                        continue;
                    if(poLine.ReceivedQuantity == 0)
                        _poLines.Remove(poLine);

                    await View.PushMessage($"PO [{poLine.PurchaseOrderNumber}] adjusted!");
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