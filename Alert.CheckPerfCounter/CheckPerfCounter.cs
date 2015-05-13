using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Alert.Common;

namespace Alert.CheckPerfCounter
{
    [Export(typeof(ICheck))]
    public class CheckPerfCounter : ICheck
    {

        #region ICheck Members

        public IEnumerable<Common.Alert> Inspect()
        {
            var messages = new List<Common.Alert>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Configuration mainConfig = ConfigurationManager.OpenExeConfiguration(assembly.Location);
            var config = mainConfig.GetSection("countersToCheck") as PerformanceSection;
            if (config == null)
                return messages;

            foreach (PerformanceElement counter in config.Counters)
            {
                var cntr = new PerformanceCounter
                {
                    CategoryName = counter.Category,
                    CounterName = counter.CounterName,
                    InstanceName = counter.InstanceName
                };

                var counterValue = cntr.NextValue();

                if (counterValue.CompareTo(0) == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    counterValue = cntr.NextValue();                    
                }

                if (counterValue > counter.MaxValue)
                {
                    messages.Add(new Common.Alert
                    {
                        Message = string.Format("Alert: Performance Counter {0}  is equal to {1} greater than Max Value {2}",
                            counter.Category + "\\" + counter.CounterName + "\\" + counter.InstanceName,
                            counterValue,
                            counter.MaxValue),
                        Source = "CheckPerfCounter",
                        Target = counter.Category + "|" + counter.CounterName + "|" + counter.InstanceName,
                        Status = counterValue.ToString()
                                                    
                    });
                }

                if (counterValue < counter.MinValue)
                {
                    messages.Add(new Common.Alert
                    {
                        Message = string.Format("Alert: Performance Counter {0}  is equal to {1} less than Min Value {2}",
                            counter.Category + "\\" + counter.CounterName + "\\" + counter.InstanceName,
                            counterValue,
                            counter.MinValue),
                        Source = "CheckPerfCounter",
                        Target = counter.Category + "|" + counter.CounterName + "|" + counter.InstanceName,
                        Status = counterValue.ToString()
                    });
                }
                
            }
            return messages;
        }
        #endregion
    }
}