using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    public class CustomerMappingService : Core.ServiceBase, ICustomerMappingService
    {
        public DC.LocationsMappings AddLocation(DC.LocationsMappings value, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddLocation(value, tenantId, userId);
        }

        public DC.CustomerMapping AddCustomer(DC.CustomerMapping value, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddCustomer(value, tenantId, userId);
        }

        public int DeleteLocation(int value, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteLocation(value, tenantId);
        }

        public string[] BulkImport(IList<DC.BulkImportDTO> value, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().BulkImportLocations(value, tenantId, userId);
        }

        public int DeleteCustomer(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteCustomer(value);
        }

        public List<DC.ApplicationMappingDTO> GetApplicationsList()
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetApplicationsList();
        }

        public IEnumerable<DC.LocationsMappings> GetLocations(string lVISAbeid, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetLocations(lVISAbeid, tenantId);
        }

        public List<DC.CategoryMapping> GetCategoryList(int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetCategoryList(tenantId);
        }

        public List<DC.CustomerMapping> GetLVISCustomers(int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetLVISCustomers(tenantId);
        }
        public string GetUserName(int cusotmerId, int applicationId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetUserName(cusotmerId, applicationId); 
        }
        public List<DC.CustomerMapping> GetLVISCustomersOnly(int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetLVISCustomersOnly(tenantId);
        }

        public int UpdateLocation(DC.LocationsMappings value, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().UpdateLocation(value, tenantId, userId);
        }

        public DC.CustomerMapping UpdateCustomer(DC.CustomerMapping value, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().UpdateCustomer(value, userId);
        }

        public IEnumerable<DC.LocationsMappings> GetLocations(int tenantId)
        {

            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetLocations(tenantId);
        }

        public IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceTypes()
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetCustomerPreferenceTypes();
        }

        public IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceSubTypes(int typeCodeId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetCustomerPreferenceSubTypes(typeCodeId);
        }
        public bool IsUniqueUserName(string  userName,int customerId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().IsUniqueUserName(userName,customerId);
        }
        public int ConfirmDeleteLocation(int value, int iTenantid)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().ConfirmDeleteLocation(value, iTenantid);
        }

        public int ConfirmDeleteCustomer(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().ConfirmDeleteCustomer(value);
        }

        public DC.ContactMappings AddContact(DC.ContactMappings value, int userId, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddContact(value, userId, tenantId);
        }

        public int UpdateContact(DC.ContactMappings value, int userId, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().UpdateContact(value, userId, tenantId);
        }

        public int DeleteContact(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteContact(value);
        }

        public int ConfirmDeleteContact(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().ConfirmDeleteContact(value);
        }

        public int DeleteContactProvider(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteContactProvider(value);
        }

        public int ConfirmDeleteContactProvider(int value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().ConfirmDeleteContactProvider(value);
        }

        public DC.ContactProviderMappings UpdateContactProvider(DC.ContactProviderMappings value, int userId, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().UpdateContactProvider(value, userId, tenantId);
        }

        public DC.ContactProviderMappings AddContactProvider(DC.ContactProviderMappings value, int userId, int tenantId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddContactProvider(value, userId, tenantId);
        }

        public List<DC.ServicePreference> GetServicePreferenceLocation(int locationid)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetServicePreferenceLocation(locationid);
        }

        public List<DC.ServicePreference> GetServicePreferenceContact(int contactid)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetServicePreferenceContact(contactid);
        }

        public List<DC.ServicePreference> GetServicePreferenceCustomer(int customerid)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetServicePreferenceCustomer(customerid);
        }

        public IEnumerable<DC.webhook> GetWebhooks(string Customer)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetWebhooks(Customer);
        
        }
        public int AddWebhook(DC.webhookDto value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddWebhook(value);

        }

        public string GetWebhookUser(int CustomerID)
        {
            return DataProviderFactory.Resolve <ICustomerMappingDataProvider>().GetWebhookUser(CustomerID);
        }

        public string[] GetAvailbleActionType(string User)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetAvailbleActionType(User);
        }
        public int EditWebhook(DC.webhookDto value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().EditWebhook(value);

        }
        public int DeleteWebhook(DC.webhookDto value)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteWebhook(value);

        }
        
        public IEnumerable<DC.WebhookDomain> GetWebhookDomains(int CustomerId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().GetWebhookDomains(CustomerId);
        
        }
        public DC.WebhookDomain AddWebhookDomain(DC.WebhookDomain webhookDomain, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().AddWebhookDomain(webhookDomain, userId);

        }

        public int UpdateWebhookDomain(DC.WebhookDomain webhookDomain, int userId)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().UpdateWebhookDomain(webhookDomain, userId);

        }

        public int DeleteWebhookDomain(DC.WebhookDomain webhookDomain)
        {
            return DataProviderFactory.Resolve<ICustomerMappingDataProvider>().DeleteWebhookDomain(webhookDomain);

        }
    }
}
