using System;
using Pro4Soft.DataTransferObjects.QueueMessages;

namespace Pro4Soft.P4Books.Common.QueueMessages.QueueItems
{

    [QueueSource("clone_tenant")]
    public class CloneTenantRequest : BaseQueueItem
    {
        public string NewAlias { get; set; }
        public Guid TaskId { get; set; }
    }
}