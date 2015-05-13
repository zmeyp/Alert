using System.Configuration;

namespace Alert.CheckPerfCounter
{
    public class PerformanceSection : ConfigurationSection
    {
        [ConfigurationProperty("counters", IsDefaultCollection = true)]
        public PerformanceElementCollection Counters
        {
            get
            {
                return (PerformanceElementCollection)this["counters"];
            }
            set
            {
                this["counters"] = value;
            }
        }
    }
}
