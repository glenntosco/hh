using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class TruckLoadShipSettings
    {
        public List<Guid> TruckLoadIds { get; set; } = new List<Guid>();
        public string SealNumber { get; set; }
    }

    public class PickTicketShipSettings
    {
        public List<Guid> PickTicketIds { get; set; } = new List<Guid>();
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string VehicleId { get; set; }
        public string SealNumber { get; set; }
    }
}