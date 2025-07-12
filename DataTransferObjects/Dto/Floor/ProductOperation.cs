using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class ProductOperation: InventoryDetail
    {
        public Guid ProductId { get; set; }
        public string Expiry { get; set; }

        public string Reason { get; set; }
        public string ReferenceCode { get; set; }
        public Guid? ReferenceId { get; set; }
        public string ReferenceType { get; set; }
    }

    public class InventoryLocationProductOp : ProductOperation
    {
        public Guid? BinId { get; set; }
        public string LicensePlate { get; set; }
    }

    public class ReasonOperation
    {
        public string Reason { get; set; }
    }

    public enum ProductionConsumptionMode
    {
        ProductionProportionate,
        OrderedQuantity
    }
}