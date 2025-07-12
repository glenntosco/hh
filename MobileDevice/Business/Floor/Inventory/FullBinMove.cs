using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Inventory
{
    [ViewController("main.fullBinMove")]
    public class FullBinMove : ScanScreenController
    {
        public override string Title => "Full bin move";

        private LocationLookup _fromBinLookupDetails;
        private LocationLookup _toBinLookupDetails;
        
        protected override async Task Init()
        {
            if (AssignedTask != null)
                await View.PushMessage($@"{Lang.Translate($"Move all from [{AssignedTask.ReferenceNumber}]")}
{Lang.Translate($"To [{AssignedTask.Details.To}]")}", null, false);

            await AskFromBinLpn();
        }

        private async Task AskFromBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromBinLookupDetails = await LocationLookup(AskFromBinLpn, "Scan from Bin/LPN...", BinDirection.Out);
                if (AssignedTask != null && _fromBinLookupDetails.Id != AssignedTask.Details.FromId)
                    throw new ExceptionLocalized($"Invalid From [{_fromBinLookupDetails.LocationCode}], expected [{AssignedTask.Details.From}]");
            });
            await AskToBinLpn();
        }

        private async Task AskToBinLpn()
        {
            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = await LocationLookup(AskToBinLpn, "Scan to Bin/LPN...", BinDirection.In);
                if (AssignedTask != null && _toBinLookupDetails.Id != AssignedTask.Details.ToId)
                    throw new ExceptionLocalized($"Invalid To [{_toBinLookupDetails.LocationCode}], expected [{AssignedTask.Details.To}]");
            });
            await Process();
        }
        
        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/FullBinMove?{_fromBinLookupDetails.QueryUrl}&{_toBinLookupDetails.QueryUrl}";
                if (AssignedTask != null)
                    url += $"&taskId={AssignedTask.Id}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                await View.PushMessage("Moved!");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                View.InactivateMessages();
                await AskFromBinLpn();
            }
        }
    }
}