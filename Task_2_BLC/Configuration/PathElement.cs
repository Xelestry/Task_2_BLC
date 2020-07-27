using System.Configuration;

namespace Task_2_BLC.Configuration
{
    public class PathElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
        public string Path => (string)this["path"];
    }
}
