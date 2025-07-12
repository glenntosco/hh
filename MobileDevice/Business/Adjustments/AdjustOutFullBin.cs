using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Adjustments
{
    [ViewController("main.adjustOutFullBin")]
    public class AdjustOutFullBin : ScanScreenController
    {
        private string _reason;
        private LocationLookup _fromBinLpnLookupDetails;
        public override string Title => "Adjust out full Bin";

        protected override async Task Init()
        {
            _reason = null;
            _fromBinLpnLookupDetails = await LocationLookup(Init, "Scan from Bin/LPN...", BinDirection.Out);
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            _reason = await ReasonCodeLookup(AskReasonCode, "AdjustOut");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/adjust/AdjustOutFullBin?{_fromBinLpnLookupDetails.QueryUrl}&reason={_reason}");
                View.InactivateMessages();
                await View.PushMessage("Bin cleared!");
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