using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastFilePreferenceMappingDTO
    {
        public int FastPreferenceMapID { get; set; }
        public string FASTPreferenceMapName { get; set; }
        public string programmetype { get; set; }

        public string producttype { get; set; }
        public string ProductTypeCd { get; set; }
        public string searchtype { get; set; }
        public string LoanPurpose { get; set; }
        public string location { get; set; }
        public string Tenant { get; set; }

        public int ProgramTypeId { get; set; }
        public int  ProductTypeId { get; set; }
        public int SearchTypeId { get; set; }       
        public int LocationId { get; set; }        
        public int TenantId { get; set; }


    }

    public class StateMappingDTO
    { 
        public string stateFIPS { get; set; }

        public string State { get; set; }

        public string PreferenceState { get; set; }
    }

    public class CountyMappingDTO
    {
        public string countyFIPS { get; set; }
        public string county { get; set; }

    }




}
