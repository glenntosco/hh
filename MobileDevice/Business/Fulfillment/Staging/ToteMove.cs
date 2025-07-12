using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.toteMove")]
    public class ToteMove : ScanScreenController
    {
        public override string Title => "Tote move";

        private LocationLookup _toBinLookupDetails;
        private ToteLookup _tote;
        
        protected override async Task Init()
        {
            _tote = await LoopUntilGood(async () =>
            {
                var tote = await ToteLookup(Init);
                var wrongStates = new[] {PickTicketState.Shipped, PickTicketState.Closed};
                if (wrongStates.Contains(tote.PickTicketState ?? PickTicketState.Shipped))
                    throw new ExceptionLocalized($"Tote [{tote.Sscc18Code}] has already been shipped");
                return tote;
            }, Init);
            
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            _toBinLookupDetails = await LocationLookup(AskToBinLpn, "Scan location", BinDirection.In, true);
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/ToteMove?toteId={_tote.Id}";
                if (_toBinLookupDetails != null)
                    url += $"&{_toBinLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                View.InactivateMessages();
                await View.PushMessage($"Tote [{_tote.Sscc18Code}] moved to [{_toBinLookupDetails?.LocationCode ?? "Floor"}]");
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