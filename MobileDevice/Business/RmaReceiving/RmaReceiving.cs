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
using Xamarin.Forms;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.RmaReceiving
{
    [ViewController("main.rmaReceiving")]
    public class RmaReceiving : ProductScanController
    {
        public override string Title => "RMA receiving";

        private LocationLookup _toBinLpnLookupDetails;

        private List<CustomerReturnLine> _rmaLines;
        private CustomerReturn _rma;

        private Button _completeToolbar;

        private bool _promptExpectedQty;
        private bool _allowDiscaringDamaged;
        private bool _allowReturnClose;
        private bool _isPrintLabels;

        private bool _damagedGoods;

        protected override async Task Init()
        {
            var configs = (await Singleton<Web>.Instance.PostInvokeAsync<List<ConfigEntry>>("data/configs", new List<string>
            {
                nameof(ConfigConstants.Business_Returns_Operations_AllowDiscardingDamagedProduct),
                nameof(ConfigConstants.Business_Returns_Handheld_ShowExpectedQuantity),
                nameof(ConfigConstants.Business_Returns_Handheld_AllowHandheldClose),
                nameof(ConfigConstants.Business_Returns_Handheld_PrintLabels),
            })).ToDictionary(c => c.Name, c => c.BoolValue);

            _promptExpectedQty = configs[nameof(ConfigConstants.Business_Returns_Handheld_ShowExpectedQuantity)];
            _allowReturnClose = configs[nameof(ConfigConstants.Business_Returns_Handheld_AllowHandheldClose)];
            _allowDiscaringDamaged = configs[nameof(ConfigConstants.Business_Returns_Operations_AllowDiscardingDamagedProduct)];
            _isPrintLabels = configs[nameof(ConfigConstants.Business_Returns_Handheld_PrintLabels)];
            await AskReturn();
        }

        //Pre start
        protected async Task AskReturn()
        {
            if (_allowReturnClose)
                _completeToolbar = View.RemoveToolbar(_completeToolbar);

            await LoopUntilGood(async () =>
            {
                _rma = await RmaLookup();
                if (_rma.CustomerReturnState != CustomerReturnState.NotReceived &&
                    _rma.CustomerReturnState != CustomerReturnState.PartiallyReceived)
                    throw new ExceptionLocalized($"Cannot receive [{_rma.CustomerReturnNumber}], invalid state [{_rma.CustomerReturnState}]");

                var message = $@"{Lang.Translate($"RMA [{_rma.CustomerReturnNumber}]")}
{Lang.Translate($"Customer [{_rma.CustomerCompanyName}]")}
{Lang.Translate($"Lines [{_rma.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                if (_promptExpectedQty)
                    message += $"\n{Lang.Translate($"Units [{_rma.Lines.Sum(c => c.OutstandingQuantity)}]")}";

                await View.PushMessage(message, null, false);
            }, AskReturn);

            if (_allowReturnClose)
                _completeToolbar = View.AddToolbar("Complete", Complete);

            await AskProduct();
        }

        protected async Task AskProduct()
        {
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProduct, _rma.ClientId);
                _rmaLines = _rma.Lines.Where(c => c.OutstandingQuantity > 0).Where(c => c.ProductId == ProdDetails.Id).ToList();
                if (ProdDetails.PacksizeId != null)
                {
                    var bound = _rmaLines
                        .Where(c => c.ToteLineId != null)
                        .Where(c => c.LineDetails.Any(c1 => c1.PacksizeEachCount == ProdDetails.EachCount)).ToList();

                    var nonBound = _rmaLines.Any(c => c.Packsize == ProdDetails.EachCount) ? _rmaLines.Where(c => c.ToteLineId == null).Where(c => c.Packsize == ProdDetails.EachCount).ToList() : _rmaLines.Where(c => c.ToteLineId == null).Where(c => c.LineNumber == _rmaLines.Min(c1 => c1.LineNumber)).ToList();

                    _rmaLines = bound.Union(nonBound).ToList();
                }

                if (!_rmaLines.Any())
                {
                    if (ProdDetails.PacksizeId == null)
                        throw new ExceptionLocalized($"No lines of [{ProdDetails.Sku}] to receive");
                    throw new ExceptionLocalized($"No lines of [{ProdDetails.Sku}] [x{ProdDetails.EachCount}] to receive");
                }
            }, AskProduct);

            ProdOperation = new ProductOperation
            {
                ProductId = ProdDetails.Id,
                PacksizeId = ProdDetails.PacksizeId
            };

            if (_promptExpectedQty)
            {
                var message = $@"{Lang.Translate($"RMA [{_rma.CustomerReturnNumber}]")}";
                foreach (var rmaLine in _rmaLines.OrderBy(c => c.LineNumber))
                {
                    message += $"\n{Lang.Translate($"Line [{rmaLine.LineNumber}] Qty [{rmaLine.OutstandingQuantity}] expected")}";
                    foreach (var lineDetail in rmaLine.LineDetails.Where(c => c.Quantity > 0))
                    {
                        string lot = null, expiry = null, pck, serial = null;
                        if (!string.IsNullOrWhiteSpace(lineDetail.LotNumber))
                            lot = lineDetail.LotNumber;
                        if (!string.IsNullOrWhiteSpace(lineDetail.SerialNumber))
                            serial = lineDetail.SerialNumber;
                        if (!string.IsNullOrWhiteSpace(lineDetail.ExpiryString))
                            expiry = lineDetail.ExpiryString;
                        if (lineDetail.PacksizeEachCount != 1)
                            pck = Lang.Translate($"[{lineDetail.OutstandingQuantity}] pack(s) of [x{lineDetail.PacksizeEachCount}]");
                        else
                            pck = Lang.Translate($"[{lineDetail.OutstandingQuantity}] units");

                        var str = $"\n{string.Join(" - ", new[] { lot, serial, expiry, pck }.Where(c => c != null))}";
                        if (!string.IsNullOrWhiteSpace(str))
                            message += str;
                    }
                }

                await View.PushMessage(message, null, false);
            }

            _damagedGoods = false;
            if (_allowDiscaringDamaged)
                _damagedGoods = await View.PromptBool("Damaged?", "Yes", "No");

            if (_damagedGoods)
                await AskQuantity();
            else
            {

                if (ProdDetails.IsLotControlled)
                    await AskLot();
                if (ProdDetails.IsExpiryControlled)
                    await AskExpiry();
                if (ProdDetails.IsSerialControlled)
                    await AskToBinLpn();
                else
                    await AskQuantity();
            }
        }

        protected override Func<Task> ProductReady => async () =>
        {
            if (_damagedGoods)
                await Process();
            else
                await AskToBinLpn();
        };
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => AskProduct;

        private async Task AskToBinLpn()
        {
            if (Singleton<Context>.Instance.IsSuggestPutawayReturns)
            {
                var bins = await Singleton<Web>.Instance.PostInvokeAsync<List<string>>($"hh/lookup/SuggestPutaway?productId={ProdDetails.Id}&warehouseId={_rma.WarehouseId}&source=returns", ProdOperation);
                if (bins.Any())
                    await View.PushMessage(string.Join("\n", bins), null, false);
            }

            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await Process();
        }

        protected async Task Process()
        {
            try
            {
                var originalEntered = ProdOperation.Quantity;
                foreach (var rmaLine in _rmaLines.OrderBy(c => c.LineNumber).ToList())
                {
                    if (ProdOperation.Quantity <= 0)
                        break;

                    if (_damagedGoods)
                        await Singleton<Web>.Instance.PostInvokeAsync<CustomerReturnLine>($"hh/receive/ReceiveRma?lineId={rmaLine.Id}&isDamaged=true", ProdOperation);
                    else
                        await Singleton<Web>.Instance.PostInvokeAsync<CustomerReturnLine>($"hh/receive/ReceiveRma?lineId={rmaLine.Id}&{_toBinLpnLookupDetails.QueryUrl}", ProdOperation);

                    var message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] received{(_damagedGoods ? " damaged" : "")}!");
                    if (ProdDetails.PacksizeId != null)
                        message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] pack(s) of [x{ProdDetails.EachCount}] received{(_damagedGoods ? " damaged" : "")}!");

                    await View.PushMessage(message, null, false);

                    if (!_damagedGoods && _isPrintLabels && await View.PromptBool("Print labels?", "Yes", "No"))
                    {
                        var qty = ProdOperation.Quantity;
                        if (ProdDetails.IsDecimalControlled)
                            qty = 0;

                        var labelCount = (int)await View.PromptNumeric("Number of labels", qty);
                        var labelPrintPayload = new ProductOperation
                        {
                            ProductId = ProdOperation.ProductId,
                            PacksizeId = ProdOperation.PacksizeId,
                            LotNumber = ProdOperation.LotNumber,
                            Expiry = ProdOperation.Expiry,
                            SerialNumber = ProdOperation.SerialNumber,
                            Quantity = labelCount
                        };
                        await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/PrintProductLabels", labelPrintPayload);
                    }

                    _damagedGoods = false;
                    rmaLine.ReceivedQuantity += ProdOperation.Quantity * (ProdDetails.EachCount ?? 1);
                    originalEntered -= ProdOperation.Quantity;
                    ProdOperation.Quantity = originalEntered;
                    if (rmaLine.OutstandingQuantity > 0)
                        continue;

                    await View.PushMessage($"Line [{rmaLine.LineNumber}] of [{rmaLine.CustomerReturnNumber}] received in full!");
                    _rma.Lines.Remove(rmaLine);
                    if (_rma.Lines.Any())
                        continue;

                    await View.PushMessage($"Return [{_rma.CustomerReturnNumber}] received in full!");
                }

                View.InactivateMessages();
                Func<Task> next = AskProduct;
                if (ProdDetails.IsSerialControlled)
                    next = AskSerial;
                if (!_rma.Lines.Any())
                    next = Complete;
                await next();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBinLpn();
            }
        }

        protected async Task Complete()
        {
            if (_rma.Lines.Any())
            {
                await View.PushMessage($@"{Lang.Translate("Are you sure?")}
{Lang.Translate($"[{_rma.Lines.Sum(c1 => c1.OutstandingQuantity)}] outstanding")}", null, false);
                var confirm = await View.PromptBool("Confirm", "Yes", "No");
                await View.PopLastMessage();
                if (!confirm)
                {
                    await AskProduct();
                    return;
                }
            }

            try
            {
                await Singleton<Web>.Instance.PostInvokeAsync("hh/receive/CompleteRma", new List<Guid> {_rma.Id});
                await View.PushMessage("RMA completed!");
                await View.PromptBool("Confirm", "OK");
                await AskReturn();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message, Complete);
            }
        }
    }
}