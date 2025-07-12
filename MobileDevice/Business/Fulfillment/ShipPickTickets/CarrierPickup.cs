using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.ShipPickTickets
{
    [ViewController("main.carrierPickup")]
    public class CarrierPickup : ScanScreenController
    {
        public override string Title => "Carrier pickup";

        private CarrierShipLicensePlate _lpnLookup;
        
        protected override async Task Init()
        {
            _lpnLookup = null;
            await PromptLpn();
        }

        private async Task PromptLpn()
        {
            await LoopUntilGood(async () =>
            {
                _lpnLookup = await CarrierLpnLookup();
                var message = $@"{_lpnLookup.LicensePlateCode}
{Lang.Translate($"Carrier: [{_lpnLookup.Carrier}]")}
{Lang.Translate($"Orders [{_lpnLookup.OrdersCount}]")}
{Lang.Translate($"Totes: [{_lpnLookup.CartonsCount}]")}";
                await View.PushMessage(message, null, false);
            }, PromptLpn);

            await Process();
        }

        protected async Task Process()
        {
            try
            {
                if (!await View.PromptBool("Ship?", "Yes", "No"))
                    View.ClearMessages();
                else
                {
                    await Singleton<Web>.Instance.PostInvokeAsync($"api/ToteMasterApi/SmallParcelCarrierPickup?manifest={_lpnLookup.LicensePlateCode}", _lpnLookup.ToteIds);

                    View.InactivateMessages();
                    await View.PushMessage("Shipped!");
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await Init();
            }
        }
    }
}