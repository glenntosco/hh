using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Floor.Bulk
{
    [ViewController("main.letdownLpnByBin")]
    public class LetdownLpnByBin: LetdownLpn
    {
        public override LetdownSequence LetdownSequence => LetdownSequence.ByBin;
        public override string Title => "LPN letdown by Bin";
    }

    [ViewController("main.letdownLpnBySku")]
    public class LetdownLpnBySku : LetdownLpn
    {
        public override LetdownSequence LetdownSequence => LetdownSequence.BySku;
        public override string Title => "LPN letdown by product";
    }

    public abstract class LetdownLpn : ScanScreenController
    {
        private LocationLookup _fromLpnLookupDetails;
        private LocationLookup _toBinLookupDetails;

        private string _key;

        public abstract LetdownSequence LetdownSequence { get; }

        protected override async Task Init()
        {
            await AskLetdownTicket();
        }

        protected async Task AskLetdownTicket()
        {
            if (AssignedTask == null)
                _key = await View.PromptString("Letdown ticket");
            else
                _key = AssignedTask.ReferenceNumber;
            AssignedTask = null;
            await CheckLetdownTicket(_key);
        }

        private async Task CheckLetdownTicket(string key)
        {
            var url = $"hh/lookup/LpnLetdownLookup?lookupType={LetdownSequence}";
            if (!string.IsNullOrWhiteSpace(key))
                url += $"&key={key}";
            var message = await Singleton<Web>.Instance.GetInvokeAsync<string>(url);
            if (string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(key))
                    await View.PushMessage("No pending letdowns");
                else
                    await View.PushMessage($"No pending letdowns for [{key}]");
                await AskLetdownTicket();
                return;
            }
            await View.PushMessage(message, null, false);
            await AskLpn();
        }

        private async Task AskLpn()
        {
            await LoopUntilGood(async () =>
            {
                _fromLpnLookupDetails = await LocationLookup(AskLpn, "Scan LPN...");
                if (!_fromLpnLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan bin, LPN expected");
            }, AskLpn);
            
            await AskToBin();
        }

        private async Task AskToBin()
        {
            await LoopUntilGood(async () =>
            {
                _toBinLookupDetails = await LocationLookup(AskToBin, "Scan to bin...", BinDirection.In);
                if (_toBinLookupDetails.IsLpn)
                    throw new ExceptionLocalized("Cannot scan LPN, Bin expected");
            }, AskToBin);

            await Process();
        }
        
        protected async Task Process()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"hh/floor/LetdownLpn?{_fromLpnLookupDetails.QueryUrl}&{_toBinLookupDetails.QueryUrl}");
                View.InactivateMessages();
                await View.PushMessage($"[{_fromLpnLookupDetails.LocationCode}] replenished [{_toBinLookupDetails?.LocationCode}]");
                await CheckLetdownTicket(_key);
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToBin();
            }
        }
    }
}