using System;

namespace Pro4Soft.DataTransferObjects.Dto
{
    public class TenantDetails
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; }
        public string Alias { get; set; }
        public string Phone { get; set; }
        public string LanguageId { get; set; }
        public string WelcomeMessage { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }

        public string TimeZone { get; set; }

        public string CustomSettings { get; set; }

        public string CustomPackslip { get; set; }
        public Guid? CustomPackslipId { get; set; }

        public string CustomCommInvoice { get; set; }
        public Guid? CustomCommInvoiceId { get; set; }

        public string CustomProforma { get; set; }
        public Guid? CustomProformaId { get; set; }

        public string CustomRma { get; set; }
        public Guid? CustomRmaId { get; set; }

        public string CustomSlip { get; set; }
        public Guid? CustomSlipId { get; set; }

        public string Custom3PLInvoice { get; set; }
        public Guid? Custom3PLInvoiceId { get; set; }

        public DateTimeOffset? LicenseEnd { get; set; }
        public int ConcurrentConnections { get; set; }
    }
}
