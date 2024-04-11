using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class DashBoardExceptionDTO
    {
        public string ExceptionName { get; set; }

        public int NoOfExceptions { get; set; }

        public string DateTime { get; set; }

        public int ThreshholdCount { get; set; }
    }
    
    public class DashBoardGraphicalExceptionDTO
    {

        public int QueueCount { get; set; }

        public int ArchiveCount { get; set; }

        public int NewCount { get; set; }

        public int ActiveCount { get; set; }

        public int HoldCount { get; set; }


        public string Hour { get; set; }

        public string Datetime { get; set; }
    }
}
