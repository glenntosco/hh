using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Adjustments
{
    [ViewController("main.vendReturnAdjustOut")]
    public class VendorReturn : ProductScanController
    {
        public override string Title => "Vendor return";

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
                Reason = await ReasonCodeLookup(AskReasonCode, "VendorReturn")
            };
            await AskVendor();
        }

        protected async Task AskVendor()
        {
            try
            {
                var vendorCode = await View.PromptString("Enter vendor");
                var details = await Singleton<Web>.Instance.GetInvokeAsync<GenericMessage>($"hh/lookup/VendorLookup?vendorCode={vendorCode}");
                await View.PushMessage(details.Details, AskVendor, false);
                _refId = details.Id;
                await AskReference();
            }
            catch (Exception e)
            {
                await View.PushError(e.Message);
                await AskVendor();
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
            _binLookupDetails = await LocationLookup(AskBinLpn, "Scan Bin/LPN...", BinDirection.Out);
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

                var url = $"hh/adjust/VendorReturn?{_binLookupDetails.QueryUrl}";
                await Singleton<Web>.Instance.PostInvokeAsync<AuditRec>(url, ProdOperation);
                View.InactivateMessages();
                await View.PushMessage("Removed!");
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