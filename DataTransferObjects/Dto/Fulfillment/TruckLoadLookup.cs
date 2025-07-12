using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Floor;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class TruckLoadLookup
    {
        public Guid Id { get; set; }
        public string TruckLoadNumber { get; set; }
        public string BillOfLadingNumber { get; set; }
        public TruckLoadState TruckLoadState { get; set; }
        public WarehouseLookup Warehouse { get; set; }
        
        public Guid? MasterTruckLoadId { get; set; }
        public bool IsMasterTruckLoad { get; set; }

        public Guid? ClientId { get; set; }
        public string ClientName { get; set; }

        public Guid? CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public Guid? DockDoorId { get; set; }
        public string DockDoorBarcode { get; set; }
        public string DockDoor { get; set; }

        public int TotalTotes { get; set; }
        public decimal TotalUnits { get; set; }
        public decimal TotalWeight { get; set; }
        public UnitOfMeasure WeightUoM { get; set; }

        public List<ToteLookup> Totes { get; set; } = new List<ToteLookup>();
    }

    public enum TruckLoadState
    {
        Draft,
        Consolidating,
        Staging,
        PendingShipperSignature,
        PendingCarrierSignature,
        Shipped,
    }

    public enum FreightChargeTerms
    {
        Empty,
        Prepaid,
        Collect,
        ThirdParty
    }

    public enum TrailerLoadedType
    {
        Empty,
        ByShipper,
        ByDriver
    }

    public enum FreightCountedType
    {
        Empty,
        ByShipper,
        ByDriverPallets,
        ByDriverPieces
    }

    public enum TruckLoadConsolidationType
    {
        Any,
        MatchFullAddress,
        MatchRoute,
        MatchApptNumber,
        MatchStateProvince,
        MatchCity,
        MatchZipPostal,
    }
}