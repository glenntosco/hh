using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Production
{
    [ViewController("main.productionOrderProduce")]
    public class ProductionOrderProduce : ProductScanController
    {
        private ProductionOrder _prodOrder;
        private ProducibleLine _lineToBuild;
        private LocationLookup _toBinLpnLookupDetails;
        public override string Title => "Production order";

        protected override async Task Init()
        {
            await AskSubstOrder();
        }

        public async Task AskSubstOrder()
        {
            await LoopUntilGood(async () =>
            {
                _prodOrder = await ProductionOrderLookup();
                if (!_prodOrder.CanComplete)
                    throw new ExceptionLocalized($"Production order [{_prodOrder.ProductionOrderNumber}]: Cannot produce, order is not in final workflow step");
            }, AskSubstOrder);

            await View.PushMessage(@$"{_prodOrder.ProductionOrderNumber}
{Lang.Translate($"SKUs to build: [{_prodOrder.InLines.Where(c => c.Quantity - (c.ProducedQuantity ?? 0) > 0).Select(c => c.Product.Sku).Distinct().Count()}]")}
{Lang.Translate($"Quantity to build: [{_prodOrder.InLines.Sum(c => c.Quantity - (c.ProducedQuantity ?? 0))}]")}", null, false);

            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            await LoopUntilGood(async () =>
            {
                FinishSerialButton = View.RemoveToolbar(FinishSerialButton);

                ProdDetails = await ProductLookup(AskProductOp);

                if (Singleton<Context>.Instance.ProdOrderCollectBarcode)
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

                if (Singleton<Context>.Instance.ProdOrderCollectDims && !ProdDetails.IsDecimalControlled)
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

                _lineToBuild = _prodOrder.InLines.SingleOrDefault(c => c.Product.Id == ProdDetails.Id);
                if (_lineToBuild == null)
                    throw new ExceptionLocalized($"Invalid SKU [{ProdDetails.Sku}]. Product is not planned for production");
                await View.PushMessage(@$"Outstanding quantity: [{_lineToBuild.Quantity - (_lineToBuild.ProducedQuantity ?? 0)}]");

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
                    await AskQuantity(false);

                if (ProdOperation.Quantity * (ProdOperation.PacksizeEachCount ?? 1) > _lineToBuild.Quantity - (_lineToBuild.ProducedQuantity ?? 0))
                    throw new ExceptionLocalized($"Invalid quantity. Expected [{_lineToBuild.Quantity - (_lineToBuild.ProducedQuantity ?? 0)}]");
            }, AskProductOp);

            await AskToBinLpn();
        }
        
        protected override Func<Task> ProductReady => AskToBinLpn;
        protected override Func<Task> SerialReady => AskToBinLpn;
        protected override Func<Task> NoMoreSerials => AskProductOp;

        private async Task AskToBinLpn()
        {
            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
            await Process();
        }

        private async Task Process()
        {
            try
            {
                var url = $"hh/production/BuildProductionOrder?prodOrderId={_prodOrder.Id}&{_toBinLpnLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                View.InactivateMessages();
                
                _lineToBuild.ProducedQuantity = (_lineToBuild.ProducedQuantity ?? 0) + ProdOperation.Quantity * (ProdDetails.EachCount ?? 1);

                var message = Lang.Translate($"Produced [{ProdOperation.Quantity}]");

                if (Singleton<Context>.Instance.ProdOrderPrintLabels && !ProdDetails.IsDecimalControlled && await View.PromptBool("Print labels?", "Yes", "No"))
                {
                    var labelCount = (int)await View.PromptNumeric("Number of labels", ProdOperation.Quantity);
                    var labelPrintPayload = new ProductOperation
                    {
                        ProductId = ProdOperation.ProductId,
                        PacksizeId = ProdOperation.PacksizeId,
                        LotNumber = ProdOperation.LotNumber,
                        Expiry = ProdOperation.Expiry,
                        SerialNumber = ProdOperation.SerialNumber,
                        Quantity = labelCount
                    };
                    await Singleton<Web>.Instance.PostInvokeAsync($"hh/production/PrintProductionOrderLabels", labelPrintPayload);
                }

                var finished = _prodOrder.InLines.All(c => c.ProducedQuantity >= c.Quantity);

                if (finished)
                    message += $"\n{Lang.Translate($"Production order [{_prodOrder.ProductionOrderNumber}] built in full")}";
                await View.PushMessage(message, null, false);

                if (finished)
                    await AskSubstOrder();
                else
                    await AskProductOp();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBinLpn();
            }
        }
    }
}