using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastProcessTaskEventDTO : DataContractBase
    {
        public int ProcesseventId { get; set;}

        public int TaskeventId { get; set;}

        public string Processdescription { get; set;}

        public string Taskdescription { get; set;}


    }
}
