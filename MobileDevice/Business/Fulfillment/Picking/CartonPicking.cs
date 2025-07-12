using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Picking
{
    [ViewController("main.cartonPicking")]
    public class CartonPicking : WavePicking
    {
        public override string Title => "Carton picking";

        private Button _startToolbar;

        protected override async Task Init()
        {
            _lastFunc = Init;
            _pickTickets =  new List<PickTicketLookup>();
            _startToolbar = View.RemoveToolbar(_startToolbar);
            _short = View.RemoveToolbar(_short);
            _skip = View.RemoveToolbar(_skip);
            _printTote = View.RemoveToolbar(_printTote);
            //View.ClearMessages();
            await AskCartonizedTotes();
        }

        //Pre start
        protected async Task AskCartonizedTotes()
        {
            await LoopUntilGood(async () =>
            {
                var pickTicket = await CartonizationResultLookup();
                if (!pickTicket.RemainingPicks.Any())
                    throw new ExceptionLocalized($"No picks for that tote");

                var tote = pickTicket.RemainingPicks.First();

                if (!_pickTickets.Any())
                    _pickTickets.Add(pickTicket);
                else
                {
                    if(_pickTickets.First().WaveNumber != pickTicket.WaveNumber)
                        throw new ExceptionLocalized($"Cannot pick multile waves");
                    var existingPickTicket = _pickTickets.SingleOrDefault(c => c.Id == pickTicket.Id);
                    if(existingPickTicket == null)
                        _pickTickets.Add(pickTicket);
                    else
                    {
                        if(existingPickTicket.RemainingPicks.Any(c => c.Sscc18Code == tote.Sscc18Code))
                            throw new ExceptionLocalized($"Tote [{tote.Sscc18Code}] is already in the batch");
                        existingPickTicket.RemainingPicks.AddRange(pickTicket.RemainingPicks);
                    }
                }

                var remainingPicks = _pickTickets.SelectMany(c => c.RemainingPicks).ToList();
                await View.PushMessage($@"{Lang.Translate($"Tote: [{tote.Sscc18Code}] - [{tote.BigText}]")}
{Lang.Translate($"Pick ticket: [{pickTicket.PickTicketNumber}]")}
{Lang.Translate($"Picks [{remainingPicks.Count}]")}
{Lang.Translate($"Units [{remainingPicks.Sum(c => c.QuantityToPick)}]")}", null, false);
            }, AskCartonizedTotes);
            
            if(_pickTickets.Count > 0)
                _startToolbar ??= View.AddToolbar("Start", PickingCycle);
            
            await AskCartonizedTotes();
        }
    }
}