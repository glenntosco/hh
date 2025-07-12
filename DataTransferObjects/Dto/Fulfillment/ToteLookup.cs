using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Floor;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class PickTicketLookupLine
    {
        public Guid Id { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal? PickedQuantity { get; set; }
    }

    public class ToteLookup
    {
        public Guid Id { get; set; }
        public Guid PickTicketId { get; set; }
        public string Sscc18Code { get; set; }
        public string BigText { get; set; }
        public string PickTicketNumber { get; set; }
        public string TrackTraceNumber { get; set; }
        public PickTicketState? PickTicketState { get; set; }

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public UnitOfMeasure LengthUnitOfMeasure { get; set; }

        public decimal? Weight { get; set; }
        public UnitOfMeasure WeightUnitOfMeasure { get; set; }

        public Guid? BindId { get; set; }
        public string BinCode { get; set; }

        public Guid? LicensePlateId { get; set; }
        public string LicensePlate { get; set; }

        public Guid? DockDoorId { get; set; }
        public string DockDoorName { get; set; }
        public string DockDoorBarcode { get; set; }

        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }

        public ToteType ToteType { get; set; }

        public Guid? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        
        public List<ToteLookupLine> Lines { get; set; } = new List<ToteLookupLine>();
    }

    public enum ToteType
    {
        Carton,
        Pallet,
        Other
    }

    public class ToteLookupLine
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int LineNumber { get; set; }
        public decimal QuantityToPick { get; set; }
        public decimal PickedQuantity { get; set; }
        public decimal ShippedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }

        public List<ToteContentLineDetail> Details { get; set; } = new List<ToteContentLineDetail>();
    }

    public class ToteContentLineDetail: InventoryDetail
    {

    }

    public class IdName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}