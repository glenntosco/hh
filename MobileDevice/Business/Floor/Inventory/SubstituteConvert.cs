using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.substituteConvert")]
    public class SubstituteConvert : ProductScanController
    {
        public override string Title => "Substitute conversion";

        string _actionId;
        private SubstituteConvertRequest _instructions;

        private ProductDetails _fromProdExpected;
        private ProductOperation _fromProdOperation;
        private LocationLookup _fromBinLookupDetails;

        private ProductDetails _toProdExpected;
        private ProductOperation _toProdOperation;
        private LocationLookup _toBinLookupDetails;

        protected override async Task Init()
        {
            await AskInstructions();
        }

        private async Task AskInstructions()
        {
            _fromProdOperation = null;
            _toProdOperation = null;
            
            await LoopUntilGood(async () =>
            {
                _actionId = await View.PromptScan("Scan instruction");
                var payload = await Singleton<Web>.Instance.GetInvokeAsync<string>($"hh/lookup/WarehouseActionLookup?key={_actionId}");
                if (string.IsNullOrWhiteSpace(payload))
                    throw new ExceptionLocalized($"Invalid instruction [{_actionId}]");
                _instructions = Utils.DeserializeFromJson<SubstituteConvertRequest>(payload);
            }, AskInstructions);

            _fromProdExpected = await Singleton<Web>.Instance.GetInvokeAsync<ProductDetails>($"hh/lookup/ProductLookupById?productId={_instructions.FromProductId}");
            _toProdExpected = await Singleton<Web>.Instance.GetInvokeAsync<ProductDetails>($"hh/lookup/ProductLookupById?productId={_instructions.ToProductId}");

            var message = $@"{Lang.Translate($"Consume: [{_fromProdExpected.Sku}]")}
{Lang.Translate($"From: {_instructions.GetPickBin()}")}";

            if (_instructions.Consume != null)
                message += $"\n{Lang.Translate($"Quantity: [{_instructions.Consume} {_fromProdExpected.UnitOfMeasure?.ToString() ?? "Ea"}]")}";

            message += $"\n\n{_fromProdExpected.Description}";

            if (string.IsNullOrWhiteSpace(_instructions.FromImageUrl))
                await View.PushMessage(message, null, false);
            else
                await View.PushThumbnailMessage(message, _instructions.FromImageUrl, null, false);

            message = @$"{Lang.Translate($"Produce: [{_toProdExpected.Sku}]")}";
            if (_instructions.Produce != null)
                message += $"\n{Lang.Translate($"Quantity: [{_instructions.Produce} {_toProdExpected.UnitOfMeasure?.ToString() ?? "Ea"}]")}";

            message += $"\n\n{_toProdExpected.Description}";

            if (string.IsNullOrWhiteSpace(_instructions.ToImageUrl))
                await View.PushMessage(message, null, false);
            else
                await View.PushThumbnailMessage(message, _instructions.ToImageUrl, null, false);

            if (!string.IsNullOrWhiteSpace(_instructions.Instructions))
                await View.PushMessage(Lang.Translate($"Instructions: ") + _instructions.Instructions, null, false);

            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromBinLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (_fromBinLookupDetails.LocationCode != (_instructions.BinCode ?? _instructions.LicensePlateCode))
                    throw new ExceptionLocalized($"Invalid bin, {_instructions.GetPickBin()} expected");
            }, AskFromBinLpn);

            await AskProductOp();
        }

        protected override async Task AskProductOp()
        {
            await LoopUntilGood(async () =>
            {
                ProdDetails = await ProductLookup(AskProductOp);

                if (_fromProdOperation == null)
                {
                    if (ProdDetails.Id != _fromProdExpected.Id)
                        throw new ExceptionLocalized($"Invalid source product, [{_fromProdExpected.Sku}] expected");
                }
                else if (_toProdOperation == null)
                {
                    if (ProdDetails.Id != _toProdExpected.Id)
                        throw new ExceptionLocalized($"Invalid destination product, [{_toProdExpected.Sku}] expected");
                }
            }, AskProductOp);

            ProdOperation ??= new ProductOperation();
            ProdOperation.ProductId = ProdDetails.Id;
            ProdOperation.PacksizeId = ProdDetails.PacksizeId;

            if (ProdDetails.IsLotControlled)
                await AskLot();

            if (ProdDetails.IsExpiryControlled)
                await AskExpiry();

            if (ProdDetails.IsSerialControlled)
                await AskSerial();
            else
                await AskQuantity();
        }

        protected override Func<Task> ProductReady => AskToBinLpn;
        protected override Func<Task> SerialReady => AskToBinLpn;
        protected override Func<Task> NoMoreSerials => null;

        private async Task AskToBinLpn()
        {
            if (_fromProdOperation == null)
            {
                _fromProdOperation = PropMapper<ProductOperation, ProductOperation>.From(ProdOperation);
                _toProdOperation = null;
            }
            else if (_toProdOperation == null)
                _toProdOperation = PropMapper<ProductOperation, ProductOperation>.From(ProdOperation);

            ProdOperation = null;

            if (_toProdOperation == null)
            {
                _toBinLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
                await AskProductOp();
            }
            else
                await Process();
        }

        protected async Task Process()
        {
            try
            {
                var payload = new SubstituteConvertAction
                {
                    ToBinId = _toBinLookupDetails.BinId ?? _toBinLookupDetails.LpnBinId,
                    ToLpn = _toBinLookupDetails.LpnCode,
                    ToOperation = _toProdOperation,

                    FromBinId = _fromBinLookupDetails.BinId,
                    FromLpn = _fromBinLookupDetails.LpnCode,
                    FromOperation = _fromProdOperation
                };

                await Singleton<Web>.Instance.PostInvokeAsync($"hh/floor/SubstituteConvert", payload);

                _actionId = null;
                _instructions = null;

                _fromProdExpected = null;
                _fromProdOperation = null;
                _fromBinLookupDetails = null;

                _toProdExpected = null;
                _toProdOperation = null;
                _toBinLookupDetails = null;

                View.InactivateMessages();
                await View.PushMessage("Converted!");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await AskInstructions();
            }
        }
    }
}