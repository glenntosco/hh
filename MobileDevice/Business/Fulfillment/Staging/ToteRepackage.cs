using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Staging
{
    [ViewController("main.toteRepackage")]
    public class ToteRepackage : ProductScanController
    {
        public override string Title => "Tote repackage";

        private ToteLookup _fromTote;
        private ToteLookup _toTote;
        private Func<Task> _lastFunc;

        private Button _changeTote;
        private Button _printTote;

        protected override async Task Init()
        {
            await AskFromTote();
        }

        protected async Task AskFromTote()
        {
            _changeTote = View.RemoveToolbar(_changeTote);
            _printTote = View.RemoveToolbar(_printTote);
            await LoopUntilGood(async () =>
            {
                var toteCode = await View.PromptScan("Scan from tote...");
                var toteLookup = await Singleton<Web>.Instance.GetInvokeAsync<ToteLookup>($"hh/lookup/ToteLookup?toteCode={toteCode}");
                var allowedStates = new[] {PickTicketState.BeingPicked, PickTicketState.Rating, PickTicketState.PendingShipCount, PickTicketState.PendingDriverSignature};
                if (!allowedStates.Contains(toteLookup.PickTicketState ?? PickTicketState.Closed))
                    throw new ExceptionLocalized($"Cannot repackage. Invalid status [{toteLookup.PickTicketState}]");
                if(!toteLookup.Lines.Any())
                    throw new ExceptionLocalized($"Cannot repackage. Tote has no product");

                await View.PushMessage($@"{Lang.Translate($"Tote [{toteLookup.Sscc18Code}]")}
{Lang.Translate($"Pick ticket [{toteLookup.PickTicketNumber}] - [{toteLookup.PickTicketState}]")}
{Lang.Translate($"Lines [{toteLookup.Lines.Count}]")}
{Lang.Translate($"Quantity [{toteLookup.Lines.Sum(c => c.PickedQuantity)}]")}", null, false);

                _fromTote = toteLookup;
            }, AskProductOp);

            _changeTote ??= View.AddToolbar("Done", Init);
            _printTote ??= View.AddToolbar("Print tote", PrintTote);
            await AskProductOp();
        }

        protected override Func<Task> ProductReady => AskToTote;
        protected override Func<Task> SerialReady => AskToTote;
        protected override Func<Task> NoMoreSerials => AskProductOp;

        protected override async Task AskProductOp()
        {
            _lastFunc = AskProductOp;
            await LoopUntilGood(async () =>
            {
                await base.AskProductOp();
                var line = _fromTote.Lines.SingleOrDefault(c => c.ProductId == ProdDetails.Id);
                if (line == null)
                    throw new ExceptionLocalized($"Sku [{ProdDetails.Sku}] is not in tote");
                if (ProdOperation.Quantity > line.PickedQuantity)
                    throw new ExceptionLocalized($"Cannot move more than Picked quantity");
            }, AskProductOp);

            await AskToTote();
        }

        protected async Task AskToTote()
        {
            _lastFunc = AskToTote;
            await LoopUntilGood(async () =>
            {
                var toteCode = await View.PromptScan("Scan to tote...");
                var toteLookup = await Singleton<Web>.Instance.GetInvokeAsync<ToteLookup>($"hh/lookup/ToteLookup?toteCode={toteCode}");
                if(toteLookup.PickTicketNumber != _fromTote.PickTicketNumber)
                    throw new BusinessWebException($"Cannot repackage totes from different PickTickets");

                await View.PushMessage($@"{Lang.Translate($"Tote [{toteLookup.Sscc18Code}]")}
{Lang.Translate($"Lines [{toteLookup.Lines.Count}]")}
{Lang.Translate($"Quantity [{toteLookup.Lines.Sum(c => c.PickedQuantity)}]")}", null, false);

                _toTote = toteLookup;
            }, AskProductOp);

            await Process();
        }

        protected async Task Process()
        {
            try
            {
                var lines = _fromTote.Lines.Where(c=>c.ProductId == ProdDetails.Id).ToList();
                var line = lines.First();
                if (lines.Count > 1)
                {
                    var lineNum = await View.PromptPicker("Order line", lines.Select(c => c.LineNumber.ToString()).OrderBy(c => c).ToList());
                    line = lines.Single(c => c.LineNumber.ToString() == lineNum);
                }

                _fromTote = await Singleton<Web>.Instance.PostInvokeAsync<ToteLookup>($"hh/fulfillment/ToteRepackage?fromToteLineId={line.Id}&toToteId={_toTote.Id}", ProdOperation);
                View.InactivateMessages();
                await View.PushMessage($"Moved!");

                if (!_fromTote.Lines.Any())
                    await Init();
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