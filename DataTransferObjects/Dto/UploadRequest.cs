using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto
{
    public class UploadRequest
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
        public bool? UploadedSuceeded { get; set; }
        public string UploadMessage { get; set; }
        public bool ResetUploadCount { get; set; }
    }
}