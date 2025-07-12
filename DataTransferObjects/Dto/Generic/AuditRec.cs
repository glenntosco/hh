using System;

namespace Pro4Soft.DataTransferObjects.Dto.Generic
{
    public class AuditRec
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public AuditType TypeEnum { get; set; }
        public string Type
        {
            get => TypeEnum.ToString();
            set => TypeEnum = value.ParseEnum<AuditType>();
        }

        public string SubType { get; set; }

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        public Guid? ClientId { get; set; }
        public string Client { get; set; }

        public Guid? UserId { get; set; }
        public string Username { get; set; }

        public string ReferenceType { get; set; } = null;
        public Guid? ReferenceId { get; set; } = null;
        public string ReferenceCode { get; set; } = null;

        public Guid? ProductId { get; set; } = null;
        public string Sku { get; set; }

        public string FromWarehouse { get; set; }
        public Guid? FromBinId { get; set; }
        public string FromBin { get; set; }

        public Guid? FromLpnId { get; set; }
        public string FromLpn { get; set; }
        public bool? IsFromBinEmpty { get; set; }
        public bool? IsFromLpnEmpty { get; set; }

        public string ToWarehouse { get; set; }
        public Guid? ToBinId { get; set; }
        public string ToBin { get; set; }
        public Guid? ToLpnId { get; set; }
        public string ToLpn { get; set; }

        public Guid? PacksizeId { get; set; }
        public string PacksizeName { get; set; }
        public string PacksizeBarcode { get; set; }
        public int? EachCount { get; set; }
        public int? NumberOfPacks { get; set; }
        public string LotNumber { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public string SerialNumber { get; set; }

        public decimal Quantity { get; set; }
        public decimal? AbsoluteFromQuantity { get; set; }
        public decimal? AbsoluteToQuantity { get; set; }

        public string Reason { get; set; } = null;

        public string IntegrationReference { get; set; }
        public string IntegrationMessage { get; set; }
        public DateTimeOffset? IntegrationTimestamp { get; set; }
    }

    public enum AuditType
    {
        ProductAdd,
        ProductMove,
        ProductRemove,
        LicensePlateMove,
        Handling,
        CustomAction,
        UserAction
    }
}