using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.toteCount")]
    public class ToteCount : ProductScanController
    {
        public override string Title => "Tote qty count";

        private ToteLookup _tote;
        
        private Button _changeTote;

        protected override async Task Init()
        {
            _changeTote = View.RemoveToolbar(_changeTote);

            await LoopUntilGood(async () =>
            {
                _tote = await ToteLookup(Init);
                if (AssignedTask != null && AssignedTask.ReferenceId != _tote.PickTicketId)
                    throw new ExceptionLocalized($"Invalid pick ticket [{_tote.PickTicketNumber}], expected [{AssignedTask.ReferenceNumber}]");
            });

            _changeTote = View.AddToolbar("Next tote", Init);
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => Process;
        protected override Func<Task> SerialReady => Process;
        protected override Func<Task> NoMoreSerials => AskProductOp;

        protected async Task Process()
        {
            try
            {
                var lines = _tote.Lines.Where(c => c.ProductId == ProdDetails.Id).ToList();
                if (!lines.Any())
                    throw new ExceptionLocalized($"Product [{ProdDetails.Sku}] is not in tote");
                if (lines.Sum(c => c.PickedQuantity) < ProdOperation.Quantity * (ProdDetails.EachCount ?? 1))
                    throw new ExceptionLocalized($"Product [{ProdDetails.Sku}] counted more than picked");

                await Singleton<Web>.Instance.PostInvokeAsync($"hh/fulfillment/ToteCount?toteId={_tote.Id}", ProdOperation);
                View.InactivateMessages();
                await View.PushMessage($"Counted [{ProdOperation.Quantity}] units!");

                if (lines.Sum(c => c.PickedQuantity) == ProdOperation.Quantity * (ProdDetails.EachCount ?? 1))
                    await AskProductOp();
                else if (ProdDetails.IsSerialControlled)
                    await AskSerial();
                else
                    await AskProductOp();
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
                await AskProductOp();
            }
        }
    }
}