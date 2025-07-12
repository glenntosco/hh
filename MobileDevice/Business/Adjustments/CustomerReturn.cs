using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Adjustments
{
    [ViewController("main.custReturnAdjustIn")]
    public class CustomerReturn : ProductScanController
    {
        public override string Title => "Customer return";

        private LocationLookup _binLookupDetails;

        private Guid? _refId;
        private string _refCode;

        protected override async Task Init()
        {
            _refId = null;
            _refCode = null;
            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            ProdOperation = new ProductOperation
            {
                Reason = await ReasonCodeLookup(AskReasonCode, "CustomerReturn")
            };
            await AskCustomer();
        }

        protected async Task AskCustomer()
        {
            try
            {
                var customerCode = await View.PromptString("Enter customer");
                var details = await Singleton<Web>.Instance.GetInvokeAsync<GenericMessage>($"hh/lookup/CustomerLookup?customerCode={customerCode}");
                await View.PushMessage(details.Details, AskCustomer, false);
                _refId = details.Id;
                await AskReference();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message);
                await AskCustomer();
            }
        }
        
        protected async Task AskReference()
        {
            _refCode = await View.PromptString("Reference");
            await View.PushMessage($"Reference: [{ProdOperation.ReferenceCode}]", AskReference);
            await AskBinLpn();
        }

        private async Task AskBinLpn()
        {
            _binLookupDetails = await LocationLookup(AskBinLpn, "Scan Bin/LPN...", BinDirection.In);
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => Process;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => AskProductOp;

        protected async Task Process()
        {
            try
            {
                ProdOperation.ReferenceCode = _refCode;
                ProdOperation.ReferenceId = _refId;

                var url = $"hh/adjust/CustomerReturn?{_binLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                View.InactivateMessages();
                await View.PushMessage("Added!");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, ProductReady);
            }
            finally
            {
                if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskProductOp();
            }
        }
    }
}