using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FASTOfficeMap : DataContractBase
    {
        public int FASTOfficeMapId { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public int ExternalApplicationID { get; set; }

        public int RegionId { get; set; }

        public int EscrowRegionId { get; set; }

        public int TitleOfficeId { get; set; }

        public int EscrowOfficeId { get; set; }

        public string FASTOfficeMapDesc { get; set; }

        public string ExternalId { get; set; }

        //public string Provider { get; set; }
        public string Region { get; set; }

        public string TitleOffice { get; set; }

        public string EscrowOffice { get; set; }

        public string StateCode { get; set; }

        public string County { get; set; }

        public int TenantId { get; set; }

        public int? Contactid { get; set; }

        public string Tenant { get; set; }

        public int? locationId { get; set; }

        
        public int? TokenTypeCodeId { get; set; }


        public string Location { get; set; }

        public string EscrowAssistantCode { get; set; }

        public string EscrowOfficerCode { get; set; }

        public string TitleOfficerCode { get; set; }

        public string EscrowOfficer { get; set; }

        public string TitleOfficer { get; set; }

        public List<ConditionPreferenceDTO> LocationCondition { get; set; }

        public List<Contacts> ContactList { get; set; }

        public Nullable<int> CustomerId { get; set; }

        public string ApplicationName { get; set; }
    }

}

