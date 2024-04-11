using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.Common;


namespace FA.LVIS.Tower.BEQRejectProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Reject BEQ Exception");
            Logger log = new Logger(typeof(BEQRejectProcess));
            log.Info("BEQRejectProcess: Starting Reject BEQ Exception");
            BEQRejectProcess FDS = new BEQRejectProcess();
            try
            {
                FDS.FetchRejectBeqException();
                log.Info("BEQRejectProcess: Completed Reject BEQ Exception");
            }
            catch (System.Exception ex)
            {
                log.Error("BEQRejectProcess: Error occured in Reject BEQ Exception. EXCEPTION: " + ex.ToString());
            }
        }
    }
}
