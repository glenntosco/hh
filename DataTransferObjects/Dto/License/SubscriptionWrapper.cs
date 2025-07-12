using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.License
{
    public class SubscriptionWrapper
    {
        public string HardwareKey { get; set; }
        public string Alias { get; set; }
        public Guid TenantId { get; set; }
        public int AvailableLicenses { get; set; }
        public List<LicenseWrapper> Licenses { get; set; }
    }

    public class LicenseWrapper
    {
        public string Name { get; set; }
        public DateTime LicenseStart { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public int LicenseCount { get; set; }
    }
}
