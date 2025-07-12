using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Fulfillment
{
    public class CartonizationSettings
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();

        public bool? AllowSkuMixing { get; set; } = true;
        public bool? AllowPacksizeMixing { get; set; } = true;
        public int? MaxUnitsPerCarton { get; set; }
        public decimal? MaxWeightPerCarton { get; set; }
    }
}