using System.Configuration;

namespace Task_2_BLC.Configuration
{
    public class RuleElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultFolder", IsRequired = true)]
        public string DefaultDirectory => (string)this["defaultFolder"];

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuleElement)element).FilePattern;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleElement();
        }
    }
}
