using FA.LVIS.Tower.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.RetryResubmitProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Resubmit TEQ Exception");
            Logger log = new Logger(typeof(TEQRetryResubmitionProcess));
            log.Info("Starting Retry Resubmit TEQ Exception");
            TEQRetryResubmitionProcess FDS = new TEQRetryResubmitionProcess();
            try
            {
                FDS.FetchResubmitTeqException();
                log.Info("Completed Retry Resubmit TEQ Exception");
            }
            catch (System.Exception ex)
            {
                log.Info("Error occured in Resubmit TEQ Exception. EXCEPTION: " + ex.ToString());
            }
        }
    }
}
