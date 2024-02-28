using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.Core;

namespace FA.LVIS.Tower.Data
{
    public class CustomerDataProvider : Core.DataProviderBase, ICustomerDataProvider
    {
        public List<DataContracts.Customer> GetLVISCustomers()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
          
            
            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            var distinctCustomers = dbContext.Customers.Select(a => new { a.CustomerId, a.CustomerName })
                .Distinct().ToList();

            // convert the entity classes to DTO classes
            List<DataContracts.Customer> customers = new List<DataContracts.Customer>();
            foreach (var customer in distinctCustomers)
                customers.Add(new DataContracts.Customer()
                {
                    LvisCustomerID = customer.CustomerId.ToString(),
                    CustomerName = customer.CustomerName
                });

            return customers;
        }

        public List<DataContracts.CustomerDetails> GetLVISCustomers(int iTenatid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();


            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            var distinctCustomers = dbContext.Customers.Select(a => new { a.CustomerId, a.CustomerName,a.TenantId })
                .Distinct().ToList();

            // convert the entity classes to DTO classes
            List<DataContracts.CustomerDetails> customers = new List<DataContracts.CustomerDetails>();
            foreach (var customer in distinctCustomers)
            {
                if  (iTenatid == customer.TenantId)
                {
                    customers.Add(new DataContracts.CustomerDetails()
                    {
                        LvisCustomerID = customer.CustomerId,
                        CustomerName = customer.CustomerName + "(" + customer.CustomerId.ToString() + ")",

                    });
                }
                else if (iTenatid == (int)TenantIdEnum.LVIS)
                {
                    customers.Add(new DataContracts.CustomerDetails()
                    {
                        LvisCustomerID = customer.CustomerId,
                        CustomerName = customer.CustomerName + "(" + customer.CustomerId.ToString() + ")",

                    });
                }
            }

            return customers;
        }

        private static ConnectorType GetConnectorType(string connectorName)
        {
            //return (DataContracts.ConnectorType)Enum.Parse(typeof(DTO.ConnectorType), connectorName);
            if (string.IsNullOrEmpty(connectorName))
                return DataContracts.ConnectorType.RealEC;

            if (connectorName.Trim().ToUpper() == Constants.APPLICATION_REALEC)
                return DataContracts.ConnectorType.RealEC;
            else
                return DataContracts.ConnectorType.eLynx;
        }
            
        public List<DataContracts.Regions> GetFastRegions()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            //List<DTO.Customer> customers = dbContext.PIDMappings.Select(a => new DTO.Customer {
            //    LvisCustomerID = a.ParentABEID, CustomerName = a.CustomerName, Connector = GetConnectorType(a.Connector)})
            //    .Distinct().ToList();

            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            var DistinctRegions = dbContext.FASTRegions 
                .Select(a => new { a.RegionID , a.Name })
                .Distinct().OrderBy(a => a.Name).ToList();

            // convert the entity classes to DTO classes
            List<DataContracts.Regions> Regions = new List<DataContracts.Regions>();
            foreach (var Region in DistinctRegions)
                Regions.Add(new DataContracts.Regions()
                {
                    Name = Region.Name + " (" + Region.RegionID + ")",
                    Id = Region.RegionID
                });

            return Regions;
        }

        public IEnumerable<Regions> GetFastRegions(string application)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            if(application != "")
                 application = (application.ToUpper().Contains(Constants.APPLICATION_FAST)) ? Constants.APPLICATION_FAST : application.Split(',')[0];

            int AppName = dbContext.Applications.Where(se => se.ApplicationName.ToLower() == application.ToLower()).Select(sl => sl.ApplicationId).FirstOrDefault();

            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            var DistinctRegions = dbContext.FASTRegions.Where(re => re.ApplicationID == AppName)
                .Select(a => new { a.RegionID, a.Name })
                .Distinct().OrderBy(a => a.Name).ToList();

            // convert the entity classes to DTO classes
            List<DataContracts.Regions> Regions = new List<DataContracts.Regions>();

            if (DistinctRegions.Count() == 0)
            {

                var DefaultRegion = new Regions();
                DefaultRegion.Id = 0;
                DefaultRegion.Name = "--NA-- (0)";
                DefaultRegion.Application = application.Trim();
                DefaultRegion.Ticked = false;
                Regions.Add(DefaultRegion);
            }
            else
            {
                foreach (var Region in DistinctRegions)
                    Regions.Add(new DataContracts.Regions()
                    {
                        Name = Region.Name + " (" + Region.RegionID + ")",
                        Id = Region.RegionID,
                        Ticked = false,
                        Application = application
                    });
            }

            return Regions;
        }

        public IEnumerable<Users> GetExternalApplications()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

           // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            IEnumerable<Users> Extusers = dbContext.Applications.Where(se => !se.IsInternal).Select(a => new Users{ID = a.ApplicationId,Name = a.ApplicationName }).ToList();
        
            return Extusers;
        }

        public IEnumerable<Users> GetInternalApplications()
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            IEnumerable<Users> Intusers = dbContext.Applications.Where(se => se.IsInternal).Select(a => new Users { ID = a.ApplicationId, Name = a.ApplicationName }).ToList();

            return Intusers;
        }

        public IEnumerable<FastOffices> GetfastOffices(int Region)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            List<DataContracts.FastOffices> DistinctOffices = dbContext.FASTOffices.Where(of => of.RegionID == Region)
                .Select(a => new FastOffices() { id = a.OfficeID, Name = (a.OfficeName + " (" + a.OfficeID + ")") }).ToList();

            return DistinctOffices;
        }

        public IEnumerable<ContactMappings> GetContact(int locationid)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();

            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            List<DataContracts.ContactMappings> DistinctContacts = dbContext.Contacts.Where(of => of.LocationId == locationid)
                .Select(a => new ContactMappings() {
                    ContactId = a.ContactId,
                    LvisContactid = a.LVISContactId,
                    IsActive = a.IsActive
                }).ToList();

            return DistinctContacts;

        }

        public IEnumerable<ContactProviderMappings> GetContactProviderDetails(int CustomerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            List<DataContracts.ContactProviderMappings> DistinctContactProviderMaps = new List<ContactProviderMappings>();
            var contactProviderList = dbContext.ContactProviderMaps.Where(of =>of.CustomerId == CustomerId).ToList();
            foreach (var contactProvider in contactProviderList)
                DistinctContactProviderMaps.Add(new DataContracts.ContactProviderMappings()
                {
                    ContactProviderMapId = contactProvider.ContactProviderMapId,
                    ProviderID = contactProvider.ProviderId,
                    ProviderName = contactProvider.Provider.ProviderName,
                    CustomerId = contactProvider.CustomerId,
                    CustomerName = contactProvider.Customer.CustomerName,
                    LocationId = contactProvider.LocationId,
                    LocationName = contactProvider.Location.LocationName + "(" + contactProvider.LocationId + ")",
                    TenantId = contactProvider.TenantId,
                    Tenant = contactProvider.Tenant.TenantName,
                    ContactId = contactProvider.ContactId,
                    LvisContactId = contactProvider.Contact != null ? contactProvider.Contact.LVISContactId : "",
                });         
            return DistinctContactProviderMaps;
        }

        public List<LocationsMappings> GetLocationList(int CustomerId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
            var distinctLocation = dbContext.Locations
                .Select(se => new DataContracts.LocationsMappings()
                {
                    LocationId = se.LocationId,
                    LocationName = se.LocationName + "(" + se.LocationId + ")",
                    TenantId = se.TenantId
                });
            return distinctLocation.DistinctBy(Sm => Sm.LocationId).ToList();
        }      
    }
}
