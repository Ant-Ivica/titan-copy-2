using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC = FA.LVIS.Tower.DataContracts;
namespace FA.LVIS.Tower.DataContracts
{
    public class UserProfile
    {
        public string ID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
        public string Emailid { get; set; }

        public string Employeeid { get; set; }        

        public string sTenant { get; set; }

        public int TenantId { get; set; }

        public bool ManageBEQ { get; set; }
        public bool ManageTEQ { get; set; }

        public bool ManageAccessREQ { get; set; }
    }

    
}
