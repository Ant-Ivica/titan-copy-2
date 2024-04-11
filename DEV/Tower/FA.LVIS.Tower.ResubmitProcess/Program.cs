using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.Common;


namespace FA.LVIS.Tower.ResubmitProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Resubmit TEQ Exception");
            Logger log = new Logger(typeof(TEQResubmitionProcess));
            log.Info("Starting Resubmit TEQ Exception");
            TEQResubmitionProcess FDS = new TEQResubmitionProcess();
            try
            {
                FDS.FetchResubmitTeqException();
                log.Info("Completed Resubmit TEQ Exception");
            }
            catch (System.Exception ex)
            {
                log.Info("Error occured in Resubmit TEQ Exception. EXCEPTION: " + ex.ToString());
            }
        }
    }
}
