using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;

namespace Pro4Soft.DataTransferObjects.Dto.Production
{
    public class ProductionOrder
    {
        public Guid Id { get; set; }
        public string ProductionOrderNumber { get; set; }
        public ProductionOrderState ProductionOrderState { get; set; }

        public string ProductionStep { get; set; }

        public bool CanComplete { get; set; }

        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }

        public Guid? WorkAreaBinId { get; set; }
        public string WorkAreaBinCode { get; set; }

        public List<PickReservation> RemainingPicks { get; set; } = new List<PickReservation>();
        public List<ProducibleLine> InLines { get; set; } = new List<ProducibleLine>();
    }

    public class ProducibleLine
    {
        public decimal Quantity { get; set; }
        public decimal? ProducedQuantity { get; set; }
        public ProductDetails Product { get; set; }
    }

    public enum ProductionOrderState
    {
        Draft,
        Unallocated,
        HeldShort,
        PendingLetdown,
        PendingPacksizeBreakdown,
        ReadyToPick,
        BeingPicked,
        ReadyForProduction,
        InProduction,
        Completed,
        Closed,
        Suspended,
    }
}