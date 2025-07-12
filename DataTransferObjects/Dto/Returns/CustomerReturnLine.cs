using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Returns
{
    public class CustomerReturnLine
    {
        public Guid Id { get; set; }
        public Guid CustomerReturnId { get; set; }

        public int LineNumber { get; set; }

        public Guid ProductId { get; set; }
        public Guid? ToteLineId { get; set; }

        public string CustomerReturnNumber { get; set; }

        public int? Packsize { get; set; }
        public int? NumberOfPacks { get; set; }
        public string Instructions { get; set; }

        public List<CustomerReturnLineDetail> LineDetails { get; set; } = new List<CustomerReturnLineDetail>();

        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal DamagedQuantity { get; set; }
        public decimal OutstandingQuantity => Quantity - (ReceivedQuantity + DamagedQuantity);
    }
}