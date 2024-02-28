using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class Customer : DataContractBase
    {
        public string LvisCustomerID { get; set; }

        public string CustomerName { get; set; }

        public ConnectorType Connector { get; set; }

        
    }
    public class WebhookDomain: DataContractBase
    {
        public int WebhookDomainId { get; set; }
        public int CustomerId { get; set; }
        public string Domain { get; set; }

    }

    public class webhook : DataContractBase
    {
        public string UserID { get; set; }
        public string URL { get; set; }
        public string Secret { get; set; }
        public string ActionType { get; set; }
        public int MaxAttempts { get; set; }
        public string Active { get; set; }
        public string X_API_ID{ get; set; }


    }

    public class webhookDto : DataContractBase
    {
        public webhook webhook { get; set; }
        public string Id { get; set; }
        public string WebHookUri { get; set; }
        public string Secret { get; set; }
        public string User { get; set; }
        public string OriginalUserID { get; set; }
        public string OriginalActionType{get;set;}

        public string X_API_ID { get; set; }

    }

    public class ProtectedDataWebhook 
    {
        public string Id { get; set; }
        public string WebHookUri { get; set; }
        public string Secret { get; set; }
        public string Description { get; set; }
        public bool IsPaused { get; set; }
        public string[] Filters { get; set; }
        public Dictionary<string, object> Headers { get; set; }= new Dictionary<string, object>();
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }

    public class CustomerDetails : DataContractBase
    {
        public int LvisCustomerID { get; set; }

        public string CustomerName { get; set; }       


    }

    public class Regions : DataContractBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Application { get; set; }

        public Boolean Ticked { get; set; }

    }

    public class FastOffices : DataContractBase
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class Contacts :DataContractBase
    {
        public int ContactId { get; set; }
        public string LvisContactid { get; set; }
        public bool IsActive { get; set; }

    }

    public class Users : DataContractBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ExceptionStatus : DataContractBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Service : DataContractBase
    {
        public int  ID { get; set; }
        public string Name { get; set; }
    }

    public class Status : DataContractBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public enum ConnectorType : int
    {
        RealEC = 0,
        eLynx = 1,
        FAST
    }
}
