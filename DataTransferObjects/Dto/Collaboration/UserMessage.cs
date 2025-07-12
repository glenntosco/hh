using System;

namespace Pro4Soft.DataTransferObjects.Dto.Collaboration
{
    public class UserMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();

        public DateTimeOffset Timestamp { get; set; }
        
        public Guid FromUserId { get; set; }
        public string FromUsername { get; set; }
        public Guid ToUserId { get; set; }
        public string ToUsername { get; set; }
        public bool IsNew { get; set; }

        public string Message { get; set; }
    }

    public class Contact
    {
        public Guid Id { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
        public string Username { get; set; }
        public bool IsOnline { get; set; }
        public int NewMessages { get; set; }
    }
}