using System;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("webhooks")]
    public class WebhookRequest : BaseQueueItem
    {
        public string Body { get; set; }
        public Guid? ClientId { get; set; }
    }
}