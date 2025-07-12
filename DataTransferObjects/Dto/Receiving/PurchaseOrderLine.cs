using System;

namespace Pro4Soft.DataTransferObjects.Dto.Receiving
{
    public class PurchaseOrderLine
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }

        public int LineNumber { get; set; }

        public Guid ProductId { get; set; }
        
        public string PurchaseOrderNumber { get; set; }

        public int? Packsize { get; set; }
        public int? NumberOfPacks { get; set; }
        public string Instructions { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal OutstandingQuantity => OrderedQuantity - ReceivedQuantity;
    }
}