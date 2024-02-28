using LVIS.Infrastructure.Logging;
using System;
using System.Diagnostics;

namespace FA.LVIS.Tower.FastDataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting FAST Regions Data Sync");
         //   EventLog.WriteEntry("FastDataSync", "Starting FAST Regions Data Sync");

            ILogger sLogger = LoggerFactory.GetLogger(typeof(FASTDataSync));

            sLogger.Info("Starting FAST Regions Data Sync");


            try
            {
                FASTDataSync FDS = new FASTDataSync(sLogger);
                sLogger.Info("Trying FAST Regions Data Sync");
                FDS.SyncFastRegions();
                sLogger.Info("Complted FAST Regions Data Sync");
                EventLog.WriteEntry("FastDataSync", "Completed FAST Regions Data Sync");
                sLogger.Info("Completed FAST Regions Data Sync");
            }
            catch (Exception ex)
            {
                sLogger.Info("Error occured in FAST Regions Data Sync. EXCEPTION: " + ex.ToString());
                EventLog.WriteEntry("FastDataSync", ex.Message + "Exception" + ex.InnerException, EventLogEntryType.Error, 121, short.MaxValue);
            }
        }
    }
}
