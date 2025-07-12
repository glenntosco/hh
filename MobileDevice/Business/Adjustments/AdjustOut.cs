using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Adjustments
{
    [ViewController("main.adjustOutByBin")]
    public class AdjustOutByBin : AdjustOut
    {
        public override string Title => "Adjust out by Bin";
        protected override async Task Finish()
        {
            await AskProductOp();
        }
    }

    [ViewController("main.adjustOut")]
    public class AdjustOutRegular : AdjustOut
    {
        public override string Title => "Adjust out";
        protected override async Task Finish()
        {
            await Init();
        }
    }

    public abstract class AdjustOut : ProductScanController
    {
        protected abstract Task Finish();

        private LocationLookup _fromBinLpnLookupDetails;
        protected override async Task Init()
        {
            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
            _fromBinLpnLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => AskReasonCode;
        protected override Func<Task> SerialReady => AskReasonCode;
        protected override Func<Task> NoMoreSerials => Finish;

        protected async Task AskReasonCode()
        {
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "AdjustOut");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                var url = $"hh/adjust/AdjustOut?{_fromBinLpnLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                View.InactivateMessages();
                await View.PushMessage("Removed!");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await Finish();
            }
        }
    }
}