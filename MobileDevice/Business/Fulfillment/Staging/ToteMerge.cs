using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.toteMerge")]
    public class ToteMerge : ScanScreenController
    {
        public override string Title => "Tote merge";
        
        private Func<Task> _lastFunc;
        private ToteLookup _fromTote;
        private ToteLookup _toTote;
        private Button _printTote;
        private string _reason;

        protected override async Task Init()
        {
            _fromTote = null;
            _toTote = null;
            _reason = null;

            await AskFromTote();
        }

        protected async Task AskFromTote()
        {
            _lastFunc = AskFromTote;
            _printTote = View.RemoveToolbar(_printTote);
            _fromTote = await LoopUntilGood(async () =>
            {
                var tote = await ToteLookup(Init, null, "From tote...");
                var wrongStates = new[] {PickTicketState.Shipped, PickTicketState.Closed};
                if (wrongStates.Contains(tote.PickTicketState ?? PickTicketState.Shipped))
                    throw new ExceptionLocalized($"Tote [{tote.Sscc18Code}] has already been shipped");
                return tote;
            }, Init);

            _printTote ??= View.AddToolbar("Print tote", PrintTote);
            await AskToTote();
        }

        private async Task AskToTote()
        {
            _lastFunc = AskToTote;
            _toTote = await LoopUntilGood(async () =>
            {
                var tote = await ToteLookup(Init, null, "To tote...");
                if(tote.Id == _fromTote.Id)
                    throw new ExceptionLocalized($"From and To totes are the same");
                if (tote.PickTicketId != _fromTote.PickTicketId)
                    throw new ExceptionLocalized($"Cannot merge totes from different pick tickets");
                return tote;
            }, AskToTote);

            await AskReasonCode();
        }

        protected async Task AskReasonCode()
        {
            _lastFunc = AskReasonCode;
            _reason = await ReasonCodeLookup(AskReasonCode, "Repackage");
            await Process();
        }

        protected async Task Process()
        {
            try
            {
                await Singleton<Web>.Instance.PostInvokeAsync($"hh/fulfillment/ToteMerge?fromToteId={_fromTote.Id}&toToteId={_toTote.Id}", new ReasonOperation
                {
                    Reason = _reason
                });
                View.InactivateMessages();
                await View.PushMessage($"Tote [{_fromTote.Sscc18Code}] merged to [{_toTote.Sscc18Code}]");
                await Init();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskToTote();
            }
        }

        private async Task PrintTote()
        {
            try
            {
                var cartonSizes = await Singleton<Web>.Instance.GetInvokeAsync<List<IdName>>($"hh/lookup/GetCartonSizes");
                var url = $"hh/fulfillment/PrintTote?pickTicketId={_fromTote.PickTicketId}";
                if (cartonSizes.Any())
                {
                    if (cartonSizes.Count == 1)
                        url += $"&cartonSizeId={cartonSizes.First().Id}";
                    else
                    {
                        var name = await View.PromptPicker("Carton size", cartonSizes.Select(c => c.Name).OrderBy(c => c).ToList());
                        url += $"&cartonSizeId={cartonSizes.First(c => c.Name == name).Id}";
                    }
                }

                var tote = await Singleton<Web>.Instance.GetInvokeAsync<ToteLookup>(url);
                await View.PushMessage($@"[{tote.Sscc18Code}] generated");

                if (cartonSizes.Count > 1)
                {
                    if (_lastFunc != null)
                        await _lastFunc();
                    else
                        await Init();
                }
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, PrintTote);
            }
        }
    }
}