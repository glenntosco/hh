using System;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("invoice_generation")]
    public class InvoiceGenerateRequest : BaseQueueItem
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Guid ClientId { get; set; }
        public string RootUrl { get; set; }
    }
}