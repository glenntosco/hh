using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Business.Floor.Bulk;
using Pro4Soft.MobileDevice.Business.Floor.Inventory;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.Fulfillment.Picking
{
    [ViewController("main.tagPickTicketListHH")]
    public class TagPickTicketList : ScanScreenController
    {
        public override string Title => "Pick ticket Tags";

        protected override async Task Init()
        {
            try
            {
                if (Singleton<Context>.Instance.DefaultWarehouseId == null)
                    throw new ExceptionLocalized("Warehouse is not setup for user");

                var tagToUse = (await Singleton<Web>.Instance.GetInvokeAsync<ConfigEntry>($"data/config/{nameof(ConfigConstants.Business_Fulfillment_Handheld_TagList)}")).StringValue;

                var allowedState = new List<PickTicketState>
                {
                    PickTicketState.PendingLetdown,
                    PickTicketState.PendingPacksizeBreakdown,
                };

                var orders = await Singleton<Web>.Instance.GetInvokeAsync<List<PickTicketHelper>>(@$"odata/PickTicket?$select=Id,PickTicketNumber,PickTicketState,{tagToUse}
&$orderby=PickTicketNumber desc
&$expand=Customer($select=Id,CustomerCode,CompanyName)
&$filter=WarehouseId eq {Singleton<Context>.Instance.DefaultWarehouseId} and ({string.Join(" or ", allowedState.Select(c => $"PickTicketState eq '{c}'"))})");

                var groups = orders.Where(c => c.Tags.Any(c1 => !string.IsNullOrWhiteSpace(c1))).GroupBy(c => $"{c.PickTicketState}.{c.Tag1?.Trim()}.{c.Tag2?.Trim()}.{c.Tag3?.Trim()}.{c.Tag4?.Trim()}.{c.Tag5?.Trim()}");

                foreach (var group in groups.OrderBy(c=>c.Key))
                {
                    var order = group.First();

                    var title = order.Tags.First(c => !string.IsNullOrWhiteSpace(c)).Trim();

                    View.PushMessageWithSubtitle(title, Lang.Translate($"[{group.Count()}] orders"), Lang.Translate(Utils.SpaceCamel(order.PickTicketState.ToString())), async () =>
                    {
                        Type controllerType;
                        switch (order.PickTicketState)
                        {
                            case PickTicketState.PendingPacksizeBreakdown:
                                controllerType = typeof(PacksizeBreakdown);
                                break;
                            default:
                                controllerType = typeof(LetdownProductByBin);
                                break;
                        }

                        await Main.NavigateToController(controllerType, c =>
                        {
                            c.AssignedTask = new UserTask
                            {
                                ReferenceNumber = title,
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
}