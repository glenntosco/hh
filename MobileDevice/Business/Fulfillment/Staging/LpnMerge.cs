using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.lpnMerge")]
    public class LpnMerge : ScanScreenController
    {
        public override string Title => "LPN merge";

        private LocationLookup _fromBinLookupDetails;
        private LocationLookup _toBinLookupDetails;

        protected override async Task Init()
        {
            _fromBinLookupDetails = await LocationLookup(Init, "Scan from LPN", BinDirection.Out);
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            _toBinLookupDetails = await LocationLookup(AskToBinLpn, "Scan to LPN", BinDirection.In);
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/fulfillment/LpnMerge?{_fromBinLookupDetails.QueryUrl}&{_toBinLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                View.InactivateMessages();
                await View.PushMessage($"LPN [{_fromBinLookupDetails.LocationCode}] merged with [{_toBinLookupDetails.LocationCode}]");
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