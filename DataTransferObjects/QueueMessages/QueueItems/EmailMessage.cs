using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.QueueMessages.QueueItems
{
    [QueueSource("emails")]
    public class EmailMessage : BaseQueueItem
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void AddAttachment(string name, string url)
        {
            Attachments.Add(new EmailMessageAttachment
            {
                Name = name,
                UrlPath = url,
            });
        }

        public void AddAttachment(string url)
        {
            AddAttachment(null, url);
        }

        public List<EmailMessageAttachment> Attachments { get; set; } = new List<EmailMessageAttachment>();
    }

    public class EmailMessageAttachment
    {
        public string Name { get; set; }
        public string UrlPath { get; set; }
    }
}