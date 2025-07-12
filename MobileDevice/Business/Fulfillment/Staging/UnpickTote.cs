using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.unpickTote")]
    public class UnpickTote : ScanScreenController
    {
        public override string Title => "Unpick tote";

        private string _reason;
        private LocationLookup _toBinLpnLookupDetails;
        private ToteLookup _tote;
        
        protected override async Task Init()
        {
            await LoopUntilGood(async () =>
            {
                _tote = await ToteLookup(Init);
                //if (_tote.Lines.Sum(c => c.PickedQuantity) <= 0)
                //    throw new ExceptionLocalized($"Tote [{_tote.Sscc18Code}] is empty");
            }, Init);
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            _toBinLpnLookupDetails = await LocationLookup(AskToBinLpn, "Scan to bin...", BinDirection.In);
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            _reason = await ReasonCodeLookup(AskReasonCode, "Unpick");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/fulfillment/UnpickTote?toteId={_tote.Id}&{_toBinLpnLookupDetails.QueryUrl}&reason={_reason}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                View.InactivateMessages();
                await View.PushMessage($"Tote [{_tote.Sscc18Code}] unpicked to [{_toBinLpnLookupDetails.LocationCode}]");
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