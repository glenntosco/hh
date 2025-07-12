using System;
using System.Linq;
using System.Reflection;

namespace Pro4Soft.DataTransferObjects.Configuration
{
	public class ConfigDefinitionAttribute : Attribute
	{
		public ConfigDefinitionAttribute(ConfigType type, string description = null, object defaultValue = null, string[] multiSelectOptions = null, bool showOnUi = true)
		{
			Type = type;
			DefaultValue = defaultValue;
			Description = description;
            MultiSelectOptions = multiSelectOptions;
            ShowOnUi = showOnUi;
		}

		public ConfigType Type { get; set; }
		public string Description { get; set; }
        public string[] MultiSelectOptions { get; set; }
        public object DefaultValue { get; set; }
		public bool ShowOnUi { get; set; }

        public static ConfigDefinitionAttribute GetConfigAttr(string name)
        {
            var prop = typeof(ConfigConstants).GetFields(BindingFlags.Public | BindingFlags.Static).SingleOrDefault(c => c.Name == name);
            if (prop == null)
                throw new BusinessWebException($"Config Property [{name}] does not exist");
            if (!(GetCustomAttribute(prop, typeof(ConfigDefinitionAttribute)) is ConfigDefinitionAttribute attr))
                throw new BusinessWebException($"Config Property Attribute is missing");
            return attr;
        }
    }

    public enum ConfigType
    {
        String,
        MultilineString,
        EmailBody,
        Password,
        Int,
        Double,
        Bool,
        MultiSelect
    }
}