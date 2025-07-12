using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class ProductAuditRecord
    {
        public Guid Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string Type { get; set; }
        public string SubType { get; set; }

        public string UserName { get; set; }
        public Guid? UserId { get; set; }

        public string Details { get; set; }

        public bool? IsWhTransfer { get; set; }

        public Guid? PickTicketId { get; set; }
        public string PickTicketNumber { get; set; }
        public int? PickTicketLineNumber { get; set; }

        public Guid? ToteMasterId { get; set; }
        public string ToteMaster { get; set; }

        public Guid? PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int? PurchaseOrderLineNumber { get; set; }

        public Guid? ProductionOrderId { get; set; }
        public string ProductionOrderNumber { get; set; }

        public decimal Quantity { get; set; }
        public decimal RunningTotal { get; set; }
    }

    public class WhProductAuditRecord
    {
        public string Sku { get; set; }
        public string WarehouseCode { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal RunningTotal { get; set; }
        public List<ProductAuditRecord> Records { get; set; } = new List<ProductAuditRecord>();
    }
}