using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public class CategoryMapping : DataContractBase
    {
        public int CategoryId { get; set; }
                
        public string CategoryName { get; set; }

        public string ObjectCD { get; set; }
                
        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public int Applicationid { get; set; }

        public string ApplicationName { get; set; }


    }
}
