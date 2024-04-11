using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.Common;


namespace FA.LVIS.Tower.BEQResubmitProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Resubmit BEQ Exception");
            Logger log = new Logger(typeof(BEQResubmitProcess));
            log.Info("BEQResubmitProcess: Starting Resubmit BEQ Exception");
            BEQResubmitProcess FDS = new BEQResubmitProcess();
            try
            {
                FDS.FetchResubmitBeqException();
                log.Info("BEQResubmitProcess: Completed Resubmit BEQ Exception");
            }
            catch (System.Exception ex)
            {
                log.Error("BEQResubmitProcess: Error occured in Resubmit BEQ Exception. EXCEPTION: " + ex.ToString());
            }
        }
    }
}
