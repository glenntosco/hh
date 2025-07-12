using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class AllocationSettings
    {
        public Guid? UserId { get; set; }
        public List<Guid> Ids { get; set; } = new List<Guid>();

        public AllocationSource AllocationSourceEnum { get; set; } = Fulfillment.AllocationSource.PickTicket;
        public string AllocationSource
        {
            get => AllocationSourceEnum.ToString();
            set => AllocationSourceEnum = value.ParseEnum<AllocationSource>();
        }

        public List<ZoneSelection> Zones { get; set; } = new List<ZoneSelection>();

        public GeneralAllocationSettings General { get; set; } = new GeneralAllocationSettings();
        public ExpiryAllocationSettings Expiry { get; set; } = new ExpiryAllocationSettings();
        public BomAllocationSettings Boms { get; set; } = new BomAllocationSettings();
        public LotAllocationSettings Lots { get; set; } = new LotAllocationSettings();
        public PacksizeAllocationSettings Packsizes { get; set; } = new PacksizeAllocationSettings();
    }

    public class ZoneSelection
    {
        public bool Selected { get; set; }
        public string ZoneCode { get; set; }
        public string ProductHandling { get; set; }
    }

    public class GeneralAllocationSettings
    {
        public AllocationStyleEnum AllocationStyleEnum { get; set; } = AllocationStyleEnum.FIFO;
        public string[] AllocationStyleDatasource { get; set; }
        public string AllocationStyle
        {
            get => AllocationStyleEnum.ToString();
            set => AllocationStyleEnum = value.ParseEnum<AllocationStyleEnum>();
        }

        public bool ShowShortOptions { get; set; } = true;
        public string[] ShortOptionsDatasource { get; set; }
        public ShortOptionsEnum ShortOptionsEnum { get; set; } = ShortOptionsEnum.PickShort;
        public string ShortOptions
        {
            get => ShortOptionsEnum.ToString();
            set => ShortOptionsEnum = value.ParseEnum<ShortOptionsEnum>();
        }

        public bool ObeyZoneSequence { get; set; }
        public bool AllocateBulk { get; set; }
        public bool? HoldLetdown { get; set; }

        public string RestockZone { get; set; }
    }

    public class BomAllocationSettings
    {
        public bool HasBoms { get; set; }
        public bool GenerateProduction { get; set; }
        public bool AutoRelease { get; set; }
    }

    public class ExpiryAllocationSettings
    {
        public ExpiryAllocationPrecedenceEnum ExpiryAllocationPrecedenceEnum { get; set; } = ExpiryAllocationPrecedenceEnum.OldestFirst;
        public string[] ExpiryAllocationPrecedenceDatasource { get; set; }
        public string ExpiryAllocationPrecedence
        {
            get => ExpiryAllocationPrecedenceEnum.ToString();
            set => ExpiryAllocationPrecedenceEnum = value.ParseEnum<ExpiryAllocationPrecedenceEnum>();
        }

        public bool HasExpiry { get; set; }
    }

    public class LotAllocationSettings
    {
        public bool HasLots { get; set; }
        public LotAllocationStyleEnum LotAllocationStyleEnum { get; set; } = LotAllocationStyleEnum.Alphabetical;
        public string[] LotAllocationStyleDatasource { get; set; }
        public string LotAllocationStyle
        {
            get => LotAllocationStyleEnum.ToString();
            set => LotAllocationStyleEnum = value.ParseEnum<LotAllocationStyleEnum>();
        }

        public bool AllocatePartial { get; set; }
    }

    public class PacksizeAllocationSettings
    {
        public bool HasPacksizes { get; set; }
        public bool AllocateEaches { get; set; }
        public bool PacksizeBreakdown { get; set; }
        public bool BreakToInnerPacks { get; set; }

        public PacksizeAllocationStyleEnum PacksizeAllocationStyleEnum { get; set; } = PacksizeAllocationStyleEnum.Inherit;
        public string[] PacksizeAllocationStyleDatasource { get; set; }
        public string PacksizeAllocationStyle
        {
            get => PacksizeAllocationStyleEnum.ToString();
            set => PacksizeAllocationStyleEnum = value.ParseEnum<PacksizeAllocationStyleEnum>();
        }
    }

    public enum AllocationSource
    {
        PickTicket,
        ProductionOrder
    }

    public enum LotAllocationStyleEnum
    {
        Inherit,
        Alphabetical,
        MaxQtyFirst,
        MinQtyFirst,
    }

    public enum PacksizeAllocationStyleEnum
    {
        Inherit,
        BiggerPacksizeFirst,
        SmallerPacksizeFirst,
    }

    public enum AllocationStyleEnum
    {
        FIFO,
        LIFO,
        MostProductFirst,
        LeastProductFirst,
        BinName,
        //LeastPicks
    }

    public enum ExpiryAllocationPrecedenceEnum
    {
        NoPrecedence,
        OldestFirst,
        NewestFirst
    }

    public enum ShortOptionsEnum
    {
        PickShort,
        HoldShort,
        HoldProduction,
    }

    public enum UnAllocateProductionAction
    {
        PreventUnallocate,
        Delete,
        Detach,
    }

    public enum LetdownSequence
    {
        ByBin,
        BySku,
    }
}