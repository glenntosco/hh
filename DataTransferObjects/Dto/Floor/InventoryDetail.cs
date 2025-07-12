using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public abstract class InventoryDetail
    {
        public DateTime? ExpiryDate { get; set; }
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }
        public decimal Quantity { get; set; }
        public Guid? PacksizeId { get; set; }
        public int? PacksizeEachCount { get; set; }
    }
}