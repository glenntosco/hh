using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Receiving;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Business.PoReceiving
{
    [ViewController("main.purchaseOrderListHH")]
    public class PoList : ScanScreenController
    {
        public override string Title => "Purchase orders";

        protected override async Task Init()
        {
            try
            {
                if (Singleton<Context>.Instance.DefaultWarehouseId == null)
                    throw new ExceptionLocalized("Warehouse is not setup for user");

                var allowedStates = new[]
                {
                    PurchaseOrderState.NotReceived,
                    PurchaseOrderState.PartiallyReceived
                };

                var orders = await Singleton<Web>.Instance.GetInvokeAsync<List<PurchaseOrderHelper>>(@$"odata/PurchaseOrder?
$select=Id,PurchaseOrderNumber,PurchaseOrderState,ReleaseDate
&$orderby=PurchaseOrderNumber desc
&$expand=Vendor($select=Id,VendorCode,CompanyName)
&$filter=WarehouseId eq {Singleton<Context>.Instance.DefaultWarehouseId} and ({string.Join(" or ", allowedStates.Select(c => $"PurchaseOrderState eq '{c}'"))})
&$top=100");

                foreach (var order in orders)
                {
                    View.PushMessageWithSubtitle(order.PurchaseOrderNumber, order.Vendor?.CompanyName, Lang.Translate(Utils.SpaceCamel(order.PurchaseOrderState.ToString())), async () =>
                    {
                        await Main.NavigateToController<PoReceiving>(c =>
                        {
                            c.AssignedTask = new UserTask
                            {
                                ReferenceId = order.Id,
                                ReferenceNumber = order.PurchaseOrderNumber,
                                TaskTypeEnum = UserTaskType.PurchaseOrder
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

    public class PurchaseOrderHelper
    {
        public Guid Id { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public PurchaseOrderState PurchaseOrderState { get; set; }
        public Guid? ClientId { get; set; }

        public VendorHelper Vendor { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class VendorHelper
    {
        public Guid Id { get; set; }
        public string VendorCode { get; set; }
        public string CompanyName { get; set; }
    }
}