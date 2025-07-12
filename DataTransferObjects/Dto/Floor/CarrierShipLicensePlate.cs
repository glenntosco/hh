using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class CarrierShipLicensePlate
    {
        public string LicensePlateCode { get; set; }
        public string Carrier { get; set; }
        public int CartonsCount { get; set; }
        public int OrdersCount { get; set; }

        public List<Guid> ToteIds { get; set; }
    }
}