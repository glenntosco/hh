using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class LocationLookup
    {
        public Guid? LpnBinId { get; set; }
        public Guid? Id { get; set; }
        public string LocationCode { get; set; }
        public string ZoneCode { get; set; }
        public ProductHandlingType ProductHandlingEnum { get; set; }
        public bool IsBin { get; set; }
        public bool IsLpn { get; set; }
        public bool IsDockDoor { get; set; }
        public List<BinProductContent> Contents { get; set; } = new List<BinProductContent>();
        public List<ToteLookup> Totes { get; set; } = new List<ToteLookup>();

        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public UnitOfMeasure LengthUnitOfMeasure { get; set; }

        public decimal? Weight { get; set; }
        public UnitOfMeasure WeightUnitOfMeasure { get; set; }

        public BinDirection Direction { get; set; } = BinDirection.None;

        public virtual string QueryUrl
        {
            get
            {
                switch (Direction)
                {
                    case BinDirection.Out:
                        return IsLpn ? $"fromLpn={LocationCode}" : IsDockDoor ? $"fromDockDoorId={Id}" : $"fromBinId={Id}";
                    case BinDirection.In:
                        return IsLpn ? $"toLpn={LocationCode}{(LpnBinId == null ? null : $"&toBinId={LpnBinId}")}" : IsDockDoor ? $"toDockDoorId={Id}" : $"toBinId={Id}";
                    default:
                        return IsLpn ? $"lpn={LocationCode}{(LpnBinId == null ? null : $"&binId={LpnBinId}")}" : IsDockDoor ? $"dockDoorId={Id}" : $"binId={Id}";
                }
            }
        }

        public Guid? BinId => IsBin ? Id: null;
        public Guid? LicensePlateId => IsLpn ? Id : null;
        public string LpnCode => IsLpn ? LocationCode : null;
        public Guid? DockDoorId => IsDockDoor ? Id : null;
    }

    public enum ProductHandlingType
    {
        ByProduct,
        ByLpn
    }

    public enum PacksizeHandlingType
    {
        Unchanged,
        PreventEaches,
        PreventPacks,
        ConvertPacksToEaches
    }

    public class BinProductContent
    {
        public Guid InventoryLocationId { get; set; }
        public Guid ProductId { get; set; }
        public string LicensePlate { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal OpenQuantity => TotalQuantity - ReservedQuantity;

        public List<BinProductContentDetail> BinDetails { get; set; } = new List<BinProductContentDetail>();
    }

    public class BinProductContentDetail : InventoryDetail
    {
        public Guid InventoryLocationDetailId { get; set; }
    }

    public class BinDetail
    {
        public Guid Id { get; set; }
        public string BinCode { get; set; }
    }

    public enum BinDirection
    {
        None,
        Out,
        In
    }
}
