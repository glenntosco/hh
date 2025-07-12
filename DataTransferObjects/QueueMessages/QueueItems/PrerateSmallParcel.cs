using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("prerate_smallparcel")]
    public class PrerateSmallParcel: BaseQueueItem
    {
        public List<Guid> PickTicketIds { get; set; }
    }
}