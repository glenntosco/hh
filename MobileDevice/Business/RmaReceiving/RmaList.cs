using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Returns;
using Pro4Soft.MobileDevice.Business.Fulfillment.Picking;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.RmaReceiving
{
    [ViewController("main.rmaListHH")]
    public class RmaList : ScanScreenController
    {
        public override string Title => "Customer returns";

        protected override async Task Init()
        {
            try
            {
                if (Singleton<Context>.Instance.DefaultWarehouseId == null)
                    throw new ExceptionLocalized("Warehouse is not setup for user");

                var allowedStates = new[]
                {
                    CustomerReturnState.NotReceived,
                    CustomerReturnState.PartiallyReceived
                };

                var orders = await Singleton<Web>.Instance.GetInvokeAsync<List<CustomerReturnHelper>>(@$"odata/CustomerReturn?
$select=Id,CustomerReturnNumber,CustomerReturnState,ReleaseDate
&$orderby=CustomerReturnNumber desc
&$expand=Customer($select=Id,CustomerCode,CompanyName)
&$filter=WarehouseId eq {Singleton<Context>.Instance.DefaultWarehouseId} and ({string.Join(" or ", allowedStates.Select(c=>$"CustomerReturnState eq '{c}'"))})
&$top=100");

                foreach (var order in orders)
                {
                    View.PushMessageWithSubtitle(order.CustomerReturnNumber, order.Customer.CompanyName, Lang.Translate(Utils.SpaceCamel(order.CustomerReturnState.ToString())), async () =>
                    {
                        await Main.NavigateToController<RmaReceiving>(c =>
                        {
                            c.AssignedTask = new UserTask
                            {
                                ReferenceId = order.Id,
                                ReferenceNumber = order.CustomerReturnNumber,
                                TaskTypeEnum = UserTaskType.CustomerReturn
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

    public class CustomerReturnHelper
    {
        public Guid Id { get; set; }
        public string CustomerReturnNumber { get; set; }
        public CustomerReturnState CustomerReturnState { get; set; }
        public Guid? ClientId { get; set; }

        public CustomerHelper Customer { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}