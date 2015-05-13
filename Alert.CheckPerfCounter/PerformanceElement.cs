using System.Configuration;

namespace Alert.CheckPerfCounter
{
    public class PerformanceElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("category", IsRequired = true)]
        public string Category
        {
            get
            {
                return (string)this["category"];
            }
            set
            {
                this["category"] = value;
            }
        }
        [ConfigurationProperty("counterName", IsRequired = true)]
        public string CounterName
        {
            get
            {
                return (string)this["counterName"];
            }
            set
            {
                this["counterName"] = value;
            }
        }
        [ConfigurationProperty("instanceName", IsRequired = true)]
        public string InstanceName
        {
            get
            {
                return (string)this["instanceName"];
            }
            set
            {
                this["instanceName"] = value;
            }
        }

        [ConfigurationProperty("minValue", IsRequired = false)]
        public int MinValue
        {
            get
            {
                return (int)this["minValue"];
            }
            set
            {
                this["minValue"] = value;
            }
        }

        [ConfigurationProperty("maxValue", IsRequired = false)]
        public int MaxValue
        {
            get
            {
                return (int)this["maxValue"];
            }
            set
            {
                this["maxValue"] = value;
            }
        }
    }
}
