using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface ISubscriptionService : Core.IServiceBase
    {
        IEnumerable<DC.Subscription> GetSubscriptionsByCustomer(int customerId, int tenantId, int applicationId);

        IEnumerable<DC.Subscription> GetSubscriptionsByCategory(int categoryId, int tenantId, int applicationId);

        DC.Subscription AddSubscription(DC.Subscription Subscription, int tenantId, int iEmployeeid);

        DC.Subscription UpdateSubscription(DC.Subscription Subscription, int tenantId, int iEmployeeid);
        IEnumerable<DC.MessageType> GetApplicationMessageList(int ApplicationId, int TenantId,int SubscriptionId);

        IEnumerable<DC.MessageType> GetMessageType(int MEssageTypeId);

        IEnumerable<DC.ApplicationMappingDTO> GetApplicationByTenant(int TenantId);

        int DeleteSubscription(int value, int iTenantid);

        int ConfirmDeleteSubscription(int value, int iTenantid);
    }
}
