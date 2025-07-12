using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("prodorder_allocation")]
    public class ProductionOrderAllocationRequest : BaseQueueItem
    {
        public string Source { get; set; }
        public List<Guid> ProductionOrderIds { get; set; } = new List<Guid>();
        public AllocationSettings Settings { get; set; }
        public List<Guid> ZoneIds { get; set; } = new List<Guid>();
        public List<Guid> PriorityLocations { get; set; } = new List<Guid>();
    }
}