using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using Alert.Common;

namespace Alert.CheckService
{
    [Export(typeof(ICheck))]
    public class CheckService : ICheck
    {
        #region ICheck Members

        /// <summary>
        /// Inspects if windows service is running on that machine
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Common.Alert> Inspect()
        {
            var messages = new List<Common.Alert>();

            var assembly = Assembly.GetExecutingAssembly();
            var mainConfig = ConfigurationManager.OpenExeConfiguration(assembly.Location);
            var config = mainConfig.GetSection("servicesToCheck") as ServiceSection;
            if (config == null)
                return messages;

            foreach (ServiceElement queue in config.Services)
            {
                var mySc = new ServiceController(queue.ServiceName);
                try
                {
                    string status = mySc.Status.ToString();
                    if (status != queue.ServiceStatus)
                    {
                        messages.Add(new Common.Alert
                        {
                            Message = "Alert: Service " + queue.ServiceName + " has status " + status, 
                            Source = "CheckService",
                            Status = status,
                            Target = queue.ServiceName
                        });
                    }

                }
                catch (Exception ex)
                {
                    messages.Add(new Common.Alert
                    {
                        Message = "Service not found. It is probably not installed. [exception=" + ex.Message + "]",
                        StackTrace = ex.StackTrace,
                        Source = "CheckService",
                        Target = queue.ServiceName
                    });
                    throw;
                }
                
            }


            return messages;
        }

        #endregion
    }
}
