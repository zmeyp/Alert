using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Alert.Common;

namespace Action.RestartService
{
    [Export(typeof(IAction))]
    public class RestartService : IAction
    {
        private const int TimeoutMilliseconds = 10000;

        #region IAction Members

        void IAction.PerformAction(IEnumerable<Alert.Common.Alert> messagesParam)
        {
            var messages = messagesParam.ToList();

            if (!messages.Any())
                return;

            var assembly = Assembly.GetExecutingAssembly();
            var mainConfig = ConfigurationManager.OpenExeConfiguration(assembly.Location);
            var config = mainConfig.GetSection("servicesToRestart") as ServiceSection;
            if (config == null)
                return;
            
            var elements = config.Services.Cast<ServiceElement>().ToList();

            var controllers = from item in messages.Where(message => message.Source == "CheckService")
                              select elements.Where(s => s.ServiceName == item.Target && s.ServiceStatus.Contains(item.Status)).ToList();

            foreach (var srv in controllers.Select(item => item.FirstOrDefault()).Where(srv => srv != null))
            {
                StopService(srv.ServiceName);
                StartService(srv.ServiceName);
            }
        }

        private static void StartService(string serviceName)
        {
            var serviceController = new ServiceController(serviceName);
            if (serviceController.Status == ServiceControllerStatus.Running)
                return;

            var timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
            serviceController.Start();
            serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        private static void StopService(string serviceName)
        {
            var serviceController = new ServiceController(serviceName);
            if (serviceController.Status == ServiceControllerStatus.Stopped)
                return;

            var timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        }

        #endregion
    }
}
