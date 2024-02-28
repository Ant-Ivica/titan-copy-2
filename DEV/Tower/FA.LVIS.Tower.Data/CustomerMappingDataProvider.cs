using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using LVIS.Common;
using System.Net.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;


namespace FA.LVIS.Tower.Data
{
    public class CustomerMappingDataProvider : Core.DataProviderBase, ICustomerMappingDataProvider
    {


        IUtils Utils = new Utils();
        public LocationsMappings AddLocation(LocationsMappings value, int tenantId, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Location LocationtoAdd = new TerminalDBEntities.Location();

                LocationtoAdd.ExternalId = value.ExternalId;
                LocationtoAdd.LocationName = value.LocationName;
                LocationtoAdd.CustomerId = value.CustomerId.ToIntDefEx();
                LocationtoAdd.TenantId = tenantId;
                LocationtoAdd.CreatedDate = DateTime.Now;
                LocationtoAdd.LastModifiedDate = DateTime.Now;
                LocationtoAdd.CreatedById = userId;
                LocationtoAdd.Notes = value.Notes;

                dbContext.Locations.Add(LocationtoAdd);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                    value.LocationId = LocationtoAdd.LocationId;
                if (value.ServicePreference != null && value.ServicePreference.Count() > 0)
                {

                    foreach (var item in value.ServicePreference)
                    {

                        TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                        addservicepreference.CustomerId = Convert.ToInt32(value.CustomerId);
                        addservicepreference.LocationId = value.LocationId;
                        addservicepreference.ServiceId = item.ID;
                        addservicepreference.ContactId = null;
                        addservicepreference.TenantId = tenantId;
                        addservicepreference.CreatedDate = DateTime.Now;
                        addservicepreference.CreatedById = userId;
                        addservicepreference.LastModifiedDate = DateTime.Now;
                        addservicepreference.LastModifiedById = userId;
                        dbContext.ServicePreferenceMaps.Add(addservicepreference);
                    }
                    AuditLogHelper.SaveChanges(dbContext);
                }
            }

            return value;
        }

        public int DeleteLocation(int value, int tenantId)
        {
            if (value > 0)
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "Location").FirstOrDefault();

                    if (result != null)
                    {
                        return 0;
                    }

                    else
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        public IEnumerable<LocationsMappings> GetLocations(string customerId, int tenantId)
        {
            Entities dbContext = new Entities();

            var locations = dbContext.Locations.Where(se => se.CustomerId.ToString() == customerId)
                .Select(sm => new DataContracts.LocationsMappings()
                {
                    LocationId = sm.LocationId,
                    ExternalId = sm.ExternalId,
                    LocationName = sm.LocationName,
                    CustomerId = customerId,
                    CustomerName = sm.Customer.CustomerName,
                    TenantId = sm.TenantId,
                    Tenant = sm.Tenant.TenantName,
                    Notes = sm.Notes
                }).ToList();

            if (locations.Count() > 0 && tenantId != (int)TenantIdEnum.LVIS)
            {
                locations = locations
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return locations;
        }

        public string[] BulkImportLocations(IList<BulkImportDTO> Value, int tenantId, int userId)
        {
            //IList<BulkImportDTO> list = null;
            List<string> output = new List<string>();
            int UniqueEntryCount = 0;
            int UpdateEntryCount = 0;

            using (Entities dbContext = new Entities())
            {

                //using (DbContextTransaction dbTransaction = dbContext.Database.BeginTransaction())
                //{
                try
                {

                    foreach (var value in Value)
                    {
                        TerminalDBEntities.Location LocationtoAdd = new TerminalDBEntities.Location();
                        TerminalDBEntities.Location LocationToUpdate = new TerminalDBEntities.Location();
                        TerminalDBEntities.FASTGABMap FastGabAdd = new TerminalDBEntities.FASTGABMap();
                        TerminalDBEntities.FASTGABMap FastGabUpdate = null;
                        bool locationInsertSuccess = false;
                        bool locationUpdateSuccess = false;
                        int? loanTypecount = null;

                        //Start Transaction for Location Table
                        if (value.ExternalId != "" && value.Name.Trim() != "")
                        {
                            LocationToUpdate = (from branch in dbContext.Locations
                                                where branch.TenantId == tenantId
                                                && branch.ExternalId == value.ExternalId
                                                && branch.LocationName.Trim().ToUpper() == value.Name.Trim().ToUpper()
                                                select branch).FirstOrDefault();
                            if (LocationToUpdate != null)
                            {
                                LocationToUpdate.CustomerId = value.CustomerId.ToIntDefEx();
                                LocationToUpdate.ExternalId = value.ExternalId;
                                LocationToUpdate.LocationName = value.Name;
                                LocationToUpdate.LastModifiedDate = DateTime.Now;
                                LocationToUpdate.LastModifiedById = userId;
                                LocationToUpdate.Notes = value.Notes;
                                dbContext.Entry(LocationToUpdate).State = System.Data.Entity.EntityState.Modified;
                                int Success = AuditLogHelper.SaveChanges(dbContext);
                                locationUpdateSuccess = true;
                            }
                            else
                            {
                                LocationtoAdd.ExternalId = value.ExternalId;
                                LocationtoAdd.LocationName = value.Name;
                                LocationtoAdd.CustomerId = value.CustomerId.ToIntDefEx();
                                LocationtoAdd.TenantId = tenantId;
                                LocationtoAdd.CreatedDate = DateTime.Now;
                                LocationtoAdd.LastModifiedDate = DateTime.Now;
                                LocationtoAdd.CreatedById = userId;
                                LocationtoAdd.LastModifiedById = userId;
                                LocationtoAdd.Notes = value.Notes;

                                dbContext.Locations.Add(LocationtoAdd);
                                int Success = AuditLogHelper.SaveChanges(dbContext);
                                locationInsertSuccess = true;
                            }
                        }

                        if (value.LoanType != null && value.LoanType != "")
                        {
                            loanTypecount = (from typeCode in dbContext.TypeCodes
                                             where typeCode.TypeCodeDesc == value.LoanType.Trim()
                                             select typeCode).Count();
                        }

                        //Start Transaction for FastGabMap Table
                        if (((LocationToUpdate != null && LocationToUpdate.LocationId > 0) || LocationtoAdd.LocationId > 0) &&
                              value.RegionID > 0 && (loanTypecount > 0 || loanTypecount == null))
                        {
                            if (LocationToUpdate != null)
                            {
                                FastGabUpdate = (from fastgab in dbContext.FASTGABMaps
                                                 where fastgab.LocationId == LocationToUpdate.LocationId
                                                 && fastgab.RegionId == value.RegionID
                                                 select fastgab).FirstOrDefault();
                            }

                            if (FastGabUpdate != null)
                            {
                                FastGabUpdate.LocationId = LocationToUpdate.LocationId;
                                FastGabUpdate.NewLenderABEID = value.NewLenderABEID;
                                FastGabUpdate.BusinessSourceABEID = value.BusinessSourceABEID;
                                FastGabUpdate.FASTGABMapDesc = value.Description;
                                FastGabUpdate.RegionId = value.RegionID;
                                ////FastGabUpdate.LoanTypeCodeId = !String.IsNullOrEmpty(value.LoanType) ? dbContext.TypeCodes.Where(se => se.TypeCodeDesc == value.LoanType.Trim()).Select(sel => sel.TypeCodeId).FirstOrDefault() : (int?)null;
                                FastGabUpdate.LoanTypeCodeId = !String.IsNullOrEmpty(value.LoanType) ?
                                                               dbContext.TypeCodes.Where(se => se.TypeCodeDesc == value.LoanType.Trim()).Select(sel => sel.TypeCodeId).FirstOrDefault() : (int?)null;
                                FastGabUpdate.LastModifiedDate = DateTime.Today;
                                FastGabUpdate.CreatedDate = DateTime.Today;
                                FastGabUpdate.LastModifiedById = userId;
                                dbContext.Entry(FastGabUpdate).State = System.Data.Entity.EntityState.Modified;
                                int Success = AuditLogHelper.SaveChanges(dbContext);
                                UpdateEntryCount++;
                            }
                            else
                            {
                                FastGabAdd.LocationId = LocationtoAdd.LocationId;
                                FastGabAdd.NewLenderABEID = value.NewLenderABEID;
                                FastGabAdd.BusinessSourceABEID = value.BusinessSourceABEID;
                                FastGabAdd.FASTGABMapDesc = value.Description;
                                FastGabAdd.RegionId = value.RegionID;
                                FastGabAdd.LoanTypeCodeId = !String.IsNullOrEmpty(value.LoanType) ?
                                                            dbContext.TypeCodes.Where(se => se.TypeCodeDesc == value.LoanType.Trim()).Select(sel => sel.TypeCodeId).FirstOrDefault() : (int?)null;
                                FastGabAdd.CreatedDate = DateTime.Today;
                                FastGabAdd.LastModifiedDate = DateTime.Today;
                                FastGabAdd.CreatedById = userId;

                                dbContext.FASTGABMaps.Add(FastGabAdd);
                                int Success = AuditLogHelper.SaveChanges(dbContext);
                                UniqueEntryCount++;
                            }
                        }
                        else
                        {
                            using (var dbContext1 = new Entities())
                            {
                                IQueryable<TerminalDBEntities.Location> LocationDelete = null;

                                if (locationInsertSuccess)
                                {
                                    LocationDelete = (from branch in dbContext1.Locations
                                                      where branch.LocationId == LocationtoAdd.LocationId
                                                      select branch);
                                }
                                else if (locationUpdateSuccess)
                                {
                                    LocationDelete = (from branch in dbContext1.Locations
                                                      where branch.LocationId == LocationToUpdate.LocationId
                                                      select branch);
                                }

                                if (LocationDelete != null)
                                {
                                    dbContext1.Locations.RemoveRange(LocationDelete);
                                    AuditLogHelper.SaveChanges(dbContext1);
                                }
                            }

                            continue;
                        }
                    }
                }

                catch (System.Exception ex)
                {
                    ex.ToString();
                    output.Add("New Records: " + UniqueEntryCount);
                    output.Add("Updated Records: " + UpdateEntryCount);
                    output.Add("Records Failed: " + (Value.Count() - (UniqueEntryCount + UpdateEntryCount)));
                    return output.ToArray();
                }

                //}
            }

            output.Add("New Records: " + UniqueEntryCount);
            output.Add("Updated Records: " + UpdateEntryCount);
            if ((Value.Count() - (UniqueEntryCount + UpdateEntryCount)) > 0)
                output.Add("Records Failed: " + (Value.Count() - (UniqueEntryCount + UpdateEntryCount)));
            return output.ToArray();
        }



        public List<CustomerMapping> GetLVISCustomers(int tenantId)
        {
            //getting lender Data from PID MAPPING Table
            Entities dbContext = new Entities();
            List<DataContracts.CustomerMapping> CustomerMapping = new List<DataContracts.CustomerMapping>();

            if (dbContext.Customers.Count() > 0)
            {
                var Customers = dbContext.Customers.AsEnumerable().Where(se => se.TenantId == tenantId);
                if (tenantId == (int)TenantIdEnum.LVIS)
                    Customers = dbContext.Customers.AsEnumerable();

                CustomerMapping = Customers.Select(cust =>
                 new CustomerMapping()
                 {
                     CustomerUserId = cust?.Credentials.FirstOrDefault(x => x.CustomerId == cust.CustomerId)?.Username,
                     LVISCustomerID = cust.CustomerId,
                     Category = cust.CategoryId != null ? cust.Category.CategoryName : string.Empty,
                     CustomerName = cust.CustomerName,
                     CategoryId = cust.CategoryId,
                     Applicationid = cust.ApplicationId,
                     ApplicationName = cust.ApplicationId == 0 ? string.Empty : dbContext.Applications.Where(se => se.ApplicationId == cust.ApplicationId).Select(sel => sel.ApplicationName).FirstOrDefault(),
                     TenantId = cust.TenantId,
                     Tenant = cust.TenantId == 0 ? string.Empty : cust.Tenant.TenantName,
                     CAPIClientID = cust.CustomerInfoes != null ? cust.CustomerInfoes.Where(ci => ci.TypeCodeId == 801 && ci.Value != null).Select(c => c.Value).FirstOrDefault() : "0",
                     ServiceSelect = cust.CustomerInfoes != null ? cust.CustomerInfoes.Where(ci => ci.TypeCodeId == 826 || ci.TypeCodeId == 827 || ci.TypeCodeId == 828 && ci.Value != null).Select(c => c.Value).FirstOrDefault() : "",
                     DTDeliveryUrl = cust.CustomerInfoes != null ? cust.CustomerInfoes.Where(ci => ci.TypeCodeId == 829 && ci.Value != null).Select(c => c.Value).FirstOrDefault() : "",
                     DTCredentials = cust.CustomerInfoes != null ? cust.CustomerInfoes.Where(ci => ci.TypeCodeId == 830 && ci.Value != null).Select(c => c.Value).FirstOrDefault() : "",
                     CustomerPreference = cust.CustomerInfoes != null ? cust.CustomerInfoes.Select(se => new TypeCodeDTO()
                     {
                         TypeCodeId = se.TypeCodeId.Value,
                         TypeCodeDesc = se.TypeCodeId == 0 ? string.Empty : dbContext.TypeCodes.Where(tc => tc.TypeCodeId == se.TypeCodeId).Select(sel => sel.TypeCodeDesc).FirstOrDefault()
                     }).ToList() : null,
                     ServicePreference = cust.ServicePreferenceMaps != null ? cust.ServicePreferenceMaps.Where(ser => ser.CustomerId == cust.CustomerId && ser.LocationId == null && ser.ContactId == null).Select(se => new ServicePreference()
                     {
                         ID = se.ServiceId,
                         Name = se.ServiceId == 0 ? string.Empty : dbContext.Services.Where(ser => ser.ServiceId == se.ServiceId).Select(sel => sel.ServiceName).FirstOrDefault(),
                         Ticked = true
                     }).ToList() : null,
                 }).ToList();

            }

            return CustomerMapping;
        }

        public string GetUserName(int cusotmerId, int applicationId)
        {
            string userName = string.Empty;
            using (Entities dBEntities = new Entities())
            {
                TerminalDBEntities.Customer cust = dBEntities.Customers.FirstOrDefault(x => x.CustomerId == cusotmerId);
                if (cust != null)
                    userName = cust?.Credentials.FirstOrDefault(x => x.CustomerId == cust.CustomerId)?.Username;
                if (string.IsNullOrEmpty(userName))
                    while (true)
                    {
                        string appChars = dBEntities.Applications.FirstOrDefault(x => x.ApplicationId == applicationId).ApplicationName.Substring(0, 2).ToUpper();
                        string randomDigits_4 = new RandomDigitGenerator().Generate(4);
                        userName = $"LV{appChars}{randomDigits_4}";
                        if (!dBEntities.Credentials.Any(x => x.Username == userName))
                            return userName;
                    }
                return userName;
            }
        }

        public List<CustomerMapping> GetLVISCustomersOnly(int tenantId)
        {
            //getting lender Data from PID MAPPING Table
            Entities dbContext = new Entities();
            List<DataContracts.CustomerMapping> CustomerMapping = new List<DataContracts.CustomerMapping>();

            if (dbContext.Customers.Count() > 0)
            {
                CustomerMapping = dbContext.Customers.AsEnumerable().Select(cust =>
                 new CustomerMapping()
                 {
                     LVISCustomerID = cust.CustomerId,
                     Category = cust.CategoryId != null ? cust.Category.CategoryName : string.Empty,
                     CustomerName = cust.CustomerName,
                     CategoryId = cust.CategoryId,
                     Applicationid = cust.ApplicationId,
                     TenantId = cust.TenantId,
                     Tenant = cust.TenantId == 0 ? string.Empty : cust.Tenant.TenantName,
                 }).ToList();

            }

            if (CustomerMapping.Count() > 0 && tenantId != (int)TenantIdEnum.LVIS)
            {
                CustomerMapping = CustomerMapping
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return CustomerMapping;
        }

        public int UpdateLocation(LocationsMappings value, int tenantId, int userId)
        {
            using (var dbContext = new Entities())
            {
                var LocationToUpdate = (from branch in dbContext.Locations
                                        where branch.LocationId == value.LocationId
                                        select branch).FirstOrDefault();

                if (LocationToUpdate != null)
                {
                    LocationToUpdate.ExternalId = value.ExternalId;
                    LocationToUpdate.LocationName = value.LocationName;
                    LocationToUpdate.LastModifiedDate = DateTime.Now;
                    LocationToUpdate.LastModifiedById = userId;
                    LocationToUpdate.Notes = value.Notes;

                    dbContext.Entry(LocationToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        IEnumerable<ServicePreferenceMap> ServicePreference = dbContext.ServicePreferenceMaps
                         .RemoveRange(dbContext.ServicePreferenceMaps
                         .Where(se => (se.LocationId == value.LocationId && se.ContactId == null)));

                        AuditLogHelper.SaveChanges(dbContext);

                        if (value.ServicePreference != null && value.ServicePreference.Count() > 0)
                        {

                            foreach (var item in value.ServicePreference)
                            {

                                TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                                addservicepreference.CustomerId = Convert.ToInt32(value.CustomerId);
                                addservicepreference.LocationId = value.LocationId;
                                addservicepreference.ServiceId = item.ID;
                                addservicepreference.ContactId = null;
                                addservicepreference.TenantId = tenantId;
                                addservicepreference.CreatedDate = DateTime.Now;
                                addservicepreference.CreatedById = userId;
                                addservicepreference.LastModifiedDate = DateTime.Now;
                                addservicepreference.LastModifiedById = userId;
                                dbContext.ServicePreferenceMaps.Add(addservicepreference);
                            }
                            AuditLogHelper.SaveChanges(dbContext);
                        }

                        return LocationToUpdate.LocationId;
                    }

                    else
                        return 0;
                }
            }

            return 0;
        }

        public CustomerMapping AddCustomer(CustomerMapping customer, int tenantId, int userId)
        {
            Entities dbcontext = new Entities();
            TerminalDBEntities.Customer addcustomer = new TerminalDBEntities.Customer();

            addcustomer.CustomerName = customer.CustomerName;
            addcustomer.CategoryId = customer.CategoryId;
            addcustomer.ApplicationId = customer.Applicationid;
            addcustomer.TenantId = tenantId;
            addcustomer.CreatedDate = DateTime.Now;
            addcustomer.LastModifiedDate = DateTime.Now;
            addcustomer.CreatedById = userId;
            addcustomer.LastModifiedById = userId;

            dbcontext.Customers.Add(addcustomer);
            int Success = AuditLogHelper.SaveChanges(dbcontext);

            if (Success == 1)
            {
                customer.LVISCustomerID = addcustomer.CustomerId;

                if (customer.CustomerPreference != null && customer.CustomerPreference.Count() > 0)
                {
                    foreach (var item in customer.CustomerPreference)
                    {
                        TerminalDBEntities.CustomerInfo addcustomerinfo = new TerminalDBEntities.CustomerInfo();
                        addcustomerinfo.CustomerId = customer.LVISCustomerID;
                        addcustomerinfo.TypeCodeId = item.TypeCodeId;
                        if (item.TypeCodeId == 801)
                        {
                            addcustomerinfo.Value = customer.CAPIClientID;
                        }
                        else if (item.TypeCodeId == 826 || item.TypeCodeId == 827 || item.TypeCodeId == 828)
                        {
                            addcustomerinfo.Value = customer.ServiceSelect;
                        }
                        else if (item.TypeCodeId == 829)
                        {
                            addcustomerinfo.Value = customer.DTDeliveryUrl;
                        }
                        else if (item.TypeCodeId == 830)
                        {
                            addcustomerinfo.Value = customer.DTCredentials;
                        }
                        else
                        {
                            addcustomerinfo.Value = item.TypeCodeDesc;
                        }
                        //addcustomerinfo.Value = item.TypeCodeId == 801 ? customer.CAPIClientID : item.TypeCodeDesc;
                        addcustomerinfo.TenantId = tenantId;
                        addcustomerinfo.CreatedDate = DateTime.Now;
                        addcustomerinfo.LastModifiedDate = DateTime.Now;
                        addcustomerinfo.CreatedById = userId;
                        addcustomerinfo.LastModifiedById = userId;
                        dbcontext.CustomerInfoes.Add(addcustomerinfo);

                    }

                    AddUpdateCredential(customer, userId);
                    AuditLogHelper.SaveChanges(dbcontext);
                }
            }

            if (customer.ServicePreference != null && customer.ServicePreference.Count() > 0 && customer.Applicationid == (int)ApplicationEnum.SettlementServices)
            {

                foreach (var item in customer.ServicePreference)
                {

                    TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                    addservicepreference.CustomerId = customer.LVISCustomerID;
                    addservicepreference.LocationId = null;
                    addservicepreference.ServiceId = item.ID;
                    addservicepreference.ContactId = null;
                    addservicepreference.TenantId = tenantId;
                    addservicepreference.CreatedDate = DateTime.Now;
                    addservicepreference.CreatedById = userId;
                    addservicepreference.LastModifiedDate = DateTime.Now;
                    addservicepreference.LastModifiedById = userId;
                    dbcontext.ServicePreferenceMaps.Add(addservicepreference);
                }
                AuditLogHelper.SaveChanges(dbcontext);
            }

            customer.LVISCustomerID = addcustomer.CustomerId;
            customer.CustomerName = dbcontext.Customers.Where(v => v.CustomerId == customer.LVISCustomerID)
            .Select(v => v.CustomerName).FirstOrDefault();
            customer.Category = dbcontext.Categories.Where(v => v.CategoryId == customer.CategoryId)
            .Select(v => v.CategoryName).FirstOrDefault();
            customer.ApplicationName = dbcontext.Applications.Where(se => se.ApplicationId == customer.Applicationid).Select(sel => sel.ApplicationName).FirstOrDefault();
            customer.TenantId = tenantId;

            return customer;
        }

        private string getNewPassword()
        {
            PwdGenerator passwordGenerator = new PwdGenerator();
            // Populate the Generator Config from the application configuration
            PwdGenerator.GeneratorConfig generatorConfig = new PwdGenerator.GeneratorConfig()// using defalut configuration
            {
                SpeSyb = 2
            };
            return passwordGenerator.Generate(generatorConfig);
        }
        public CustomerMapping UpdateCustomer(CustomerMapping customer, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                var updatecustomer = (from customers in dbContext.Customers
                                      where customers.CustomerId == customer.LVISCustomerID
                                      select customers).FirstOrDefault();
                if (updatecustomer != null)
                {
                    updatecustomer.LastModifiedDate = DateTime.Now;
                    updatecustomer.LastModifiedById = userId;
                    updatecustomer.CustomerName = customer.CustomerName;
                    updatecustomer.CategoryId = customer.CategoryId;
                    updatecustomer.ApplicationId = customer.Applicationid;
                    dbContext.Entry(updatecustomer).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        IEnumerable<CustomerInfo> BusinessProgramTypes = dbContext.CustomerInfoes
                        .RemoveRange(dbContext.CustomerInfoes
                        .Where(se => (se.CustomerId == customer.LVISCustomerID)));

                        AuditLogHelper.SaveChanges(dbContext);

                        if (customer.CustomerPreference != null && customer.CustomerPreference.Count() > 0)
                        {

                            foreach (var item in customer.CustomerPreference)
                            {

                                TerminalDBEntities.CustomerInfo addcustomerinfo = new TerminalDBEntities.CustomerInfo();
                                addcustomerinfo.CustomerId = customer.LVISCustomerID;
                                addcustomerinfo.TypeCodeId = item.TypeCodeId;
                                if (item.TypeCodeId == 801)
                                {
                                    addcustomerinfo.Value = customer.CAPIClientID;
                                }
                                else if (item.TypeCodeId == 826 || item.TypeCodeId == 827 || item.TypeCodeId == 828)
                                {
                                    addcustomerinfo.Value = customer.ServiceSelect;
                                }
                                else if (item.TypeCodeId == 829)
                                {
                                    addcustomerinfo.Value = customer.DTDeliveryUrl;
                                }
                                else if (item.TypeCodeId == 830)
                                {
                                    addcustomerinfo.Value = customer.DTCredentials;
                                }
                                else
                                {
                                    addcustomerinfo.Value = item.TypeCodeDesc;
                                }
                                ////addcustomerinfo.Value = item.TypeCodeId == 801 ? customer.CAPIClientID : item.TypeCodeDesc;
                                addcustomerinfo.TenantId = customer.TenantId;
                                addcustomerinfo.CreatedDate = DateTime.Now;
                                addcustomerinfo.LastModifiedDate = DateTime.Now;
                                addcustomerinfo.CreatedById = userId;
                                addcustomerinfo.LastModifiedById = userId;
                                dbContext.CustomerInfoes.Add(addcustomerinfo);
                            }
                            AuditLogHelper.SaveChanges(dbContext);
                        }

                        var ServicePreferenceExistingList = dbContext.ServicePreferenceMaps.Where(ser => ser.CustomerId == customer.LVISCustomerID
                                                                                                    && ser.LocationId == null && ser.ContactId == null);

                        if ((customer.ServicePreference != null && customer.ServicePreference.Count() > 0) ||
                                (ServicePreferenceExistingList != null && ServicePreferenceExistingList.Count() > 0))
                        {
                            IEnumerable<ServicePreferenceMap> ServicePreference = dbContext.ServicePreferenceMaps
                           .RemoveRange(dbContext.ServicePreferenceMaps
                           .Where(se => (se.CustomerId == customer.LVISCustomerID && se.LocationId == null && se.ContactId == null)));

                            AuditLogHelper.SaveChanges(dbContext);

                            if (customer.Applicationid == (int)ApplicationEnum.SettlementServices)
                            {
                                foreach (var item in customer.ServicePreference)
                                {

                                    TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                                    addservicepreference.CustomerId = customer.LVISCustomerID;
                                    addservicepreference.LocationId = null;
                                    addservicepreference.ServiceId = item.ID;
                                    addservicepreference.ContactId = null;
                                    addservicepreference.TenantId = customer.TenantId;
                                    addservicepreference.CreatedById = userId;
                                    addservicepreference.CreatedDate = DateTime.Now;
                                    addservicepreference.LastModifiedDate = DateTime.Now;
                                    addservicepreference.LastModifiedById = userId;
                                    dbContext.ServicePreferenceMaps.Add(addservicepreference);

                                }
                                AuditLogHelper.SaveChanges(dbContext);
                            }
                        }
                        customer.LVISCustomerID = updatecustomer.CustomerId;
                        customer.CustomerName = dbContext.Customers.Where(v => v.CustomerId == customer.LVISCustomerID)
                        .Select(v => v.CustomerName).FirstOrDefault();
                        customer.Category = dbContext.Categories.Where(v => v.CategoryId == customer.CategoryId)
                        .Select(v => v.CategoryName).FirstOrDefault();
                        customer.ApplicationName = dbContext.Applications.Where(se => se.ApplicationId == customer.Applicationid).Select(sel => sel.ApplicationName).FirstOrDefault();
                        AddUpdateCredential(customer, userId);
                    }

                    return customer;
                }

            }
            return customer;
        }
        private void AddUpdateCredential(CustomerMapping customer, int userId)
        {
            if (customer.GenerateCredential)
            {
                string pwd = getNewPassword();
                using (Entities ctx = new Entities())
                {
                    var credential = ctx.Credentials.Where(x => x.CustomerId == customer.LVISCustomerID).FirstOrDefault();
                    if (credential == null)
                    {
                        credential = new Credential()
                        {
                            Username = customer.CustomerUserId,
                            Password = PwdGenerator.GetLVISHash(pwd),
                            LastModifiedDate = DateTime.Now,
                            LastModifiedById = userId,
                            CreatedById = userId,
                            CreatedDate = DateTime.Now,
                            CustomerId = customer.LVISCustomerID,
                            LastAuthenticatedDate = DateTime.Now
                        };


                        ctx.Credentials.Add(credential);
                        ctx.SaveChanges();
                        EndpointTypeCode endpointTypeCode = GetEndPointId((ApplicationEnum)customer.Applicationid);
                        if (endpointTypeCode != EndpointTypeCode.NA)
                        {
                            ctx.CredentialEndPoints.Add(
                                new CredentialEndPoint()
                                {
                                    CredentialId = credential.CredentialId,
                                    CreatedById = userId,
                                    CreatedDate = DateTime.Now,
                                    LastModifiedById = userId,
                                    LastModifiedDate = DateTime.Now,
                                    TypeCodeId = (int)endpointTypeCode

                                });
                            ctx.SaveChanges();
                        }

                    }
                    else
                    {
                        credential.Username = customer.CustomerUserId;
                        credential.Password = PwdGenerator.GetLVISHash(pwd);
                        credential.LastModifiedById = userId;
                        credential.LastModifiedDate = DateTime.Now;
                        credential.CustomerId = customer.LVISCustomerID;
                        credential.LastAuthenticatedDate = DateTime.Now;
                        ctx.SaveChanges();
                    }
                    var user = ctx.Tower_Users.FirstOrDefault(x => x.UserId == userId);
                    sentMail(user.Email, user.Name, pwd, customer.CustomerName, customer.ApplicationName);

                }
            }
        }
        private EndpointTypeCode GetEndPointId(ApplicationEnum appId)
        {
            Dictionary<ApplicationEnum, EndpointTypeCode> mapping = new Dictionary<ApplicationEnum, EndpointTypeCode>() {
                {ApplicationEnum.OpenAPI,EndpointTypeCode.openapi },
                { ApplicationEnum.SettlementServices,EndpointTypeCode.settlementservices},
                {ApplicationEnum.CAPI ,EndpointTypeCode.calculator }
            };
            if (mapping.ContainsKey(appId))
                return mapping[appId];
            return EndpointTypeCode.NA;
        }
        private void sentMail(string emailTo, string userName, string pwd, string cusotmerName, string applicationName)
        {
            try
            {
                string body = string.Format("&nbsp;  Hello {0} <br/><br/>&nbsp;        Here is credential for {1}<br/>&nbsp;                 Password : {2}<br/><br/>&nbsp;  Thank You<br/>&nbsp;  LVIS Team",
                                                    userName, cusotmerName, pwd);
                string emailFrom = Utils.GetConfig("EmailFrom");
                string subject = string.Format("FA-Secure Add/Edit {0} Customer", applicationName);//Utils.GetConfig("EmailSubject");
                if (string.IsNullOrEmpty(emailFrom) || string.IsNullOrEmpty(emailTo))
                    throw new System.Exception("Sender and Recipient is mandatory");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailFrom);
                foreach (var address in emailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mailMessage.To.Add(address);
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.corp.firstam.com";
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Port = 25;
                smtp.Send(mailMessage);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception(string.Format("Failed to send Email due to {0}", Ex));
            }

        }
        public int DeleteCustomer(int value)
        {
            if (value > 0)
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "Customer").FirstOrDefault();
                    if (result != null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        public List<ApplicationMappingDTO> GetApplicationsList()
        {
            Entities dbContext = new Entities();
            var distinctApplications = dbContext.Applications
                .Select(se => new DataContracts.ApplicationMappingDTO()
                {
                    ApplicationId = se.ApplicationId,
                    ApplicationName = se.ApplicationName,
                });

            return distinctApplications.DistinctBy(Sm => Sm.ApplicationId).ToList();
        }

        public List<CategoryMapping> GetCategoryList(int tenantId)
        {
            Entities dbContext = new Entities();

            List<DataContracts.CategoryMapping> distinctcategories = new List<CategoryMapping>();
            if (tenantId != (int)TenantIdEnum.LVIS)
            {
                distinctcategories = dbContext.Categories
                    .Where(sel => sel.TenantId == tenantId)
                   .Select(se => new DataContracts.CategoryMapping()
                   {
                       CategoryId = se.CategoryId,
                       CategoryName = se.CategoryName,
                   }).ToList();
            }
            else
                distinctcategories = dbContext.Categories
                      .Select(se => new DataContracts.CategoryMapping()
                      {
                          CategoryId = se.CategoryId,
                          CategoryName = se.CategoryName,
                      }).ToList();

            return distinctcategories.DistinctBy(Sm => Sm.CategoryId).ToList();
        }

        public IEnumerable<LocationsMappings> GetLocations(int tenantId)
        {
            Entities dbContext = new Entities();

            var locations = dbContext.Locations
                .Select(sm => new DataContracts.LocationsMappings()
                {
                    LocationId = sm.LocationId,
                    ExternalId = sm.ExternalId,
                    LocationName = sm.LocationName + "(" + sm.ExternalId + ")",
                    CustomerId = sm.CustomerId.ToString(),
                    CustomerName = sm.Customer.CustomerName,
                    TenantId = sm.TenantId,
                    Tenant = sm.Tenant.TenantName,
                    Notes = sm.Notes,
                    ServicePreference = sm.ServicePreferenceMaps != null ? sm.ServicePreferenceMaps.Select(se => new ServicePreference()
                    {
                        ID = se.ServiceId,
                        Name = se.ServiceId == 0 ? string.Empty : dbContext.Services.Where(ser => ser.ServiceId == se.ServiceId).Select(sel => sel.ServiceName).FirstOrDefault(),
                        Ticked = true
                    }).ToList() : null,
                }).ToList();

            if (locations.Count() > 0 && tenantId != (int)TenantIdEnum.LVIS)
            {
                locations = locations
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return locations;
        }

        public IEnumerable<TypeCodeDTO> GetCustomerPreferenceTypes()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var CustomerPreferenceType = dbContext.TypeCodes.Where(sel => sel.GroupTypeCode == 800)
                .Select(se => new DataContracts.TypeCodeDTO()
                {
                    TypeCodeId = se.TypeCodeId,
                    TypeCodeDesc = se.TypeCodeDesc

                });
            return CustomerPreferenceType.OrderBy(se => se.TypeCodeDesc).ToList();
            //.OrderByDescending(se => se.TypeCodeId).ToList();
        }

        public IEnumerable<TypeCodeDTO> GetCustomerPreferenceSubTypes(int typeCodeId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            var CustomerPreferenceType = dbContext.TypeCodes.Where(sel => sel.GroupTypeCode == typeCodeId)
                .Select(se => new DataContracts.TypeCodeDTO()
                {
                    TypeCodeId = se.TypeCodeId,
                    TypeCodeDesc = se.TypeCodeDesc

                });
            return CustomerPreferenceType.OrderBy(se => se.TypeCodeDesc).ToList();
        }

        public bool IsUniqueUserName(string userName, int customerId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {
                return !dbContext.Credentials.Where(x => x.Username.ToLower() == userName.ToLower() && x.CustomerId != customerId).Any();
            }
        }

        public int ConfirmDeleteLocation(int value, int tenantId)
        {
            int success = 0;
            using (var dbContext = new Entities())
            {
                var LocationDelete = (from branch in dbContext.Locations
                                      where branch.LocationId == value
                                      select branch);

                if (LocationDelete != null)
                {
                    dbContext.Locations.RemoveRange(LocationDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }

        public int ConfirmDeleteCustomer(int value)
        {
            using (Entities dbContext = new Entities())
            {
                IEnumerable<TerminalDBEntities.CustomerInfo> Customerinfos = dbContext.CustomerInfoes
                .RemoveRange(dbContext.CustomerInfoes
                .Where(se => (se.CustomerId == value)));
                AuditLogHelper.SaveChanges(dbContext);

                IEnumerable<TerminalDBEntities.ServicePreferenceMap> ServicePreferenceMap = dbContext.ServicePreferenceMaps
                .RemoveRange(dbContext.ServicePreferenceMaps
                .Where(se => (se.CustomerId == value)));
                AuditLogHelper.SaveChanges(dbContext);

                IEnumerable<TerminalDBEntities.Credential> Credential = dbContext.Credentials
                .RemoveRange(dbContext.Credentials
                .Where(se => (se.CustomerId == value)));
                AuditLogHelper.SaveChanges(dbContext);

                IEnumerable<TerminalDBEntities.Customer> Customers = dbContext.Customers
               .RemoveRange(dbContext.Customers
               .Where(se => (se.CustomerId == value)));

                return AuditLogHelper.SaveChanges(dbContext);
            }
        }

        public ContactMappings AddContact(ContactMappings value, int userId, int tenantId)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.Contact ContacttoAdd = new TerminalDBEntities.Contact();

                ContacttoAdd.LVISContactId = value.LvisContactid;
                ContacttoAdd.LocationId = value.LocationId;
                ContacttoAdd.IsActive = value.IsActive;

                dbContext.Contacts.Add(ContacttoAdd);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                    value.ContactId = ContacttoAdd.ContactId;

                if (value.ServicePreference != null && value.ServicePreference.Count() > 0)
                {

                    foreach (var item in value.ServicePreference)
                    {

                        TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                        addservicepreference.CustomerId = dbContext.Locations.Where(ser => ser.LocationId == value.LocationId).Select(sel => sel.CustomerId).FirstOrDefault();
                        addservicepreference.LocationId = value.LocationId;
                        addservicepreference.ServiceId = item.ID;
                        addservicepreference.ContactId = value.ContactId;
                        addservicepreference.TenantId = tenantId;
                        addservicepreference.CreatedDate = DateTime.Now;
                        addservicepreference.CreatedById = userId;
                        addservicepreference.LastModifiedDate = DateTime.Now;
                        addservicepreference.LastModifiedById = userId;
                        dbContext.ServicePreferenceMaps.Add(addservicepreference);
                    }
                    AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return value;
        }

        public ContactProviderMappings AddContactProvider(ContactProviderMappings value, int userId, int tenantId)
        {
            int Success = 0;
            int i = 0;
            bool isNull = false;
            using (var dbContext = new Entities())
            {
                if (value.ContactId == null)
                {
                    var val1 = dbContext.ContactProviderMaps.Where(of => of.ProviderId == value.ProviderID && of.LocationId == value.LocationId && of.TenantId == value.TenantId && of.CustomerId == value.CustomerId).ToList();
                    if (val1.Count > 0)
                    {
                        for (i = 0; i < val1.Count; i++)
                        {
                            if (val1[i].ContactId == null)
                            {
                                isNull = true;
                            }
                        }
                        if (isNull == true)
                        {
                            value.ContactProviderMapId = 0;
                            return value;
                        }
                        else
                        {

                            TerminalDBEntities.ContactProviderMap ContactProviderAdd = new TerminalDBEntities.ContactProviderMap();
                            ContactProviderAdd.ProviderId = value.ProviderID;
                            ContactProviderAdd.ContactId = value.ContactId;
                            ContactProviderAdd.LocationId = value.LocationId;
                            ContactProviderAdd.CustomerId = value.CustomerId;
                            ContactProviderAdd.CreatedById = userId;
                            ContactProviderAdd.CreatedDate = DateTime.Now;
                            ContactProviderAdd.LastModifiedDate = DateTime.Now;
                            ContactProviderAdd.LastModifiedById = userId;
                            ContactProviderAdd.TenantId = tenantId;
                            dbContext.ContactProviderMaps.Add(ContactProviderAdd);
                            Success = AuditLogHelper.SaveChanges(dbContext);
                            if (Success == 1)
                            {
                                value.ContactProviderMapId = ContactProviderAdd.ContactProviderMapId;
                                value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderAdd.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                                if (ContactProviderAdd.ContactId != null)
                                {
                                    value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderAdd.ContactId).SingleOrDefault().LVISContactId;
                                }
                                value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderAdd.ProviderId).SingleOrDefault().ProviderName;
                            }
                            return value;
                        }

                    }
                    else
                    {

                        TerminalDBEntities.ContactProviderMap ContactProviderAdd = new TerminalDBEntities.ContactProviderMap();
                        ContactProviderAdd.ProviderId = value.ProviderID;
                        ContactProviderAdd.ContactId = value.ContactId;
                        ContactProviderAdd.LocationId = value.LocationId;
                        ContactProviderAdd.CustomerId = value.CustomerId;
                        ContactProviderAdd.CreatedById = userId;
                        ContactProviderAdd.CreatedDate = DateTime.Now;
                        ContactProviderAdd.LastModifiedDate = DateTime.Now;
                        ContactProviderAdd.LastModifiedById = userId;
                        ContactProviderAdd.TenantId = tenantId;
                        dbContext.ContactProviderMaps.Add(ContactProviderAdd);
                        Success = AuditLogHelper.SaveChanges(dbContext);
                        if (Success == 1)
                        {
                            value.ContactProviderMapId = ContactProviderAdd.ContactProviderMapId;
                            value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderAdd.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                            if (ContactProviderAdd.ContactId != null)
                            {
                                value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderAdd.ContactId).SingleOrDefault().LVISContactId;
                            }
                            value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderAdd.ProviderId).SingleOrDefault().ProviderName;
                        }
                    }
                }
                else
                {
                    var val2 = dbContext.ContactProviderMaps.Where(of => of.ProviderId == value.ProviderID && of.LocationId == value.LocationId && of.TenantId == value.TenantId && of.CustomerId == value.CustomerId && of.ContactId == value.ContactId).ToList();
                    if (val2.Count > 0)
                    {
                        value.ContactProviderMapId = 0;
                        return value;
                    }
                    else
                    {
                        TerminalDBEntities.ContactProviderMap ContactProviderAdd = new TerminalDBEntities.ContactProviderMap();
                        ContactProviderAdd.ProviderId = value.ProviderID;
                        ContactProviderAdd.ContactId = value.ContactId;
                        ContactProviderAdd.LocationId = value.LocationId;
                        ContactProviderAdd.CustomerId = value.CustomerId;
                        ContactProviderAdd.CreatedById = userId;
                        ContactProviderAdd.CreatedDate = DateTime.Now;
                        ContactProviderAdd.LastModifiedDate = DateTime.Now;
                        ContactProviderAdd.LastModifiedById = userId;
                        ContactProviderAdd.TenantId = tenantId;
                        dbContext.ContactProviderMaps.Add(ContactProviderAdd);
                        Success = AuditLogHelper.SaveChanges(dbContext);
                        if (Success == 1)
                        {
                            value.ContactProviderMapId = ContactProviderAdd.ContactProviderMapId;
                            value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderAdd.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                            if (ContactProviderAdd.ContactId != null)
                            {
                                value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderAdd.ContactId).SingleOrDefault().LVISContactId;
                            }
                            value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderAdd.ProviderId).SingleOrDefault().ProviderName;
                        }
                        return value;
                    }
                }
                return value;
            }
        }
        public int UpdateContact(ContactMappings value, int userId, int tenantId)
        {
            using (var dbContext = new Entities())
            {
                var ContactToUpdate = (from branch in dbContext.Contacts
                                       where branch.ContactId == value.ContactId
                                       select branch).FirstOrDefault();

                if (ContactToUpdate != null)
                {
                    ContactToUpdate.LVISContactId = value.LvisContactid;
                    ContactToUpdate.IsActive = value.IsActive;

                    dbContext.Entry(ContactToUpdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        IEnumerable<ServicePreferenceMap> ServicePreference = dbContext.ServicePreferenceMaps
                         .RemoveRange(dbContext.ServicePreferenceMaps
                         .Where(se => (se.ContactId == value.ContactId)));

                        AuditLogHelper.SaveChanges(dbContext);

                        if (value.ServicePreference != null && value.ServicePreference.Count() > 0)
                        {

                            foreach (var item in value.ServicePreference)
                            {

                                TerminalDBEntities.ServicePreferenceMap addservicepreference = new TerminalDBEntities.ServicePreferenceMap();

                                addservicepreference.CustomerId = dbContext.Locations.Where(ser => ser.LocationId == value.LocationId).Select(sel => sel.CustomerId).FirstOrDefault();
                                addservicepreference.LocationId = value.LocationId;
                                addservicepreference.ServiceId = item.ID;
                                addservicepreference.ContactId = value.ContactId;
                                addservicepreference.TenantId = tenantId;
                                addservicepreference.CreatedDate = DateTime.Now;
                                addservicepreference.CreatedById = userId;
                                addservicepreference.LastModifiedDate = DateTime.Now;
                                addservicepreference.LastModifiedById = userId;
                                dbContext.ServicePreferenceMaps.Add(addservicepreference);
                            }
                            AuditLogHelper.SaveChanges(dbContext);
                            return ContactToUpdate.ContactId;
                        }
                    }

                    else
                        return 0;
                }
            }

            return 0;
        }

        public ContactProviderMappings UpdateContactProvider(ContactProviderMappings value, int userId, int tenantId)
        {
            int Success = 0;
            int i = 0;
            bool isNull = false;
            using (var dbContext = new Entities())
            {
                if (value.ContactId == null)
                {
                    var val1 = dbContext.ContactProviderMaps.Where(of => of.ProviderId == value.ProviderID && of.LocationId == value.LocationId && of.TenantId == value.TenantId && of.CustomerId == value.CustomerId).ToList();
                    if (val1.Count > 0)
                    {
                        for (i = 0; i < val1.Count; i++)
                        {
                            if (val1[i].ContactId == null)
                            {
                                isNull = true;
                            }
                        }
                        if (isNull == true)
                        {
                            value.ContactProviderMapId = 0;
                            return value;
                        }
                        else
                        {
                            var ContactProviderToUpdate = (from branch in dbContext.ContactProviderMaps
                                                           where branch.ContactProviderMapId
                                                           == value.ContactProviderMapId
                                                           select branch).FirstOrDefault();
                            if (ContactProviderToUpdate != null)
                            {
                                ContactProviderToUpdate.ProviderId = value.ProviderID;
                                ContactProviderToUpdate.ContactId = value.ContactId;
                                ContactProviderToUpdate.LocationId = value.LocationId;
                                ContactProviderToUpdate.CreatedById = userId;
                                ContactProviderToUpdate.CreatedDate = DateTime.Now;
                                ContactProviderToUpdate.LastModifiedDate = DateTime.Now;
                                ContactProviderToUpdate.LastModifiedById = userId;
                                Success = AuditLogHelper.SaveChanges(dbContext);
                                if (Success == 1)
                                {
                                    value.ContactProviderMapId = ContactProviderToUpdate.ContactProviderMapId;
                                    value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderToUpdate.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                                    if (ContactProviderToUpdate.ContactId != null)
                                    {
                                        value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().LVISContactId;
                                        value.ContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().ContactId;
                                    }
                                    else
                                    {
                                        value.LvisContactId = string.Empty;
                                    }
                                    value.ProviderID = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderId;
                                    value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderName;
                                }
                            }
                            return value;
                        }

                    }
                    else
                    {
                        var ContactProviderToUpdate = (from branch in dbContext.ContactProviderMaps
                                                       where branch.ContactProviderMapId
                                                       == value.ContactProviderMapId
                                                       select branch).FirstOrDefault();
                        if (ContactProviderToUpdate != null)
                        {
                            ContactProviderToUpdate.ProviderId = value.ProviderID;
                            ContactProviderToUpdate.ContactId = value.ContactId;
                            ContactProviderToUpdate.LocationId = value.LocationId;
                            ContactProviderToUpdate.CreatedById = userId;
                            ContactProviderToUpdate.CreatedDate = DateTime.Now;
                            ContactProviderToUpdate.LastModifiedDate = DateTime.Now;
                            ContactProviderToUpdate.LastModifiedById = userId;
                            Success = AuditLogHelper.SaveChanges(dbContext);
                            if (Success == 1)
                            {
                                value.ContactProviderMapId = ContactProviderToUpdate.ContactProviderMapId;
                                value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderToUpdate.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                                if (ContactProviderToUpdate.ContactId != null)
                                {
                                    value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().LVISContactId;
                                    value.ContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().ContactId;
                                }
                                else
                                {
                                    value.LvisContactId = string.Empty;
                                }
                                value.ProviderID = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderId;
                                value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderName;
                            }
                        }
                    }
                }
                else
                {
                    var val2 = dbContext.ContactProviderMaps.Where(of => of.ProviderId == value.ProviderID && of.LocationId == value.LocationId && of.TenantId == value.TenantId && of.CustomerId == value.CustomerId && of.ContactId == value.ContactId).ToList();
                    if (val2.Count > 0)
                    {
                        value.ContactProviderMapId = 0;
                        return value;
                    }
                    else
                    {
                        var ContactProviderToUpdate = (from branch in dbContext.ContactProviderMaps
                                                       where branch.ContactProviderMapId
                                                       == value.ContactProviderMapId
                                                       select branch).FirstOrDefault();
                        if (ContactProviderToUpdate != null)
                        {
                            ContactProviderToUpdate.ProviderId = value.ProviderID;
                            ContactProviderToUpdate.ContactId = value.ContactId;
                            ContactProviderToUpdate.LocationId = value.LocationId;
                            ContactProviderToUpdate.CreatedById = userId;
                            ContactProviderToUpdate.CreatedDate = DateTime.Now;
                            ContactProviderToUpdate.LastModifiedDate = DateTime.Now;
                            ContactProviderToUpdate.LastModifiedById = userId;
                            Success = AuditLogHelper.SaveChanges(dbContext);
                            if (Success == 1)
                            {
                                value.ContactProviderMapId = ContactProviderToUpdate.ContactProviderMapId;
                                value.LocationName = dbContext.Locations.Where(se => se.LocationId == ContactProviderToUpdate.LocationId).SingleOrDefault().LocationName + "(" + value.LocationId + ")";
                                if (ContactProviderToUpdate.ContactId != null)
                                {
                                    value.LvisContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().LVISContactId;
                                    value.ContactId = dbContext.Contacts.Where(se => se.ContactId == ContactProviderToUpdate.ContactId).SingleOrDefault().ContactId;
                                }
                                else
                                {
                                    value.LvisContactId = string.Empty;
                                }
                                value.ProviderID = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderId;
                                value.ProviderName = dbContext.Providers.Where(se => se.ProviderId == ContactProviderToUpdate.ProviderId).SingleOrDefault().ProviderName;
                            }
                        }
                        return value;
                    }
                }
                return value;
            }
        }

        public int DeleteContact(int value)
        {
            if (value > 0)
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "Contact").FirstOrDefault();
                    if (result != null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }
        public int DeleteContactProvider(int value)
        {
            if (value > 0)
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetDependancyRecordOutput(value, "ContactProviderMap").FirstOrDefault();
                    if (result != null)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        public int ConfirmDeleteContact(int value)
        {
            int success = 0;
            using (var dbContext = new Entities())
            {
                var ContactDelete = (from branch in dbContext.Contacts
                                     where branch.ContactId == value
                                     select branch);

                if (ContactDelete != null)
                {
                    dbContext.Contacts.RemoveRange(ContactDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }

        public int ConfirmDeleteContactProvider(int value)
        {
            int success = 0;
            using (var dbContext = new Entities())
            {
                var ContactProviderDelete = (from branch in dbContext.ContactProviderMaps
                                             where branch.ContactProviderMapId == value
                                             select branch);

                if (ContactProviderDelete != null)
                {
                    dbContext.ContactProviderMaps.RemoveRange(ContactProviderDelete);
                    success = AuditLogHelper.SaveChanges(dbContext);
                }
            }
            return success;
        }


        public List<ServicePreference> GetServicePreferenceLocation(int locationid)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    return dbContext.ServicePreferenceMaps.Where(ser => ser.LocationId == locationid && ser.ContactId == null).Select(se => new ServicePreference()
                    {
                        ID = se.ServiceId,
                        Name = se.ServiceId == 0 ? string.Empty : dbContext.Services.Where(ser => ser.ServiceId == se.ServiceId).Select(sel => sel.ServiceName).FirstOrDefault(),
                        Ticked = true
                    }).ToList();
                }
            }
            catch { return null; }

        }

        public List<ServicePreference> GetServicePreferenceContact(int contactid)
        {
            using (var dbContext = new Entities())
            {
                try
                {
                    return dbContext.ServicePreferenceMaps.Where(ser => ser.ContactId == contactid).Select(se => new ServicePreference()
                    {
                        ID = se.ServiceId,
                        Name = se.ServiceId == 0 ? string.Empty : dbContext.Services.Where(ser => ser.ServiceId == se.ServiceId).Select(sel => sel.ServiceName).FirstOrDefault(),
                        Ticked = true
                    }).ToList();
                }
                catch { return null; }

            }

        }

        public List<ServicePreference> GetServicePreferenceCustomer(int customerid)
        {
            using (var dbContext = new Entities())
            {
                try
                {
                    return dbContext.ServicePreferenceMaps.Where(ser => ser.CustomerId == customerid && ser.LocationId == null && ser.ContactId == null).Select(se => new ServicePreference()
                    {
                        ID = se.ServiceId,
                        Name = se.ServiceId == 0 ? string.Empty : dbContext.Services.Where(ser => ser.ServiceId == se.ServiceId).Select(sel => sel.ServiceName).FirstOrDefault(),
                        Ticked = true
                    }).ToList();
                }
                catch { return null; }

            }

        }

        public IEnumerable<webhook> GetWebhooks(string Customer)
        {
            JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() { Formatting = Formatting.None };
            List<webhook> FilterWebhoks = new List<webhook>();
            using (var dbContext = new Entities())
            {
                try
                {
                    var registrations = dbContext.WebHooks.Where(se => se.User == Customer);

                    foreach (var reg in registrations)
                    {
                        Microsoft.AspNet.WebHooks.WebHook webHook = JsonConvert.DeserializeObject<Microsoft.AspNet.WebHooks.WebHook>(reg.ProtectedData, _serializerSettings);

                        webhook Details = new webhook()
                        {
                            UserID = reg.Id,
                            URL = webHook.WebHookUri.AbsoluteUri,
                            ActionType = webHook.Filters.FirstOrDefault(),
                            Secret = webHook.Secret,
                            Active = webHook.IsPaused ? "No" : "Yes",
                            MaxAttempts = webHook.Properties.ContainsKey("retryCount") ? Convert.ToInt32((webHook.Properties["retryCount"])) : 1,
                            X_API_ID = webHook.Headers.ContainsKey("x-api-key") ? webHook.Headers["x-api-key"] : "",
                        };

                        FilterWebhoks.Add(Details);
                    };

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

            }


            return FilterWebhoks;

        }

        public string[] GetAvailbleActionType(string User)
        {
            JsonSerializerSettings _serializerSettings = new JsonSerializerSettings() { Formatting = Formatting.None };
            List<string> FilterWebhoks = new List<string>();
            using (var dbContext = new Entities())
            {
                foreach (var reg in dbContext.WebHooks.Where(se => se.User == User))
                {
                    FilterWebhoks.Add(JsonConvert.DeserializeObject<Microsoft.AspNet.WebHooks.WebHook>(reg.ProtectedData, _serializerSettings)
                        .Filters.FirstOrDefault());
                }
            }
            return FilterWebhoks.ToArray();
        }

        public string GetWebhookUser(int customerID)
        {
            using (Entities dbContext = new Entities())
            {
                var webhookUserLocation = dbContext.Locations.FirstOrDefault(se => se.CustomerId == customerID);
                return webhookUserLocation.ExternalId;
            }
        }

        public int AddWebhook(webhookDto value)
        {
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.WebHook webHookAdd = new TerminalDBEntities.WebHook();
                webHookAdd.Id = value.webhook.UserID;
                webHookAdd.User = value.User;
                webHookAdd.ProtectedData = getProtectedData(value);

                dbContext.WebHooks.Add(webHookAdd);
                return AuditLogHelper.SaveChanges(dbContext);
            }
        }

        public int DeleteWebhook(webhookDto value)
        {
            using (Entities dbContext = new Entities())
            {
                var webhook = dbContext.WebHooks.AsEnumerable().FirstOrDefault(se => se.Id == value.webhook.UserID);
                if (webhook == null)
                    return 0;

                dbContext.WebHooks.Remove(webhook);

                return AuditLogHelper.SaveChanges(dbContext);
            }

        }
        public int EditWebhook(webhookDto value)
        {
            using (Entities dbContext = new Entities())
            {
                var webhook = dbContext.WebHooks.AsEnumerable().FirstOrDefault(se => (se.Id == value.OriginalUserID));
                if (webhook == null)
                    return 0;

                if (!string.IsNullOrEmpty(value.OriginalUserID) && value.OriginalUserID != value.webhook.UserID)
                {
                    TerminalDBEntities.WebHook webHookAdd = new TerminalDBEntities.WebHook();
                    webHookAdd.Id = value.webhook.UserID;
                    webHookAdd.User = value.User;
                    webHookAdd.ProtectedData = getProtectedData(value, webhook.ProtectedData);

                    dbContext.WebHooks.Add(webHookAdd);
                    dbContext.WebHooks.Remove(webhook);

                }

                else
                {
                    webhook.ProtectedData = getProtectedData(value, webhook.ProtectedData);
                }
                return AuditLogHelper.SaveChanges(dbContext);
            }
        }


        public IEnumerable<DataContracts.WebhookDomain> GetWebhookDomainsDummy(int CustomerId)
        {
            return new List<DataContracts.WebhookDomain>() {
                new DataContracts.WebhookDomain()
                {
                     CustomerId = 10300,
                     Domain = "www.Hardcoded.com",
                     WebhookDomainId = 1
                },
                new DataContracts.WebhookDomain()
                {
                     CustomerId = 10300,
                     Domain = "www.AnotherHardcoded.com",
                     WebhookDomainId = 2
                },
                new DataContracts.WebhookDomain()
                {
                     CustomerId = 10300,
                     Domain = "www.YetAnotherHardcoded.com",
                     WebhookDomainId = 3
                }

            };
        }

        public IEnumerable<DataContracts.WebhookDomain> GetWebhookDomains(int customerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            //IEnumerable<DataContracts.WebhookDomain> webhookDomains = dbContext.Database.SqlQuery<WebhookDomain>($"Select * from webhookDomain where customerId={customerId}");
            IEnumerable<DataContracts.WebhookDomain> webhookDomains = dbContext.WebhookDomains
                .Where(x => x.CustomerId == customerId)
                .Select(x => new DataContracts.WebhookDomain()
                {
                    WebhookDomainId = x.WebhookDomainId,
                    CustomerId = x.CustomerId,
                    Domain = x.Domain
                });
            return webhookDomains;
        }

        public DataContracts.WebhookDomain AddWebhookDomain(DataContracts.WebhookDomain webhookDomain, int userId)
        {
            //TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            //var result = dbContext.Database.SqlQuery<TerminalDBEntities.WebhookDomain>("Insert into WebhookDomain values({0},{1}, {2}, {3}, {4}, {5}); select * from WebhookDomain where webhookDomainId = @@IDENTITY ",
            //                                                                    webhookDomain.CustomerId,
            //                                                                    webhookDomain.Domain,
            //                                                                    DateTime.Now,
            //                                                                    0,
            //                                                                    DateTime.Now,
            //                                                                    0).FirstOrDefault();
            using (Entities dbContext = new Entities())
            {
                TerminalDBEntities.WebhookDomain addWebhookDomain = new TerminalDBEntities.WebhookDomain();

                addWebhookDomain.CustomerId = webhookDomain.CustomerId;
                addWebhookDomain.Domain = webhookDomain.Domain;
                addWebhookDomain.CreatedDate = DateTime.Now;
                addWebhookDomain.CreatedById = userId;
                addWebhookDomain.LastModifiedDate = DateTime.Now;
                addWebhookDomain.LastModifiedById = userId;

                dbContext.WebhookDomains.Add(addWebhookDomain);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                {
                    webhookDomain.WebhookDomainId = addWebhookDomain.WebhookDomainId;
                }
            }
            return webhookDomain;
        }

        public int UpdateWebhookDomain(DataContracts.WebhookDomain value, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                var webhookDomain = dbContext.WebhookDomains.Where(x => x.WebhookDomainId == value.WebhookDomainId).FirstOrDefault();
                if(webhookDomain != null)
                {
                    webhookDomain.CustomerId = value.CustomerId;
                    webhookDomain.Domain = value.Domain;
                    webhookDomain.LastModifiedDate = DateTime.Now;
                    webhookDomain.LastModifiedById = userId;

                    return AuditLogHelper.SaveChanges(dbContext);
                }
            }

            return 0;
        }

        public int DeleteWebhookDomain(DataContracts.WebhookDomain value)
        {
            using (Entities dbContext = new Entities())
            {
                var webhookDomain = dbContext.WebhookDomains.AsEnumerable().FirstOrDefault(x => x.WebhookDomainId == value.WebhookDomainId);
                if (webhookDomain == null)
                    return 0;

                dbContext.WebhookDomains.Remove(webhookDomain);

                return AuditLogHelper.SaveChanges(dbContext);
            }

        }

        private string getProtectedData(webhookDto value, string existingProtectedData)
        {
            JObject jObject = JObject.Parse(existingProtectedData);
            constructProtectedData(value.webhook, jObject);
            string protectedData = string.Empty;
            protectedData = JsonConvert.SerializeObject(jObject);
            return protectedData;

        }
        public static JObject constructProtectedData(webhook wHook, JObject jObject)
        {
            string Id = "Id";
            string WebHookUri = "WebHookUri";
            string Secret = "Secret";
            string IsPaused = "IsPaused";
            string Filters = "Filters";
            string Properties = "Properties";
            string Headers = "Headers";
            string X_API_ID = "x-api-key";
            if (jObject.Property("id") != null)
            {
                Id = "id";
                WebHookUri = "webHookUri";
                Secret = "secret";
                IsPaused = "isPaused";
                Filters = "filters";
                Properties = "properties";
                Headers = "headers";
            }

            if (jObject.Property(Id) != null)
            {
                jObject[Id] = wHook.UserID;
            }
            else
            {
                jObject.Add(Id, wHook.UserID);
            }

            if (jObject.Property(WebHookUri) != null)
            {
                jObject[WebHookUri] = wHook.URL;
            }
            else
            {
                jObject.Add(WebHookUri, wHook.URL);
            }


            if (jObject.Property(Headers) != null)
            {
                if (jObject[Headers].HasValues)
                {
                    jObject[Headers][X_API_ID] = wHook.X_API_ID;
                }
                else
                {
                    jObject[Headers] = new JObject();
                    jObject[Headers][X_API_ID] = new JValue(wHook.X_API_ID);
                }
            }
            else
            {

                jObject.Add(Headers, new JObject());
                jObject[Headers][X_API_ID] = new JValue(wHook.X_API_ID);
            }

            if (jObject.Property(Secret) != null)
            {
                jObject[Secret] = wHook.Secret;
            }
            else
            {
                jObject.Add(Secret, wHook.Secret);
            }
            if (jObject.Property(IsPaused) != null)
            {
                jObject[IsPaused] = !string.IsNullOrEmpty(wHook.Active) && wHook.Active.ToUpper() == "No".ToUpper();
            }
            else
            {
                jObject.Add(IsPaused, !string.IsNullOrEmpty(wHook.Active) && wHook.Active.ToUpper() == "No".ToUpper());
            }
            if (jObject.Property(Filters) != null)
            {
                jObject[Filters] = new JArray(wHook.ActionType);
            }
            else
            {
                jObject.Add(Filters, new JArray(wHook.ActionType));
            }
            if (jObject.Property(Properties) != null)
            {
                if (jObject[Properties].HasValues)
                {
                    jObject[Properties]["retryCount"] = wHook.MaxAttempts;
                }
                else
                {
                    jObject[Properties] = new JObject();
                    jObject[Properties]["retryCount"] = new JValue(wHook.MaxAttempts);
                }
            }
            else
            {
                jObject.Add(Properties, new JObject());
                jObject[Properties]["retryCount"] = new JValue(wHook.MaxAttempts);

            }

            return (jObject);

        }



        private string getProtectedData(webhookDto value)
        {
            string protectedData = string.Empty;
            string[] Filter = new string[1];
            Filter[0] = value.webhook.ActionType;
            ProtectedDataWebhook protectedDataWebhook = new ProtectedDataWebhook();
            protectedDataWebhook.Id = value.webhook.UserID;
            protectedDataWebhook.Secret = value.webhook.Secret;

            protectedDataWebhook.Filters = Filter;

            protectedDataWebhook.IsPaused = value.webhook.Active.ToUpper() == "YES" ? false : true;
            protectedDataWebhook.WebHookUri = value.webhook.URL;
            protectedDataWebhook.Properties = new Dictionary<string, object>() { { "retryCount", value.webhook.MaxAttempts } };
            protectedDataWebhook.Headers = new Dictionary<string, object>() { { "x-api-key", value.webhook.X_API_ID } };

            protectedData = JsonConvert.SerializeObject(protectedDataWebhook);
            return protectedData;

        }
    }
}
