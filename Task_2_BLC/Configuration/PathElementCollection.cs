using System.Configuration;

namespace Task_2_BLC.Configuration
{
    public class PathElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)element).Path;
        }
    }
}
