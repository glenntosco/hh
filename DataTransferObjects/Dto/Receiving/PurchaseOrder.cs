using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Receiving
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public PurchaseOrderState PurchaseOrderState { get; set; }

        public Guid? ClientId { get; set; }

        public bool IsWarehouseTransfer { get; set; }

        public DateTime? UploadDate { get; set; }
        public bool? UploadedSuceeded { get; set; }
        
        public Guid? VendorId { get; set; }
        public string VendorCompanyName { get; set; }
        public string VendorCode { get; set; }

        public Guid WarehouseId { get; set; }

        public List<PurchaseOrderLine> Lines { get; set; } = new List<PurchaseOrderLine>();
    }

    public enum PurchaseOrderState
    {
        Draft,
        NotReceived,
        PartiallyReceived,
        Received,
        Closed
    }
}