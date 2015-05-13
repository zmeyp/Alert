using System.Configuration;

namespace Alert.CheckQueue
{
    public class QueueRetrieverSection : ConfigurationSection
    {
        [ConfigurationProperty("queues", IsDefaultCollection = true)]
        public QueueElementCollection Queues
        {
            get
            {
                return (QueueElementCollection) this["queues"];
            }
            set
            {
                this["queues"] = value;
            }
        }
    }
}
