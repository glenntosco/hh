using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class PacksizeConvertOperation
    {
        public Guid? ProductId { get; set; }

        public Guid? FromBinId { get; set; }
        public string FromLpn { get; set; }
        public Guid FromPacksizeId { get; set; }

        public Guid? ToBinId { get; set; }
        public string ToLpn { get; set; }
        public Guid ToPacksizeId { get; set; }

        public string LotNumber { get; set; }
        public string Expiry { get; set; }

        public int Quantity { get; set; }
    }
}