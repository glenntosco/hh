using System;

namespace Pro4Soft.DataTransferObjects.QueueMessages
{
    public class BaseQueueItem
    {
        public Guid? TenantId { get; set; }
        public string Alias { get; set; }

        //public string MasterConnectionString { get; set; }
        public string TenantConnectionString { get; set; }
        public Guid? UserId { get; set; }
        public Guid? UserSessionId { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class QueueSourceAttribute : Attribute
    {
        public string QueueName { get; }

        public QueueSourceAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
