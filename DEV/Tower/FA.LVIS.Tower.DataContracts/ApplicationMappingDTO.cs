using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
  public class ApplicationMappingDTO:DataContractBase
    { 
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }

    }
}
