using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using Alert.Action.SendEmail.MMS;
using Alert.Common;

namespace Alert.Action.SendEmail
{
    [Export(typeof(IAction))]
    public class SendEmail : IAction
    {

        #region IAction Members

        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void PerformAction(IEnumerable<Common.Alert> messages)
        {
            var enumerable = messages as Common.Alert[] ?? messages.ToArray();
            if (messages == null || !enumerable.Any())
                return;

            var assembly = Assembly.GetExecutingAssembly();
            var mainConfig = ConfigurationManager.OpenExeConfiguration(assembly.Location);
            string serverAddress = mainConfig.AppSettings.Settings["ServerAddress"].Value;
            string fromAddress = mainConfig.AppSettings.Settings["FromAddress"].Value;
            string to = mainConfig.AppSettings.Settings["To"].Value;
            string subject = mainConfig.AppSettings.Settings["Subject"].Value;

            foreach (Common.Alert alert in enumerable)
            {
                Send(fromAddress, to, subject, BuildMessage(alert), DateTime.Now, serverAddress);
            }
        }

        #endregion

        /// <summary>
        /// Calls service to send alert email.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="recipients">The recipients.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="sendDate">The send date.</param>
        /// <param name="serverAddress">The server address.</param>
        /// <returns></returns>
        private static void Send(string fromAddress, string recipients, string subject, string message, DateTime sendDate, string serverAddress)
        {
            var mailItem = new Message
            {
                Type = MessageType.Email,
                RecipientCollection = new ArrayOfString()
            };

            mailItem.RecipientCollection.AddRange(recipients.Split(';'));
            mailItem.FromAddress = fromAddress;
            mailItem.DoNotSendUntilDTS = sendDate;

            mailItem.Subject = subject;
            mailItem.Body = message;

            var basicHttpBinding = new BasicHttpBinding
            {
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows,
                        ProxyCredentialType = HttpProxyCredentialType.None,
                        Realm = ""
                    }
                }
            };

            basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            basicHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;

            var service = new ServiceSoapClient(basicHttpBinding, new EndpointAddress(serverAddress));

            service.InsertMessage(mailItem);
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <returns></returns>
        private static string BuildMessage(Common.Alert alert)
        {            
            var message = new StringBuilder();
            message.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Alert Message</title>");
            message.Append("<style type=\"text/css\">.brd{border: 1px solid #D2D593;font:bold 12px/16px Arial,Helvetica,sans-serif;}</style></head>");
            message.Append("<body style=\"font:bold 12px/16px Arial,Helvetica,sans-serif;\">");
            message.Append("<table style=\"border:1px solid #D2D593;\"><tr><td class=\"brd\"><label for=\"Source\">Source</label></td><td class=\"brd\">");
            message.Append(alert.Source);
            message.Append("</td></tr><tr><td class=\"brd\"><label for=\"Target\">Target</label></td><td class=\"brd\">");
            message.Append(alert.Target);
            message.Append("</td></tr><tr><td class=\"brd\"><label for=\"MachineName\">Machine Name</label></td><td class=\"brd\">");
            message.Append(Environment.MachineName);
            message.Append("</td></tr><tr><td class=\"brd\"><label for=\"Status\">Status</label></td><td class=\"brd\">");
            message.Append(alert.Status);
            message.Append("</td></tr><tr><td class=\"brd\"><label for=\"Message\">Message</label></td><td class=\"brd\">");
            message.Append(alert.Message);
            message.Append("</td></tr><tr><td class=\"brd\"><label for=\"StackTrace\">Stack Trace</label></td><td class=\"brd\">");
            message.Append(alert.StackTrace);
            message.Append("</td></tr></table></body></html>");

            return message.ToString();
        }
    }
}
