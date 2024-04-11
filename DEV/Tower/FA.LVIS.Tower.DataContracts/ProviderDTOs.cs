using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FAOffice : DataContractBase
    {
        public int FASTOfficeID { get; set; }
        public string PPID;
    }

    public class ImportProvider : DataContractBase
    {     
        public string ExternalID { get; set; }

        public bool? IsUseRuleEngine { get; set; }

        public string ExternalApplicationName { get; set; }

        public string InternalApplicationName { get; set; }

        public string Tenant { get; set; }

    }
    public class Provider: DataContractBase
    {
        //public int ID { get; set; }
        public int  ProviderID { get; set; }

        public string ProviderName { get; set; }
        public int ServiceProviderId { get; set; }
        public string ExternalID { get; set; }

        public int ? ExternalApplicationID { get; set; }

        public int  InternalApplicationID { get; set; }

        public bool? IsUseRuleEngine { get; set; }

        public string ExternalApplication { get; set; }

        public string InternalApplication { get; set; }

        public int TenantId { get; set; }
        public string Tenant { get; set; }

        public int RegionID { get; set; }
        public int TitleOfficeID { get; set; }

        public int EscrowOfficeID { get; set; } 

        public string Description { get; set; }

        public string UseRuleEngine { get; set; }

        public List<ConditionPreferenceDTO> LocationCondition { get; set; }

        public bool IsBindOnly { get; set; }




    }

    public class ProductProviderMap : DataContractBase
    {
        public int ProductProviderMapId { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public string ExternalId { get; set; }

        public int ContactId { get; set; }

        public string ContactName { get; set; }

        public int? LocationId { get; set; }

        public string LocationName { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
        
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ServiceId { get; set; }

        public string Service { get; set; }

        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public int ApplicationId { get; set; }

        public string Application { get; set; }

        public string CreatedDate { get; set; }

        public int CreatedById { get; set; }

        public string LastModifiedDate { get; set; }

        public int LastModifiedById { get; set; }        

    }
}
