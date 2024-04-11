using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public  interface ICustomerMappingService : Core.IServiceBase
    {
        IEnumerable<DC.webhook> GetWebhooks(string Customer);
        int AddWebhook(DC.webhookDto value);
        IEnumerable<DC.LocationsMappings> GetLocations(string customerId, int tenantId);

        DC.LocationsMappings AddLocation(DC.LocationsMappings value, int tenantId, int userId);

        string[] BulkImport(IList<DC.BulkImportDTO> value, int tenantId, int userId);

        int UpdateLocation(DC.LocationsMappings value, int tenantId, int userId);

        int DeleteLocation(int value,int iTenantid);

        int ConfirmDeleteLocation(int value, int iTenantid);

        //Customer Operations
        List<DC.CustomerMapping> GetLVISCustomers(int tenantId);

        string GetUserName(int cusotmerId, int applicationId);

        List<DC.CustomerMapping> GetLVISCustomersOnly(int tenantId);

        DC.CustomerMapping AddCustomer(DC.CustomerMapping value, int tenantId, int userId);

        DC.CustomerMapping UpdateCustomer(DC.CustomerMapping value, int userId);

        int DeleteCustomer(int value);

        int ConfirmDeleteCustomer(int value);

        List<DC.ApplicationMappingDTO> GetApplicationsList();

        List<DC.CategoryMapping> GetCategoryList(int tenantId);
        IEnumerable<DC.LocationsMappings> GetLocations(int tenantId);
        IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceTypes();
        IEnumerable<DC.TypeCodeDTO> GetCustomerPreferenceSubTypes(int typeCodeId);

        DC.ContactMappings AddContact(DC.ContactMappings value, int userId, int tenantId);
        
        int UpdateContact(DC.ContactMappings value, int userId, int tenantId);
        int DeleteContact(int value);
        int ConfirmDeleteContact(int value);
        List<DC.ServicePreference> GetServicePreferenceLocation(int locationid);

        List<DC.ServicePreference> GetServicePreferenceContact(int contactid);

        List<DC.ServicePreference> GetServicePreferenceCustomer(int customerid);

        DC.ContactProviderMappings AddContactProvider(DC.ContactProviderMappings value,int userId, int tenantId);

        int DeleteContactProvider(int value);

        int ConfirmDeleteContactProvider(int value);
        int EditWebhook(DC.webhookDto value);
        DC.ContactProviderMappings UpdateContactProvider(DC.ContactProviderMappings value, int userId, int tenantId);

        bool IsUniqueUserName(string userName,int customerId);
        string GetWebhookUser(int customerID);
        string[] GetAvailbleActionType(string User);
        int DeleteWebhook(DC.webhookDto value);
        IEnumerable<DC.WebhookDomain> GetWebhookDomains(int CustomerId);
        DC.WebhookDomain AddWebhookDomain(DC.WebhookDomain webhookDomain, int userId);
        int DeleteWebhookDomain(DC.WebhookDomain webhookDomain);
        int UpdateWebhookDomain(DC.WebhookDomain webhookDomain, int userId);

    }
}
