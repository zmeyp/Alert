using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Alert.Common;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Alert.Inspector
{
    class Program
    {
        [ImportMany]
        public IEnumerable<ICheck> CheckSet { get; set; }
        [ImportMany]
        public IEnumerable<IAction> ActionSet { get; set; }

        //Here is the once-per-class call to initialize the log object
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        /// <summary>
        /// Main run
        /// For every Check dll located in the same directory runs Inspect function
        /// for every message collected from inspection runs alert action
        /// </summary>
        public void Run()
        {
            Log.Debug("Begin checking");
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog("."));
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            var messages = new List<Common.Alert>();
            foreach (var check in CheckSet)
            {
                try
                {
                    var checkMessages = check.Inspect();
                    messages.AddRange(checkMessages);
                }
                catch (Exception ex)
                {
                    messages.Add(new Common.Alert
                                        {
                                            Message = ex.Message,
                                            StackTrace = ex.StackTrace,
                                            Source = "Inspector"
                                        });

                    Log.Error("Error running the Check", ex);
                }

            }

            foreach (var action in ActionSet)
            {
                try
                {
                    action.PerformAction(messages);
                }
                catch (Exception ex)
                {
                    Log.Error("Error while performing the action", ex);
                }
            }

            container.Dispose();

            Log.Debug("End Checking");
        }
    }
}
