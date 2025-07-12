using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class StagingLocationLookup
    {
        public Guid? Id { get; set; }
        public string LocationCode { get; set; }
        public string ZoneCode { get; set; }
        public List<string> LicensePlates { get; set; } = new List<string>();
    }
}