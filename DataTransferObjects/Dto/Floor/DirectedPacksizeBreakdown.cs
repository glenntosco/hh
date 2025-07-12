using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class DirectedPacksizeBreakdown
    {
        public Guid Id { get; set; }
        public Guid? BinId { get; set; }
        public string Bin { get; set; }
        public Guid? LicensePlateId { get; set; }
        public string LicensePlate { get; set; }
        public ProductDetails Product { get; set; }
        public int FromPacksize { get; set; }
        public int ToPacksize { get; set; }
        public int Quantity { get; set; }
    }
}