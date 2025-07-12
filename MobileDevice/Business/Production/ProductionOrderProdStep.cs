using System;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Production
{
    [ViewController("main.productionOrderProdStep")]
    public class ProductionOrderProdStep : ScanScreenController
    {
        private ProductionOrder _productionOrder;
        private Node _currentProdState;
        private Edge _transition;
        private string _details;
        public override string Title => "Production processing";

        protected override async Task Init()
        {
            await AskWo();
        }

        public async Task AskWo()
        {
            _transition = null;
            _productionOrder = null;
            _currentProdState = null;
            _details = null;

            await LoopUntilGood(async () =>
            {
                _productionOrder = await ProductionOrderLookup();
                var allowedStates = new[] {ProductionOrderState.InProduction, ProductionOrderState.ReadyForProduction};
                if (!allowedStates.Contains(_productionOrder.ProductionOrderState))
                    throw new ExceptionLocalized($"Invalid state [{_productionOrder.ProductionOrderState}]");
                if (_productionOrder.CanComplete)
                    throw new ExceptionLocalized($"Production order is in final production state. Use produce instead");
                _currentProdState = await Singleton<Web>.Instance.GetInvokeAsync<Node>($"hh/lookup/ProductionOrderProdStepLookup?key={_productionOrder.Id}");
            }, AskWo);

            await View.PushMessage($@"{_productionOrder.ProductionOrderNumber}
{Lang.Translate($"Step [{_productionOrder.ProductionStep}]")}", null, false);

            foreach (var transition in _currentProdState.Outbound)
            {
                View.PushMessageWithSubtitle(transition.ProdStep, null, transition.Description, async () =>
                {
                    _transition = transition;
                    if (transition.IsCaptureDetails)
                        switch (transition.DetailsType)
                        {
                            case DetailsType.Scan:
                                _details = await View.PromptScan($"Scan [{transition.DetailsLabel??"Details"}]...");
                                break;
                            default:
                                _details = await View.PromptString($"Enter [{transition.DetailsLabel ?? "Details"}]...");
                                break;
                        }

                    await Process();
                });
            }
            await View.ScrollToBottom();
        }

        private async Task Process()
        {
            try
            {
                var url = $"hh/production/ProdStepProductionOrder?productionOrderId={_productionOrder.Id}&newStep={_transition.ProdStep}";
                if (_transition.IsCaptureDetails)
                    url += $"&detail={_details}";
                await Singleton<Web>.Instance.GetInvokeAsync(url);

                await View.PushMessage($"[{_productionOrder.ProductionOrderNumber}]: [{_productionOrder.ProductionStep}] -> [{_transition.ProdStep}]");
            }
            catch (Exception ex)
            {
                await View.PushError(ex.Message, Process);
            }
            finally
            {
                View.InactivateMessages();
                await AskWo();
            }
        }
    }
}