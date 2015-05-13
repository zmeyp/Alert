using System.Configuration;

namespace Alert.CheckQueue
{
    [ConfigurationCollection(typeof(QueueElement))]
    public class QueueElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueueElement)element).QueueName;
        }
    }
}
