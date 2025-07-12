using System;
using Pro4Soft.DataTransferObjects.Dto.Floor;

namespace Pro4Soft.DataTransferObjects.Dto.Collaboration
{
    public class UserTask
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public string UserTaskNumber { get; set; }
        public string ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }

        public string ReferenceNumber { get; set; }
        public int Priority { get; set; } = 0;

        public UserTaskType TaskTypeEnum { get; set; }

        public string Instructions { get; set; }
        
        public UserTaskDetails Details { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public override string ToString()
        {
            switch (TaskTypeEnum)
            {
                case UserTaskType.PickTicket:
                    return $"Process pick ticket [{ReferenceNumber}]";
                case UserTaskType.PurchaseOrder:
                    return $"Receive purchase order [{ReferenceNumber}]";
                case UserTaskType.CustomerReturn:
                    return $"Receive return [{ReferenceNumber}]";
                case UserTaskType.TruckLoad:
                    return $"Process truck load [{ReferenceNumber}]";
                case UserTaskType.MasterTruckLoad:
                    return $"Process master truck load [{ReferenceNumber}]";
                case UserTaskType.ProductionOrder:
                    return $"Process substitute order [{ReferenceNumber}]";
                case UserTaskType.ProductMove:
                    return $"Product move [{ReferenceNumber}]";
                case UserTaskType.BinMove:
                    return $"Bin move [{ReferenceNumber}]";
                case UserTaskType.LpnMove:
                    return $"LPN move [{ReferenceNumber}]";
                case UserTaskType.CycleCount:
                    return $"Cycle count [{ReferenceNumber}]";
            }
            return null;
        }
    }

    public class UserTaskDetails
    {
        public string FromType { get; set; }
        
        public string From { get; set; }
        public Guid FromId { get; set; }

        public string To { get; set; }
        public Guid? ToId { get; set; }

        public string Sku { get; set; }
        public string Barcode { get; set; }

        public int? Packsize { get; set; }
        public decimal? Quantity { get; set; }
    }
}
