using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Messaging;
using System.Reflection;
using Alert.Common;

namespace Alert.CheckQueue
{
    [Export(typeof(ICheck))]
    public class CheckQueue : ICheck
    {
        #region ICheck Members

        public IEnumerable<Common.Alert> Inspect()
        {
            var messages = new List<Common.Alert>();

            var assembly = Assembly.GetExecutingAssembly();
            var mainConfig = ConfigurationManager.OpenExeConfiguration(assembly.Location);
            var config = mainConfig.GetSection("queuesRetriever") as QueueRetrieverSection;
            if (config == null)
                return messages;
            foreach (QueueElement queue in config.Queues)
            {
                try
                {
                    var q = new MessageQueue(queue.QueueName);
                    var length = GetMessageCount(q);

                    if (length > queue.QueueMaxSize)
                    {
                        messages.Add(new Common.Alert
                        {
                            Message = "Alert: Number of Messages in the queue " + queue.QueueName + " is " + length, 
                            Source = "CheckQueue",
                            Target = queue.QueueName,
                            Status = length.ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    messages.Add(new Common.Alert 
                    { 
                        Message = ex.Message, 
                        Source = "CheckQueue",
                        StackTrace = ex.StackTrace,
                        Target = queue.QueueName
                    });
                    throw;
                }
            }

            return messages;
        }

        #endregion
        private static int GetMessageCount(MessageQueue q)
        {
            int count = 0;
            var me = q.GetMessageEnumerator2();
            while (me.MoveNext(new TimeSpan(0, 0, 0)))
            {
                count++;
            }
            return count;
        }

    }
}
