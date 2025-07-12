using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.MobileDevice.Business.Floor.Bulk;
using Pro4Soft.MobileDevice.Business.Floor.Inventory;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Production
{
    [ViewController("main.productionOrderListHH")]
    public class ProductionOrderList : ScanScreenController
    {
        public override string Title => "Production orders";

        protected override async Task Init()
        {
            try
            {
                if (Singleton<Context>.Instance.DefaultWarehouseId == null)
                    throw new ExceptionLocalized("Warehouse is not setup for user");

                var allowedState = new List<ProductionOrderState>
                {
                    ProductionOrderState.ReadyToPick,
                    ProductionOrderState.PendingLetdown,
                    ProductionOrderState.PendingPacksizeBreakdown,
                    ProductionOrderState.BeingPicked,
                    ProductionOrderState.ReadyForProduction,
                    ProductionOrderState.InProduction,
                };

                var orders = await Singleton<Web>.Instance.GetInvokeAsync<List<SubstOrderHelper>>(@$"odata/ProductionOrder?
$select=Id,ProductionOrderNumber,ProductionOrderState
&$orderby=ProductionOrderNumber desc
&$filter=WarehouseId eq {Singleton<Context>.Instance.DefaultWarehouseId} and ({string.Join(" or ", allowedState.Select(c => $"ProductionOrderState eq '{c}'"))})
&$top=100");

                foreach (var order in orders)
                {
                    View.PushMessageWithSubtitle(order.ProductionOrderNumber, null, Lang.Translate(Utils.SpaceCamel(order.ProductionOrderState.ToString())), async () =>
                    {
                        Type controllerType;
                        switch (order.ProductionOrderState)
                        {
                            case ProductionOrderState.PendingLetdown:
                                controllerType = typeof(LetdownLpnByBin);
                                break;
                            case ProductionOrderState.PendingPacksizeBreakdown:
                                controllerType = typeof(PacksizeBreakdown);
                                break;
                            case ProductionOrderState.ReadyForProduction:
                            case ProductionOrderState.InProduction:
                                controllerType = typeof(ProductionOrderProduce);
                                break;
                            default:
                                controllerType = typeof(ProductionOrderPick);
                                break;
                        }

                        await Main.NavigateToController(controllerType, c =>
                        {
                            c.AssignedTask = new UserTask
                            {
                                ReferenceId = order.Id,
                                ReferenceNumber = order.ProductionOrderNumber,
                                TaskTypeEnum = UserTaskType.ProductionOrder
                            };
                        });
                    }, false);
                }

                View.PromptInfo("Select an item");
            }
            catch (Exception e)
            {
                await View.PushError(e.Message, Init);
            }
        }
    }

    public class SubstOrderHelper
    {
        public Guid Id { get; set; }
        public string ProductionOrderNumber { get; set; }
        public ProductionOrderState ProductionOrderState { get; set; }
        public Guid? ClientId { get; set; }
    }
}