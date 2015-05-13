using System.Configuration;

namespace Alert.CheckQueue
{
    public class QueueElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string QueueName 
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

        [ConfigurationProperty("size", IsRequired = true, DefaultValue = 100)]
        public int QueueMaxSize
        {
            get
            {
                return (int)this["size"];               
            }
            set
            {
                this["size"] = value;
            }
        }
    }
}
