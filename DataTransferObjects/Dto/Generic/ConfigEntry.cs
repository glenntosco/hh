using System;
using Pro4Soft.DataTransferObjects.Configuration;

namespace Pro4Soft.DataTransferObjects.Dto.Generic
{
    public class ConfigEntry
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ConfigType Type { get; set; }

        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }
        public string[] MultiSelectOptions { get; set; }
    }
}