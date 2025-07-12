using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class PickTicketLookup
    {
        public Guid Id { get; set; }
        public string PickTicketNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string PickInstructions { get; set; }
        public string WaveNumber { get; set; }
        public PickTicketState PickTicketState { get; set; }
        public WarehouseLookup Warehouse { get; set; }

        public Guid? DockDoorId { get; set; }
        public string DockDoorBarcode { get; set; }
        public string DockDoor { get; set; }

        public bool IsWarehouseTransfer { get; set; }

        public bool EnforceCartonizationPicking { get; set; }

        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }

        public Guid? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public DateTime? UploadDate { get; set; }
        public DateTime? UploadedSuceeded { get; set; }
        public string SignUrl { get; set; }

        public DateTimeOffset? DateCreated { get; set; }
        

        public List<PickReservation> RemainingPicks { get; set; } = new List<PickReservation>();
        public List<ToteLookup> Totes { get; set; } = new List<ToteLookup>();
        public List<PickTicketLookupLine> Lines { get; set; } = new List<PickTicketLookupLine>();
    }

    public class PickTicketQtyLookup
    {
        public Guid Id { get; set; }
        public string PickTicketNumber { get; set; }
        public decimal Ordered { get; set; }
        public decimal Allocated { get; set; }
        public decimal Picked { get; set; }
    }

    public class WarehouseLookup
    {
        public Guid Id { get; set; }
        public string WarehouseCode { get; set; }
    }

    public enum FreightType
    {
        TruckLoad,
        SmallParcel,
        PrivateFleet,
        External
    }
    
    public enum PickTicketState
    {
        Draft,
        Unallocated,
        HeldShort,
        PendingLetdown,
        PendingProduction,
        PendingPacksizeBreakdown,
        ReadyToPick,
        Waved,
        BeingPicked,
        Rating,
        Suspended,
        PendingShipCount,
        PendingDriverSignature,
        Shipped,
        PendingDeliveryCount,
        PendingDeliverySignature,
        Closed,
    }

    public enum SsccTypeEnum
    {
        CaseOrCarton,
        Pallet,
        IntraCompanyUse,
        Undefined,
    }
}