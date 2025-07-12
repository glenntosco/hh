using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Adjustments
{
    [ViewController("main.adjustInByBin")]
    public class AdjustInByBin : AdjustIn
    {
        public override string Title => "Adjust in by Bin";
        protected override async Task Finish()
        {
            await AskProductOp();
        }
    }

    [ViewController("main.adjustIn")]
    public class AdjustInRegular : AdjustIn
    {
        public override string Title => "Adjust in";
        protected override async Task Finish()
        {
            await Init();
        }
    }

    public abstract class AdjustIn : ProductScanController
    {
        protected abstract Task Finish();

        private LocationLookup _toBinLpnLookupDetails;

        protected override async Task Init()
        {
            _toBinLpnLookupDetails = await LocationLookup(Init, "Scan to Bin/LPN...", BinDirection.In);
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => AskReasonCode;
        protected override Func<Task> SerialReady => AskReasonCode;
        protected override Func<Task> NoMoreSerials => Finish;

        protected async Task AskReasonCode()
        {
            ProdOperation.Reason = await ReasonCodeLookup(ProductReady, "AdjustIn");
            await Process();
        }

        private async Task Process()
        {
            try
            {
                var url = $"hh/adjust/AdjustIn?{_toBinLpnLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                View.InactivateMessages();
                await View.PushMessage("Added!");
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