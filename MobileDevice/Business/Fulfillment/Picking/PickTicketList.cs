using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Business.Floor.Bulk;
using Pro4Soft.MobileDevice.Business.Floor.Inventory;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Picking
{
    [ViewController("main.pickTicketListHH")]
    public class PickTicketList : ScanScreenController
    {
        public override string Title => "Pick tickets";

        private bool _autoWavePickTicket;

        protected override async Task Init()
        {
            try
            {
                if (Singleton<Context>.Instance.DefaultWarehouseId == null)
                    throw new ExceptionLocalized("Warehouse is not setup for user");

                var configs = (await Singleton<Web>.Instance.PostInvokeAsync<List<ConfigEntry>>("data/configs", new List<string>
                {
                    nameof(ConfigConstants.Business_Fulfillment_Handheld_AutoWavePickTicketOnScan)
                })).ToDictionary(c => c.Name, c => c.BoolValue);
                _autoWavePickTicket = configs[nameof(ConfigConstants.Business_Fulfillment_Handheld_AutoWavePickTicketOnScan)];

                var allowedState = new List<PickTicketState>
                {
                    PickTicketState.PendingLetdown,
                    PickTicketState.PendingPacksizeBreakdown,
                    PickTicketState.Waved,
                    PickTicketState.BeingPicked,
                };
                if(_autoWavePickTicket)
                    allowedState.Add(PickTicketState.ReadyToPick);

                var orders = await Singleton<Web>.Instance.GetInvokeAsync<List<PickTicketHelper>>(@$"odata/PickTicket?$select=Id,PickTicketNumber,PickTicketState
&$orderby=PickTicketNumber desc
&$expand=Customer($select=Id,CustomerCode,CompanyName)
&$filter=WarehouseId eq {Singleton<Context>.Instance.DefaultWarehouseId} and ({string.Join(" or ", allowedState.Select(c => $"PickTicketState eq '{c}'"))})
&$top=100");

                foreach (var order in orders)
                {
                    View.PushMessageWithSubtitle(order.PickTicketNumber, order.Customer.CompanyName, Lang.Translate(Utils.SpaceCamel(order.PickTicketState.ToString())), async () =>
                    {
                        Type controllerType;
                        switch (order.PickTicketState)
                        {
                            case PickTicketState.PendingLetdown:
                                controllerType = typeof(LetdownLpnByBin);
                                break;
                            case PickTicketState.PendingPacksizeBreakdown:
                                controllerType = typeof(PacksizeBreakdown);
                                break;
                            default:
                                controllerType = typeof(Picking);
                                break;
                        }

                        await Main.NavigateToController(controllerType, c =>
                        {
                            c.AssignedTask = new UserTask
                            {
                                ReferenceId = order.Id,
                                ReferenceNumber = order.PickTicketNumber,
                                TaskTypeEnum = UserTaskType.PickTicket
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

    public class PickTicketHelper
    {
        public Guid Id { get; set; }
        public string PickTicketNumber { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public PickTicketState PickTicketState { get; set; }
        public Guid? ClientId { get; set; }
        public CustomerHelper Customer { get; set; }

        public string[] Tags => new[] {Tag1, Tag2, Tag3, Tag4, Tag5};
    }

    public class CustomerHelper
    {
        public Guid Id { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyName { get; set; }
    }
}