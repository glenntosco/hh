using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Products
{
    [ViewController("main.productDimsCollect")]
    public class ProductDimsCollect : ScanScreenController
    {
        public override string Title => "Product dimensions";

        private ProductDetails _prodDetails;
        private string _barcode;
        private decimal? _height;
        private decimal? _length;
        private decimal? _width;
        private decimal? _weight;

        protected override async Task Init()
        {
            await AskProduct();
        }

        protected async Task AskProduct()
        {
            _prodDetails = await ProductLookup(AskProduct);
            var msg = $@"{Lang.Translate($"Barcode: [{_prodDetails.Barcode}]")}";

            if (!_prodDetails.IsDecimalControlled)
            {
                msg += @$"
{Lang.Translate($"Length: [{_prodDetails.Length ?? 0} {_prodDetails.LengthUnitOfMeasure}]")}
{Lang.Translate($"Width: [{_prodDetails.Width ?? 0} {_prodDetails.LengthUnitOfMeasure}]")}
{Lang.Translate($"Height: [{_prodDetails.Height ?? 0} {_prodDetails.LengthUnitOfMeasure}]")}
{Lang.Translate($"Weight: [{_prodDetails.Weight ?? 0} {_prodDetails.WeightUnitOfMeasure}]")}";
            }

            await View.PushMessage(msg, AskProduct, false);
            await AskBarcode();
        }

        protected async Task AskBarcode()
        {
            _barcode = await View.PromptString("Scan barcode:");
            if (_barcode != null)
                await View.PushMessage($"Barcode: [{_barcode}]");
            if(_prodDetails.IsDecimalControlled)
                await Process();
            else
                await AskLength();
        }

        protected async Task AskLength()
        {
            _length = await GetDim($"Enter length [{_prodDetails.LengthUnitOfMeasure}]:");
            if (_length != null)
                await View.PushMessage($"Length: [{_length} {_prodDetails.LengthUnitOfMeasure}]");
            await AskWidth();
        }

        protected async Task AskWidth()
        {
            _width = await GetDim($"Enter width [{_prodDetails.LengthUnitOfMeasure}]:");
            if (_width != null)
                await View.PushMessage($"Width: [{_width} {_prodDetails.LengthUnitOfMeasure}]");
            await AskHeight();
        }

        protected async Task AskHeight()
        {
            _height = await GetDim($"Enter height [{_prodDetails.LengthUnitOfMeasure}]:");
            if (_height != null)
                await View.PushMessage($"Height: [{_height} {_prodDetails.LengthUnitOfMeasure}]");
            await AskWeight();
        }

        protected async Task AskWeight()
        {
            _weight = await GetDim($"Enter weight [{_prodDetails.WeightUnitOfMeasure}]:");
            if (_weight != null)
                await View.PushMessage($"Weight: [{_weight} {_prodDetails.WeightUnitOfMeasure}]");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/ProductDimensions?productId={_prodDetails.Id}";
                if (_prodDetails.PacksizeId != null)
                    url += $"&packsizeId={_prodDetails.PacksizeId}";

                if (!string.IsNullOrWhiteSpace(_barcode) && _barcode != _prodDetails.Barcode)
                    url += $"&barcode={_barcode}";

                if (_height != null && _height != 0 && _height != _prodDetails.Height)
                    url += $"&height={_height}";

                if (_length != null && _length != 0 && _length != _prodDetails.Length)
                    url += $"&length={_length}";

                if (_width != null && _width != 0 && _width != _prodDetails.Width)
                    url += $"&width={_width}";

                if (_weight != null && _weight != 0 && _weight != _prodDetails.Weight)
                    url += $"&weight={_weight}";
                
                View.InactivateMessages();
                await Singleton<Web>.Instance.GetInvokeAsync(url);

                _prodDetails = null;
                _barcode = null;
                _height = null;
                _length = null;
                _width = null;
                _weight = null;

                await AskProduct();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
        }

        private async Task<decimal?> GetDim(string label)
        {
            var val = await View.PromptNumeric(label);
            if (val != 0)
                return val;
            return null;
        }
    }
}