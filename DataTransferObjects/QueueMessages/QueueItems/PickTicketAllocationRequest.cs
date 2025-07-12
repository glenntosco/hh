using System;
using System.Collections.Generic;
using Pro4Soft.DataTransferObjects.Dto.Fulfillment;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("pick_allocation")]
    public class PickTicketAllocationRequest : BaseQueueItem
    {
        public string Source { get; set; }
        public List<Guid> PickTicketIds { get; set; } = new List<Guid>();
        public AllocationSettings Settings { get; set; }
        public List<Guid> ZoneIds { get; set; } = new List<Guid>();
        public List<Guid> PriorityLocations { get; set; } = new List<Guid>();
    }
}