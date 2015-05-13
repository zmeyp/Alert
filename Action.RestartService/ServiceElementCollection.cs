using System.Configuration;

namespace Action.RestartService
{
    [ConfigurationCollection(typeof(ServiceElement))]
    public class ServiceElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).ServiceName;
        }
    }
}
