using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Bulk
{
    [ViewController("main.licensePlateMove")]
    public class LicensePlateMove : ScanScreenController
    {
        public override string Title => "LPN move";

        private LocationLookup _fromLpnLookupDetails;
        private LocationLookup _toLocationDetails;
        
        protected override async Task Init()
        {
            if (AssignedTask != null)
                await View.PushMessage($@"{Lang.Translate($"Move LPN [{AssignedTask.ReferenceNumber}]")}
{Lang.Translate($"To [{AssignedTask.Details.To}]")}", null, false);

            await AskLpn();
        }

        private async Task AskLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromLpnLookupDetails = await LocationLookup(AskLpn, "Scan LPN...");
                if (!_fromLpnLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan bin, LPN expected");
                if (AssignedTask != null && _fromLpnLookupDetails.Id != AssignedTask.ReferenceId)
                    throw new ExceptionLocalized($"Invalid LPN [{_fromLpnLookupDetails.LpnCode}], expected [{AssignedTask.ReferenceNumber}]");
            }, AskToBin);

            await AskToBin();
        }

        private async Task AskToBin()
        {
            await LoopUntilGood(async () =>
            {
                _toLocationDetails = await LocationLookup(AskToBin, "Scan destination...", BinDirection.In, Singleton<Context>.Instance.AllowLpnOnFloor);
                if (_toLocationDetails?.IsLpn == true)
                    throw new ExceptionLocalized("Cannot move LPN to an LPN, Use Product move instead");
                if(_toLocationDetails?.IsBin == true && _toLocationDetails.ProductHandlingEnum != ProductHandlingType.ByLpn)
                    throw new ExceptionLocalized("Cannot scan Product bin, LPN can only be moved to Bulk bins");
                if(_toLocationDetails?.IsDockDoor == true && !_fromLpnLookupDetails.Totes.Any())
                    throw new ExceptionLocalized("Only LPN containing totes can be moved to Dock door");
                if (AssignedTask != null && _toLocationDetails?.Id != AssignedTask.Details.ToId)
                    throw new ExceptionLocalized($"Invalid To [{_toLocationDetails?.LocationCode ?? "Floor"}], expected [{AssignedTask.Details.To}]");
            }, AskToBin);

            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var url = $"hh/floor/LicensePlateMove?{_fromLpnLookupDetails.QueryUrl}";
                if (_toLocationDetails != null)
                    url += $"&{_toLocationDetails.QueryUrl}";
                if (AssignedTask != null)
                    url += $"&taskId={AssignedTask.Id}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);
                View.InactivateMessages();
                await View.PushMessage($"Moved to [{_toLocationDetails?.LocationCode ?? "Floor"}]!");
                await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                await AskLpn();
            }
        }
    }
}