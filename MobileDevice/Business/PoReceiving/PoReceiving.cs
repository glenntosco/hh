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
using Xamarin.Forms;
using ProductOperation = Pro4Soft.DataTransferObjects.Dto.Floor.ProductOperation;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.poReceiving")]
    public class PoReceiving : ProductScanController
    {
        public override string Title => "PO receiving";

        private LocationLookup _toBinLpnLookupDetails;

        private List<PurchaseOrderLine> _poLines = new List<PurchaseOrderLine>();
        private readonly List<PurchaseOrder> _pos = new List<PurchaseOrder>();

        private Button _startToolbar;
        private Button _completeToolbar;

        private bool _allowPoClose;
        private bool _collectDims;
        private bool _collectBarcode;
        private bool _isPrintLabels;

        protected override async Task Init()
        {
            var configs = (await Singleton<Web>.Instance.PostInvokeAsync<List<ConfigEntry>>("data/configs", new List<string>
            {
                nameof(ConfigConstants.Business_Receiving_Handheld_AllowHandheldClose),
                nameof(ConfigConstants.Business_Receiving_Handheld_CollectWeightAndDimensions),
                nameof(ConfigConstants.Business_Receiving_Handheld_CollectBarcode),
                nameof(ConfigConstants.Business_Receiving_Handheld_PrintLabels),
            })).ToDictionary(c => c.Name, c => c.BoolValue);

            _allowPoClose = configs[nameof(ConfigConstants.Business_Receiving_Handheld_AllowHandheldClose)];
            _collectDims = configs[nameof(ConfigConstants.Business_Receiving_Handheld_CollectWeightAndDimensions)];
            _collectBarcode = configs[nameof(ConfigConstants.Business_Receiving_Handheld_CollectBarcode)];
            _isPrintLabels = configs[nameof(ConfigConstants.Business_Receiving_Handheld_PrintLabels)];
            await CollectPos();
        }

        public async Task CollectPos()
        {
            if(_allowPoClose)
                _completeToolbar = View.RemoveToolbar(_completeToolbar);
            _startToolbar = View.RemoveToolbar(_startToolbar);
            _pos.Clear();
            _poLines.Clear();
            View.ClearMessages();
            await AskPo();
        }

        //Pre start
        protected async Task AskPo()
        {
            await LoopUntilGood(async () =>
            {
                var pos = await PoContainerLookup();

                foreach (var po in pos.OrderBy(c=>c.PurchaseOrderNumber))
                {
                    if (_pos.Any(c => c.Id == po.Id))
                        throw new ExceptionLocalized($"{(po.IsWarehouseTransfer?"Transfer":"PO")} [{po.PurchaseOrderNumber}] already in batch");

                    if (po.PurchaseOrderState != PurchaseOrderState.NotReceived &&
                        po.PurchaseOrderState != PurchaseOrderState.PartiallyReceived)
                        throw new ExceptionLocalized($"Cannot receive {(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}], invalid state [{po.PurchaseOrderState}]");

                    var message = $@"{Lang.Translate($"{(po.IsWarehouseTransfer ? "Transfer" : "PO")} [{po.PurchaseOrderNumber}]")}
{Lang.Translate($"From [{po.VendorCompanyName}]")}
{Lang.Translate($"Lines [{po.Lines.Count(c => c.OutstandingQuantity > 0)}]")}";
                    if (Singleton<Context>.Instance.PromptExpectedQuantityOnReceiving)
                        message += $"\n{Lang.Translate($"Units [{po.Lines.Sum(c => c.OutstandingQuantity)}]")}";

                    await View.PushMessage(message, AskPo, false);
                    if (await View.PromptBool("Confirm", "Yes", "No"))
                        _pos.Add(po);
                    else
                        await View.PopLastMessage();
                }
            }, AskPo);
            if(_pos.Count > 0)
                _startToolbar ??= View.AddToolbar("Start", Start);
            await AskPo();
        }

        protected async Task Start()
        {
            if (_allowPoClose)
                _completeToolbar = View.AddToolbar("Complete", Complete);

            _startToolbar = View.RemoveToolbar(_startToolbar);
            _poLines.Clear();
            
            View.ClearMessages();
            await View.PushMessage($@"{Lang.Translate($"[{_pos.Count}] documents")}
{Lang.Translate($"[{_pos.Sum(c => c.Lines.Count(c1 => c1.OutstandingQuantity > 0))}] lines")}
{Lang.Translate($"[{_pos.Sum(c => c.Lines.Sum(c1 => c1.OutstandingQuantity))}] units")}", null, false);
            await AskProduct();
        }

        protected async Task AskProduct()
        {
            _poLines.Clear();
            await LoopUntilGood(async () =>
            {
                var clients = _pos.Select(c => c.ClientId).Distinct().ToList();
                Guid? clientId = null;
                if (clients.Count == 1)
                    clientId = clients.SingleOrDefault();

                ProdDetails = await ProductLookup(AskProduct, clientId);
                if (_collectBarcode)
                {
                    var barcode = ProdDetails.Barcode;
                    if (string.IsNullOrWhiteSpace(barcode))
                    {
                        barcode = await View.PromptScan("Scan barcode...");
                        if (barcode != null)
                            await View.PushMessage($"Barcode: [{barcode}]");
                    }

                    if (barcode != ProdDetails.Barcode)
                    {
                        var url = $"hh/floor/ProductDimensions?productId={ProdDetails.Id}";
                        if (ProdDetails.PacksizeId != null)
                            url += $"&packsizeId={ProdDetails.PacksizeId}";

                        if (barcode != ProdDetails.Barcode)
                            url += $"&barcode={barcode}";

                        await Singleton<Web>.Instance.GetInvokeAsync(url);
                    }
                }

                if (_collectDims && !ProdDetails.IsDecimalControlled)
                {
                    async Task<decimal?> GetDim(string label)
                    {
                        var val = await View.PromptNumeric(label);
                        if (val != 0)
                            return val;
                        return null;
                    }

                    var length = ProdDetails.Length;
                    if (length == null || length == 0)
                    {
                        length = await GetDim($"Enter length [{ProdDetails.LengthUnitOfMeasure}]:");
                        if (length != null)
                            await View.PushMessage($"Length: [{length} {ProdDetails.LengthUnitOfMeasure}]");
                    }

                    var width = ProdDetails.Width;
                    if (width == null || width == 0)
                    {
                        width = await GetDim($"Enter width [{ProdDetails.LengthUnitOfMeasure}]:");
                        if (width != null)
                            await View.PushMessage($"Width: [{width} {ProdDetails.LengthUnitOfMeasure}]");
                    }

                    var height = ProdDetails.Height;
                    if (height == null || height == 0)
                    {
                        height = await GetDim($"Enter height [{ProdDetails.LengthUnitOfMeasure}]:");
                        if(height != null)
                            await View.PushMessage($"Height: [{height} {ProdDetails.LengthUnitOfMeasure}]");
                    }

                    var weight = ProdDetails.Weight;
                    if (weight == null || weight == 0)
                    {
                        weight = await GetDim($"Enter weight [{ProdDetails.WeightUnitOfMeasure}]:");
                        if (weight != null)
                            await View.PushMessage($"Weight: [{weight} {ProdDetails.WeightUnitOfMeasure}]");
                    }

                    if (height != ProdDetails.Height || length != ProdDetails.Length || width != ProdDetails.Width || weight != ProdDetails.Weight)
                    {
                        var url = $"hh/floor/ProductDimensions?productId={ProdDetails.Id}";
                        if (ProdDetails.PacksizeId != null)
                            url += $"&packsizeId={ProdDetails.PacksizeId}";

                        if (length != ProdDetails.Length)
                            url += $"&length={length}";

                        if (width != ProdDetails.Width)
                            url += $"&width={width}";

                        if (height != ProdDetails.Height)
                            url += $"&height={height}";
                        
                        if (weight != ProdDetails.Weight)
                            url += $"&weight={weight}";

                        await Singleton<Web>.Instance.GetInvokeAsync(url);
                    }
                }

                _poLines = _pos.SelectMany(c => c.Lines).Where(c => c.OutstandingQuantity > 0).Where(c => c.ProductId == ProdDetails.Id).ToList();
                if (!_poLines.Any())
                    throw new ExceptionLocalized($"No lines of [{ProdDetails.Sku}] to receive");

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
                var message = string.Join("\n", _poLines.OrderBy(c => c.LineNumber).Select(c => Lang.Translate($"Line [{c.LineNumber}] Qty [{c.OutstandingQuantity}] expected")));
                if (ProdDetails.IsPacksizeControlled)
                    message = string.Join("\n", _poLines.OrderBy(c => c.LineNumber).Select(c => $@"{Lang.Translate($"Line [{c.LineNumber}]")}
{Lang.Translate($"[{(int) (c.OutstandingQuantity / (c.Packsize ?? 1))}] pack(s) of [x{c.Packsize}] expected")}"));
                if (_pos.Count > 1)
                    message = $@"{Lang.Translate($"PO [{_poLines.First().PurchaseOrderNumber}]")}
{message}";
                if (_poLines.Count > 1)
                    message += $"\n{Lang.Translate($"Total [{_poLines.Sum(c => c.OutstandingQuantity)}]")}";
                await View.PushMessage(message, null, false);
            }

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskToBinLpn();
            else
                await AskQuantity();
        }

        protected override Func<Task> ProductReady => async () => await AskToBinLpn();
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => AskProduct;

        private async Task AskToBinLpn(bool showBins = true)
        {
            if (showBins && Singleton<Context>.Instance.IsSuggestPutawayReceiving)
            {
                var bins = await Singleton<Web>.Instance.PostInvokeAsync<List<string>>($"hh/lookup/SuggestPutaway?productId={ProdDetails.Id}&warehouseId={_pos.First().WarehouseId}&source=receiving", ProdOperation);
                if (bins.Any())
                    await View.PushMessage(string.Join("\n", bins), null, false);
            }

            _toBinLpnLookupDetails = await LocationLookup(async () => await AskToBinLpn(), "Scan to Bin/LPN...", BinDirection.In);
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
                foreach (var poLine in _poLines.OrderBy(c=>c.LineNumber))
                {
                    if (ProdOperation.Quantity <= 0)
                        break;
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/receive/ReceivePo?poLineId={poLine.Id}&{_toBinLpnLookupDetails.QueryUrl}", ProdOperation);

                    var message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] received!");
                    if(ProdDetails.PacksizeId != null)
                        message = Lang.Translate($"[{ProdDetails.Sku}] - [{ProdOperation.Quantity}] pack(s) of [x{ProdDetails.EachCount}] received!");
                    
                    await View.PushMessage(message, null, false);

                    if (_isPrintLabels && await View.PromptBool("Print labels?", "Yes", "No"))
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

                    poLine.ReceivedQuantity += ProdOperation.Quantity * (ProdDetails.EachCount ?? 1);
                    originalEntered -= ProdOperation.Quantity;
                    
                    ProdOperation.Quantity = originalEntered;
                    if (poLine.OutstandingQuantity > 0) 
                        continue;

                    await View.PushMessage($"Line [{poLine.LineNumber}] of [{poLine.PurchaseOrderNumber}] received in full!");
                    var po = _pos.Single(c => c.Id == poLine.PurchaseOrderId);
                    po.Lines.Remove(poLine);
                    if (po.Lines.Any()) 
                        continue;

                    await View.PushMessage($"PO [{poLine.PurchaseOrderNumber}] received in full!");
                    _pos.Remove(po);
                }

                View.InactivateMessages();
                Func<Task> next = AskProduct;
                if(ProdDetails.IsSerialControlled)
                    next = AskSerial;
                if (!_pos.Any())
                    next = Complete;
                await next();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBinLpn(false);
            }
        }
        
        protected async Task Complete()
        {
            if (_pos.Any())
            {
                await View.PushMessage($@"{Lang.Translate("Are you sure?")}
{Lang.Translate($"[{_pos.Sum(c => c.Lines.Sum(c1 => c1.OutstandingQuantity))}] outstanding")}", null, false);
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
                await Singleton<Web>.Instance.PostInvokeAsync("hh/receive/Complete", _pos.Select(c => c.Id).ToList());
                await View.PushMessage("All documents completed!");
                await View.PromptBool("Confirm", "OK");
                await CollectPos();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message, Complete);
            }
        }
    }
}