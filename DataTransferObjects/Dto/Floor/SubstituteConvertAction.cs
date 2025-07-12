using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class SubstituteConvertAction
    {
        public ProductOperation FromOperation { get; set; }
        public Guid? FromBinId { get; set; }
        public string FromLpn { get; set; }

        public ProductOperation ToOperation { get; set; }
        public Guid? ToBinId { get; set; }
        public string ToLpn { get; set; }
    }
}