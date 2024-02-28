using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class Constants
    {
        public const string TENANT_ID = "TenantId";
        public const string EMPLOYEE_ID = "EmployeeId";
        public const string USER_ID = "UserId";

        public const string ROLE_SUPERADMIN = "SuperAdmin";
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_USER = "User";

        public const string APPLICATION_FAST = "FAST";
        public const string APPLICATION_REALEC = "RealEC";
        public const string APPLICATION_KEYSTONE = "Keystone";
        public const string APPLICATION_LVIS = "LVIS";
        public const string APPLICATION_RF = "RF";
        public const string ENRICHMENT_FAST_QUEUE = "ENRICHMENT.FAST";
        
        public const string CLAIM_MANAGEBEQ = "Tower.CanManageBEQ";
        public const string CLAIM_MANAGETEQ = "Tower.CanManageTEQ";
        public const string CLAIM_MANAGEACCESSREQ = "Tower.CanManageAccessREQ";

        public const int EXCEPTION_STATUS_TYPECODE = 200;        
        public const string EXCEPTION = "EXCEPTION";        

        public const string ENRICHMENT = "ENRICHMENT";

        public const string RESUBMIT = "RESUBMIT";
        public const string CONVOY = "CONVOY";

        public const string TENANT_ID_RF = "5";//Rates and Fees Tenant 
        public const string INVALIDATED = "Invalidated";
        public const string TrackingId = "Trackingid"; 

        public const int ROLE_SUPERADMIN_ID = 3;
        public const int TENENT_LVIS_ID = 3;

        public const int REALEC_APPID = 4;
        public const int OPENAPI_APPID = 34;
        public const int SETTLMENTSERVICES_APPID = 9;
        public const int CALCULATOR_APPID = 8; 
    }
}
