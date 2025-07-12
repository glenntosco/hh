using System;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("import_tenant")]
    public class ImportTenantRequest : BaseQueueItem
    {
        public string Url { get; set; }
        public Guid? NewTenantId { get; set; }
        public string NewAlias { get; set; }
    }
}