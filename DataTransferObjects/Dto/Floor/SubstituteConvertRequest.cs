using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class SubstituteConvertRequest
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public Guid FromProductId { get; set; }
        public string FromImageUrl { get;set; }
        public string BinCode { get; set; }
        public string LicensePlateCode { get; set; }
        public string Instructions { get; set; }
        public decimal? Consume { get; set; }
        public decimal? Produce { get; set; }

        public Guid ToProductId { get; set; }
        public string ToImageUrl { get; set; }

        public string GetPickBin()
        {
            string result;
            if (!string.IsNullOrWhiteSpace(LicensePlateCode))
            {
                result = $"[{LicensePlateCode}]";
                if(!string.IsNullOrWhiteSpace(BinCode))
                    result += $"@ [{BinCode}]";
            }
            else
                result = $"[{BinCode}]";
            return result;
        }
    }
}