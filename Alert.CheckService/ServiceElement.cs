using System.Configuration;

namespace Alert.CheckService
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string ServiceName
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("status", IsRequired = true, DefaultValue = "Running")]
        public string ServiceStatus
        {
            get
            {
                return (string)this["status"];
            }
            set
            {
                this["status"] = value;
            }
        }
    }
}
