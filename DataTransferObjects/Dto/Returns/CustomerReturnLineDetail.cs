using System;

namespace Pro4Soft.DataTransferObjects.Dto.Returns
{
    public class CustomerReturnLineDetail
    {
        public string LotNumber { get; set; }
        public string SerialNumber { get; set; }

        public string ExpiryString { get; set; }

        public int? PacksizeEachCount { get; set; }

        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }

        public decimal OutstandingQuantity => Quantity - ReceivedQuantity;
    }
}