using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.packsizeConvert")]
    public class PacksizeConvert : ScanScreenController
    {
        public override string Title => "Packsize convert";

        private LocationLookup _fromBinLpnLookupDetails;
        private LocationLookup _toBinLpnLookupDetails;

        private ProductDetails _prodDetails;
        private PacksizeConvertOperation _op;

        protected override async Task Init()
        {
            _op = new PacksizeConvertOperation();
            _prodDetails = null;
            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
            _fromBinLpnLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
            _op.FromBinId = _fromBinLpnLookupDetails.IsBin ? _fromBinLpnLookupDetails.Id : null;
            _op.FromLpn = _fromBinLpnLookupDetails.IsLpn? _fromBinLpnLookupDetails.LocationCode : null;
            await AskProduct();
        }

        protected async Task AskProduct()
        {
            await LoopUntilGood(async () =>
            {
                _prodDetails = await ProductLookup(AskProduct, null, false);
                if (!_prodDetails.IsPacksizeControlled)
                    throw new ExceptionLocalized($"Product [{_prodDetails.Sku}] is not packsize controlled");
                if(_prodDetails.Packsizes.Count == 1)
                    throw new ExceptionLocalized($"Product [{_prodDetails.Sku}] is setup with only one Packsize");

                _op.ProductId = _prodDetails.Id;
                if (_prodDetails.IsLotControlled)
                    await AskLot();

                if(_prodDetails.IsExpiryControlled)
                    await AskExpiry();

                if (_prodDetails.PacksizeId == null)
                    await AskFromPacksize();
                else
                {
                    _op.FromPacksizeId = _prodDetails.PacksizeId.Value;
                    await AskToBinLpn();
                }
            }, AskProduct);
        }

        protected async Task AskLot()
        {
            _op.LotNumber = await PromptLot(AskLot);
        }

        protected async Task AskExpiry()
        {
            _op.Expiry = await PromptExpiry(AskExpiry);
        }

        protected async Task AskFromPacksize()
        {
            var fromPacksize = await View.PromptPacksize("From packsize", _prodDetails.Packsizes);
            await View.PushMessage($"From packsize: [x{fromPacksize.EachCount}]", AskFromPacksize);
            _op.FromPacksizeId = fromPacksize.Id;
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
            _op.ToBinId = _toBinLpnLookupDetails.IsBin ? _toBinLpnLookupDetails.Id : _toBinLpnLookupDetails.LpnBinId;
            _op.ToLpn = _toBinLpnLookupDetails.IsLpn ? _toBinLpnLookupDetails.LocationCode : null;
            await AskToPacksize();
        }
        
        protected async Task AskToPacksize()
        {
            var toPacksize = await View.PromptPacksize("To packsize", _prodDetails.Packsizes);

            _op.ToPacksizeId = toPacksize.Id;
            await View.PushMessage($"To packsize: [x{toPacksize.EachCount}]", AskToPacksize);
            await AskQuantity();
        }

        protected async Task AskQuantity()
        {
            _op.Quantity = (int) await PromptQuantity(AskQuantity);
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                await Singleton<Web>.Instance.PostInvokeAsync($"hh/floor/PacksizeConvert", _op);
                View.InactivateMessages();
                await View.PushMessage("Converted!");
                await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskQuantity();
            }
        }
    }
}