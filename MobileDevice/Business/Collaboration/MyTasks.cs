using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Production;
using Pro4Soft.MobileDevice.Business.Floor;
using Pro4Soft.MobileDevice.Business.Floor.Bulk;
using Pro4Soft.MobileDevice.Business.Floor.CycleCount;
using Pro4Soft.MobileDevice.Business.Floor.Inventory;
using Pro4Soft.MobileDevice.Business.Fulfillment.Picking;
using Pro4Soft.MobileDevice.Business.Fulfillment.Staging;
using Pro4Soft.MobileDevice.Business.Production;
using Pro4Soft.MobileDevice.Business.RmaReceiving;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Collaboration
{
    [ViewController("main.myTasks")]
    public class MyTasks : ScanScreenController
    {
        public override string Title => "My tasks";

        protected override async Task Init()
        {
            Singleton<Context>.Instance.Tasks.CollectionChanged += TasksCollectionChanged;
            LoadButtons();
        }

        private void TasksCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            View.Dispatcher.BeginInvokeOnMainThread(LoadButtons);
        }

        public void LoadButtons()
        {
            View.ClearMessages();
            foreach (var task in Singleton<Context>.Instance.Tasks)
            {
                View.PushMessageWithSubtitle(task.ToString(), task.Instructions, task.UserTaskNumber, async () =>
                {
                    try
                    {
                        var taskToExec = task;
                        Type controllerType = null;
                        switch (taskToExec.TaskTypeEnum)
                        {
                            case UserTaskType.PurchaseOrder:
                                controllerType = typeof(PoReceiving.PoReceiving);
                                break;
                            case UserTaskType.PickTicket:
                                {
                                    var pickTicketState = (await Singleton<Web>.Instance.GetInvokeAsync<List<dynamic>>($@"odata/PickTicket?$filter=Id eq {taskToExec.ReferenceId}&$select=Id,PickTicketState,FreightType"))
                                        .Select(c => new
                                        {
                                            PickTicketState = ((string)c.PickTicketState.ToString()).ParseEnum<PickTicketState>(),
                                            FreightType = ((string)c.FreightType.ToString()).ParseEnum<FreightType>(),
                                        }).SingleOrDefault();
                                    if (pickTicketState == null)
                                        throw new ExceptionLocalized($"Pickticket [{taskToExec.ReferenceNumber}] cannot be found");
                                    switch (pickTicketState.PickTicketState)
                                    {
                                        case PickTicketState.PendingLetdown:
                                            controllerType = typeof(LetdownLpnByBin);
                                            break;
                                        case PickTicketState.PendingPacksizeBreakdown:
                                            controllerType = typeof(PacksizeBreakdown);
                                            break;
                                        case PickTicketState.PendingShipCount:
                                        case PickTicketState.PendingDeliveryCount:
                                            controllerType = typeof(ToteCount);
                                            break;
                                        case PickTicketState.Rating:
                                        case PickTicketState.PendingDriverSignature:
                                            if(pickTicketState.FreightType != FreightType.PrivateFleet)
                                                throw new ExceptionLocalized($"Pickiet ticket [{task.ReferenceNumber}] invalid freight type [{pickTicketState.FreightType}]");
                                            controllerType = typeof(Fulfillment.ShipPickTickets.Ship);
                                            if ((await Singleton<Web>.Instance.GetInvokeAsync<dynamic>($@"api/PickTicketApi/HasStagedTotes?key={taskToExec.ReferenceId}")).NeedsToteStaging == true)
                                                controllerType = typeof(Fulfillment.ShipPickTickets.DirectedToteMoveForPickTicket);
                                            break;
                                        case PickTicketState.PendingDeliverySignature:
                                            controllerType = typeof(Fulfillment.ShipPickTickets.Deliver);
                                            break;
                                        case PickTicketState.Waved:
                                            controllerType = typeof(Picking);
                                            break;
                                        default:
                                            var ds = new List<string> {Lang.Translate("Regular picking"), Lang.Translate("Carton picking")};
                                            var result = await View.PromptPicker("Picking style", ds);
                                            controllerType = ds[0] == result ? typeof(Picking) : typeof(FullPackPicking);
                                            break;
                                    }
                                }
                                break;
                            case UserTaskType.CustomerReturn:
                                controllerType = typeof(RmaReceiving.RmaReceiving);
                                break;
                            case UserTaskType.TruckLoad:
                                controllerType = typeof(Fulfillment.ShipTruckLoads.Ship);
                                if ((await Singleton<Web>.Instance.GetInvokeAsync<dynamic>($@"api/TruckLoadApi/HasStagedTotes?key={taskToExec.ReferenceId}")).NeedsToteStaging == true)
                                    controllerType = typeof(Fulfillment.ShipTruckLoads.DirectedToteMoveForTruckLoad);
                                break;
                            case UserTaskType.MasterTruckLoad:
                                controllerType = typeof(Fulfillment.ShipTruckLoads.Ship);
                                if ((await Singleton<Web>.Instance.GetInvokeAsync<dynamic>($@"api/MasterTruckLoadApi/HasStagedTotes?key={taskToExec.ReferenceId}")).NeedsToteStaging == true)
                                    controllerType = typeof(Fulfillment.ShipTruckLoads.DirectedToteMoveForTruckLoad);
                                break;
                            case UserTaskType.ProductionOrder:
                            {
                                var subState = (await Singleton<Web>.Instance.GetInvokeAsync<List<dynamic>>($@"odata/ProductionOrder?$filter=Id eq {taskToExec.ReferenceId}&$select=Id,ProductionOrderState"))
                                    .Select(c => (string)c.ProductionOrderState.ToString()).Select(c => c.ParseEnum<ProductionOrderState>()).SingleOrDefault();
                                switch (subState)
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
                            }
                                break;
                            case UserTaskType.ProductMove:
                                controllerType = typeof(ProductMove);
                                break;
                            case UserTaskType.BinMove:
                                controllerType = typeof(FullBinMove);
                                break;
                            case UserTaskType.LpnMove:
                                controllerType = typeof(LicensePlateMove);
                                break;
                            case UserTaskType.CycleCount:
                                controllerType = typeof(CycleCountByBin);
                                break;
                        }

                        if (controllerType == null)
                            return;

                        await Main.NavigateToController(controllerType, c =>
                        {
                            c.AssignedTask = taskToExec;
                        });
                    }
                    catch (Exception e)
                    {
                        View.PromptError(e);
                    }
                }, false);
            }
            View.PromptInfo("Click an item");
        }

        public override void OnClosing()
        {
            Singleton<Context>.Instance.Tasks.CollectionChanged -= TasksCollectionChanged;
        }
    }
}