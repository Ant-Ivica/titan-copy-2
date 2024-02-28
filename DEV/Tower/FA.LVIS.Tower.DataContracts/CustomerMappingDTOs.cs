using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
   public  class CustomerMapping : DataContractBase
    {
        public int LVISCustomerID { get; set; }

        public string CustomerName { get; set; }

        public string Category { get; set; }

        public int? CategoryId { get; set; }

        public int Applicationid { get; set; }

        public string ApplicationName { get; set; }

        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public string CAPIClientID { get; set; }

        public string ServiceSelect { get; set; }

        public string DTDeliveryUrl { get; set; }

        public string DTCredentials { get; set; }

        public List<TypeCodeDTO> CustomerPreference { get; set; }

        public List<ServicePreference> ServicePreference { get; set; }
        public string CustomerUserId { get; set; }
        public bool GenerateCredential { get; set; }
    }


    public class LocationsMappings : DataContractBase
    {
        public int LocationId { get; set; }

        public string ExternalId { get; set; }

        public string LocationName { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int TenantId { get; set; }

        public string Tenant { get; set; }

        public string Notes { get; set; }

        public List<ServicePreference> ServicePreference { get; set; }
    }

    public class Subscription : DataContractBase
    {
        public int SubscriptionId { get; set; }

        public int MessageTypeId { get; set; }

        public string MessageTypeName { get; set; }

        public string MessageTypeDesc { get; set; }

        public int ApplicationId { get; set; }

        public int? CustomerId { get; set; }

        public int? CategoryId { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; }

        public int Rcdcount { get; set; }

    }

    public class ContactMappings : DataContractBase {
        public int LocationId { get; set; }
        public string LocationName { get; set; }        
        public string LvisContactid { get; set; }
        public bool IsActive { get; set; }
        public int ContactId { get; set; }
        public List<ServicePreference> ServicePreference { get; set; }

    }
    public class ContactProviderMappings : DataContractBase
    {
        public int ContactProviderMapId { get; set; }
        public int ProviderID { get; set; }
         
        public int CustomerId { get; set; }

        public string ProviderName { get; set; }

        public string Tenant { get; set; }

        public string CustomerName { get; set; }

        public string LocationName { get; set; }

        public LocationsMappings LocationDet { get; set; }

        public Contacts Contactdet { get; set; }

        public Provider Providerdet { get; set;}
        public int? ContactId { get; set; }

        public string LvisContactId { get; set; }

        public int? LocationId { get; set; }

        public int TenantId { get; set; }
        
    }

    public class ServicePreference : DataContractBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Boolean Ticked { get; set; }
    }

    
}
