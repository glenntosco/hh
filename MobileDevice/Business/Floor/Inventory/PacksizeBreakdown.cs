using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.packsizeBreakdown")]
    public class PacksizeBreakdown : ScanScreenController
    {
        public override string Title => "Packsize breakdown";

        private LocationLookup _fromBinLookupDetails;
        private LocationLookup _toBinLookupDetails;

        private int _quantity;

        private DirectedPacksizeBreakdown _direction;

        private string _key;
        
        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = null;
                _direction = null;
                if (AssignedTask == null)
                    _key = await View.PromptScan("Scan breakdown ticket...");
                else
                    _key = AssignedTask.ReferenceNumber;
                AssignedTask = null;

                await GetDirection();
            });

            await AskFromBinLpn();
        }

        private async Task GetDirection()
        {
            _direction = await Singleton<Web>.Instance.GetInvokeAsync<DirectedPacksizeBreakdown>($"hh/lookup/PacksizeBreakdownTicketLookup?key={_key}");
            if (_direction == null)
            {
                await View.PushMessage("No pending breakdowns");
                await Init();
                return;
            }

            string message;
            if (_direction.LicensePlate != null)
            {
                message = $"LPN [{_direction.LicensePlate}]";
                if (_direction.Bin != null)
                    message += $" @ [{_direction.Bin}]";
            }
            else
                message = $"Bin [{_direction.Bin}]";

            message += $@"
Sku [{_direction.Product.Sku}]
Break [{_direction.Quantity}] pack(s) of [x{_direction.FromPacksize}] to [{_direction.Quantity * _direction.FromPacksize / _direction.ToPacksize}] pack(s) of [x{_direction.ToPacksize}]

{_direction.Product.Description}";

            if (string.IsNullOrWhiteSpace(_direction.Product.ImageUrl))
                await View.PushMessage(message, null, false);
            else
                await View.PushThumbnailMessage(message, _direction.Product.ImageUrl, null, false);
        }

        private async Task AskFromBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromBinLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (_direction.LicensePlate != null)
                {
                    if(_fromBinLookupDetails.LpnCode != _direction.LicensePlate)
                        throw new ExceptionLocalized($"Invalid From [{_fromBinLookupDetails.LocationCode ?? _fromBinLookupDetails.LpnCode}], expected [{_direction.Bin ?? _direction.LicensePlate}]");
                }
                else
                {
                    if (_fromBinLookupDetails.BinId != _direction.BinId)
                        throw new ExceptionLocalized($"Invalid From [{_fromBinLookupDetails.LocationCode ?? _fromBinLookupDetails.LpnCode}], expected [{_direction.Bin ?? _direction.LicensePlate}]");
                }
            });

            await AskProduct();
        }

        private async Task AskProduct()
        {
            await LoopUntilGood(async () =>
            {
                var prod = await ProductLookup(AskProduct);
                if (prod.Id != _direction.Product.Id)
                    throw new ExceptionLocalized($"Invalid product [{prod.Sku}], [{_direction.Product.Sku}] expected");
                if(prod.EachCount != _direction.FromPacksize)
                    throw new ExceptionLocalized($"Invalid packsize [x{prod.EachCount}], [x{_direction.FromPacksize}] expected");
            }, AskProduct);

            await AskQuantity();
        }

        protected async Task AskQuantity()
        {
            await LoopUntilGood(async () =>
            {
                _quantity = (int)await PromptQuantity(AskQuantity, null, "Enter large pack(s)");
                if (_quantity > _direction.Quantity)
                    throw new ExceptionLocalized($"Invalid quantity [{_direction.Quantity}] expected");
            }, AskProduct);
            
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            _toBinLookupDetails = await LoopUntilGood(async () =>
            {
                var bin = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
                if (!bin.IsLpn && bin.ProductHandlingEnum != ProductHandlingType.ByProduct)
                    throw new ExceptionLocalized($"Cannot scan Bulk bin, Pickable bin expected");
                return bin;
            }, AskToBinLpn);
            await Process();
        }
        
        protected async Task Process()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/floor/PacksizeBreakdown?quantity={_quantity}&reservationDetailId={_direction.Id}&{_toBinLookupDetails.QueryUrl}");
                View.InactivateMessages();
                await View.PushMessage($"[{_quantity}] pack(s) broke down!");

                await GetDirection();

                if (_direction != null)
                    await AskFromBinLpn();
                else
                    await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBinLpn();
            }
        }
    }
}