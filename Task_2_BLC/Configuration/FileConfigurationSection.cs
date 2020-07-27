using System.Configuration;
using System.Globalization;

namespace Task_2_BLC.Configuration
{
    public class FileConfigurationSection : ConfigurationSection
    {
        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules", IsRequired = true)]
        public RuleElementCollection Rules => (RuleElementCollection)this["rules"];

        [ConfigurationProperty("culture", DefaultValue = "en-US", IsRequired = false)]
        public CultureInfo Culture => (CultureInfo)this["culture"];

        [ConfigurationCollection(typeof(PathElement), AddItemName = "directory")]
        [ConfigurationProperty("directories", IsRequired = false)]
        public PathElementCollection Directories => (PathElementCollection)this["directories"];
    }
}
