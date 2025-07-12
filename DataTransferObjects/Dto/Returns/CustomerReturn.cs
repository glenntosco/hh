using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Returns
{
    public class CustomerReturn
    {
        public Guid Id { get; set; }
        public string CustomerReturnNumber { get; set; }

        public CustomerReturnState CustomerReturnState { get; set; }

        public Guid? ClientId { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerCode { get; set; }

        public Guid WarehouseId { get; set; }

        public List<CustomerReturnLine> Lines { get; set; } = new List<CustomerReturnLine>();
    }

    public enum CustomerReturnState
    {
        Draft,
        NotReceived,
        PartiallyReceived,
        Received,
        Closed
    }
}