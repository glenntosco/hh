using System;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class ShippingOptions:SmallParcelShippingOptions
    {
        //Private fleet
        public bool IsCountOnShip { get; set; }
        public bool IsSignOnShip { get; set; }
        public bool IsCountOnDelivery { get; set; }
        public bool IsSignOnDelivery { get; set; }

        public string Carrier { get; set; }
        public string Service { get; set; }

        //TruckLoad
        public string CarrierScac { get; set; }
        public FreightChargeTerms FreightChargeTerms { get; set; } = FreightChargeTerms.Empty;
    }

    public class SmallParcelShippingOptions
    {
        public SmallParcelPaymentType PaymentType { get; set; } = SmallParcelPaymentType.Prepay;
        
        public string CarrierAccountNumber { get; set; }
        public string InternationTaxId { get; set; }

        public bool IsResidential { get; set; }
        public bool COD { get; set; }
        public bool SaturdayPickup { get; set; }
        public bool SaturdayDelivery { get; set; }
        public bool SignatureRequired { get; set; }
        public bool PrintReturnLabel { get; set; }

        public string BillToName { get; set; }
        public string BillToPhone { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToCity { get; set; }
        public string BillToStateProvince { get; set; }
        public string BillToZipPostalCode { get; set; }
        public string BillToCountry { get; set; }
    }

    public enum SmallParcelPaymentType
    {
        Prepay,
        Collect,
        ThirdParty,
        Consignee
    }
}