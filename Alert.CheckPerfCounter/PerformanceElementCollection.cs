using System.Configuration;

namespace Alert.CheckPerfCounter
{
    [ConfigurationCollection(typeof(PerformanceElement))]
    public class PerformanceElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new PerformanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PerformanceElement)element).Key;
        }
    }
}
