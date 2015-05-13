using System.Configuration;

namespace Action.RestartService
{
    public class ServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("services", IsDefaultCollection = true)]
        public ServiceElementCollection Services
        {
            get
            {
                return (ServiceElementCollection)this["services"];
            }
            set
            {
                this["services"] = value;
            }
        }
    }
}
