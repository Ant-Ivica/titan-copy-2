using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Services
{
    public class SubscriptionService : Core.ServiceBase, ISubscriptionService
    {
        public Subscription AddSubscription(Subscription Subscription, int tenantId, int iEmployeeid)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.AddSubscription(Subscription, tenantId, iEmployeeid);
        }

        public IEnumerable<ApplicationMappingDTO> GetApplicationByTenant(int TenantId)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.GetApplicationByTenant(TenantId);
        }

        public IEnumerable<MessageType> GetApplicationMessageList(int ApplicationId, int TenantId,int SubscriptionId)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.GetApplicationMessageList(ApplicationId, TenantId, SubscriptionId);
        }

        public IEnumerable<MessageType> GetMessageType(int MEssageTypeId)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.GetMessageType(MEssageTypeId);
        }

        public IEnumerable<Subscription> GetSubscriptionsByCategory(int categoryId, int tenantId, int applicationId)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.GetSubscriptionsByCategory(categoryId, tenantId, applicationId);
        }

        public IEnumerable<Subscription> GetSubscriptionsByCustomer(int customerId, int tenantId, int applicationId)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.GetSubscriptionsByCustomer(customerId, tenantId, applicationId);
        }

        public Subscription UpdateSubscription(Subscription Subscription, int tenantId, int iEmployeeid)
        {
            ISubscriptionDataProvider eventProvider = DataProviderFactory.Resolve<ISubscriptionDataProvider>();
            return eventProvider.UpdateSubscription(Subscription, tenantId, iEmployeeid);
        }

        public int DeleteSubscription(int value, int tenantId)
        {
            return DataProviderFactory.Resolve<ISubscriptionDataProvider>().DeleteSubscription(value, tenantId);
        }

        public int ConfirmDeleteSubscription(int value, int iTenantid)
        {
            return DataProviderFactory.Resolve<ISubscriptionDataProvider>().ConfirmDeleteSubscription(value, iTenantid);
        }
    }
}
