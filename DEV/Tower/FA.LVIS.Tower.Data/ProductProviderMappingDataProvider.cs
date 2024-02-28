using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading;

namespace FA.LVIS.Tower.Data
{
    public class ProductProviderMappingDataProvider : Core.DataProviderBase, IProductProviderMappingDataProvider
    {
        public List<ProductProviderMap> GetProductProviderMappings(string providerId, int tenantId)
        {
            TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities();
            List<DataContracts.ProductProviderMap> ProductProviderMappings = new List<DataContracts.ProductProviderMap>();

            if (dbContext.ProductProviderMaps.Count() > 0)
            {
                ProductProviderMappings = dbContext.ProductProviderMaps.Where(ppr => ppr.ProviderId.ToString() == providerId)                
                .Select(p => new DataContracts.ProductProviderMap()
                {
                    ProductProviderMapId = p.ProductProviderMapId,
                    ProviderId = p.ProviderId,
                    ProviderName = dbContext.Providers.Where(a => a.ProviderId == p.ProviderId)
                                    .Select(a => a.ProviderName).FirstOrDefault() ??
                                    p.ProviderId.ToString(),
                    ExternalId = dbContext.Providers.Where(a => a.ProviderId == p.ProviderId)
                                    .Select(a => a.ExternalId).FirstOrDefault(),
                    ContactName = dbContext.Contacts.Where(c => c.ContactId == p.ContactId)
                                    .Select(c => c.LVISContactId).FirstOrDefault(),
                    ContactId = p.ContactId,
                    LocationName = dbContext.Locations.Where(l=> l.LocationId == p.LocationId)
                                    .Select(l=> l.LocationName).FirstOrDefault(),
                    LocationId = p.LocationId,
                    CustomerName = dbContext.Customers.Where(c=> c.CustomerId == p.CustomerId)
                                    .Select(c=> c.CustomerName).FirstOrDefault(),
                    CustomerId = p.CustomerId,
                    ProductName = dbContext.Products.Where(pr=> pr.ProductId == p.ProductId)
                                    .Select(pr=> pr.ProductName).FirstOrDefault(),
                    ProductId = p.ProductId,
                    Service = dbContext.Services.Where(s=> s.ServiceId == p.ServiceId)
                                    .Select(s=> s.ServiceName).FirstOrDefault(),
                    ServiceId = p.ServiceId,
                    Tenant = dbContext.Tenants.Where(t=> t.TenantId == p.TenantId)
                                    .Select(t=> t.TenantName).FirstOrDefault(),
                    TenantId = p.TenantId,
                    Application = dbContext.Applications.Where(a=> a.ApplicationId == p.ApplicationId)
                                    .Select(a=> a.ApplicationName).FirstOrDefault(),
                    ApplicationId = p.ApplicationId

                }).ToList();
            }

            if (ProductProviderMappings.Count() > 0 && tenantId != (int)TerminalDBEntities.TenantIdEnum.LVIS)
            {
                ProductProviderMappings = ProductProviderMappings
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return ProductProviderMappings;
        }


        public ProductProviderMap AddProductProvider(ProductProviderMap productProvider, int employeeId, int tenantId)
        {
            using (TerminalDBEntities.Entities dbContext = new TerminalDBEntities.Entities())
            {

                TerminalDBEntities.ProductProviderMap AddProductProvider = new TerminalDBEntities.ProductProviderMap();

                AddProductProvider.ProviderId = productProvider.ProviderId;
                AddProductProvider.ContactId = productProvider.ContactId;
                AddProductProvider.LocationId = productProvider.LocationId;
                AddProductProvider.CustomerId = productProvider.CustomerId;
                AddProductProvider.ProductId = productProvider.ProductId;
                AddProductProvider.ServiceId = productProvider.ServiceId;
                AddProductProvider.TenantId = tenantId;
                AddProductProvider.ApplicationId = productProvider.ApplicationId;
                AddProductProvider.CreatedDate = DateTime.Now;
                AddProductProvider.CreatedById = employeeId;
                AddProductProvider.LastModifiedDate = DateTime.Now;
                AddProductProvider.LastModifiedById = employeeId;

                dbContext.ProductProviderMaps.Add(AddProductProvider);
                int Success = AuditLogHelper.SaveChanges(dbContext);

                if (Success == 1)
                {
                    productProvider.ProductProviderMapId = AddProductProvider.ProductProviderMapId;
                    productProvider.ProviderName = dbContext.Providers.Where(a => a.ProviderId == AddProductProvider.ProviderId)
                                    .Select(a => a.ProviderName).FirstOrDefault() ??
                                    AddProductProvider.ProviderId.ToString();
                    productProvider.ExternalId = dbContext.Providers.Where(a => a.ProviderId == AddProductProvider.ProviderId)
                                    .Select(a => a.ExternalId).FirstOrDefault();
                    productProvider.ContactName = dbContext.Contacts.Where(c => c.ContactId == AddProductProvider.ContactId)
                                    .Select(c => c.LVISContactId).FirstOrDefault();
                    productProvider.LocationName = dbContext.Locations.Where(l => l.LocationId == AddProductProvider.LocationId)
                                    .Select(l => l.LocationName).FirstOrDefault();
                    productProvider.CustomerName = dbContext.Customers.Where(c => c.CustomerId == AddProductProvider.CustomerId)
                                    .Select(c => c.CustomerName).FirstOrDefault();
                    productProvider.ProductName = dbContext.Products.Where(pr => pr.ProductId == AddProductProvider.ProductId)
                                    .Select(pr => pr.ProductName).FirstOrDefault();
                    productProvider.Service = dbContext.Services.Where(s => s.ServiceId == AddProductProvider.ServiceId)
                                    .Select(s => s.ServiceName).FirstOrDefault();
                    productProvider.Tenant = dbContext.Tenants.Where(t => t.TenantId == AddProductProvider.TenantId)
                                    .Select(t => t.TenantName).FirstOrDefault();
                    productProvider.TenantId = AddProductProvider.TenantId;
                    productProvider.Application = dbContext.Applications.Where(a => a.ApplicationId == AddProductProvider.ApplicationId)
                                    .Select(a => a.ApplicationName).FirstOrDefault();


                    return productProvider;
                }
            }
            return productProvider;
        }


        public ProductProviderMap UpdateProductProvider(ProductProviderMap productProvider, int employeeId, int tenantId)
        {
            using (var dbContext = new TerminalDBEntities.Entities()) {

                var updateProductProvider = (from product in dbContext.ProductProviderMaps
                                             where product.ProductProviderMapId == productProvider.ProductProviderMapId
                                             select product).FirstOrDefault();


                if (updateProductProvider != null) {

                    updateProductProvider.ProviderId = productProvider.ProviderId;
                    updateProductProvider.ContactId = productProvider.ContactId;
                    updateProductProvider.LocationId = productProvider.LocationId;
                    updateProductProvider.CustomerId = productProvider.CustomerId;
                    updateProductProvider.ProductId = productProvider.ProductId;
                    updateProductProvider.ServiceId = productProvider.ServiceId;
                    updateProductProvider.ApplicationId = productProvider.ApplicationId;
                    updateProductProvider.LastModifiedDate = DateTime.Now;
                    updateProductProvider.LastModifiedById = employeeId;

                    dbContext.Entry(updateProductProvider).State = System.Data.Entity.EntityState.Modified;
                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                    {
                        productProvider.ProductProviderMapId = updateProductProvider.ProductProviderMapId;
                        productProvider.ProviderName = dbContext.Providers.Where(a => a.ProviderId == updateProductProvider.ProviderId)
                                        .Select(a => a.ProviderName).FirstOrDefault() ??
                                        updateProductProvider.ProviderId.ToString();
                        productProvider.ExternalId = dbContext.Providers.Where(a => a.ProviderId == updateProductProvider.ProviderId)
                                        .Select(a => a.ExternalId).FirstOrDefault();
                        productProvider.ContactName = dbContext.Contacts.Where(c => c.ContactId == updateProductProvider.ContactId)
                                        .Select(c => c.LVISContactId).FirstOrDefault();
                        productProvider.LocationName = dbContext.Locations.Where(l => l.LocationId == updateProductProvider.LocationId)
                                        .Select(l => l.LocationName).FirstOrDefault();
                        productProvider.CustomerName = dbContext.Customers.Where(c => c.CustomerId == updateProductProvider.CustomerId)
                                        .Select(c => c.CustomerName).FirstOrDefault();
                        productProvider.ProductName = dbContext.Products.Where(pr => pr.ProductId == updateProductProvider.ProductId)
                                        .Select(pr => pr.ProductName).FirstOrDefault();
                        productProvider.Service = dbContext.Services.Where(s => s.ServiceId == updateProductProvider.ServiceId)
                                        .Select(s => s.ServiceName).FirstOrDefault();
                        productProvider.Tenant = dbContext.Tenants.Where(t => t.TenantId == updateProductProvider.TenantId)
                                        .Select(t => t.TenantName).FirstOrDefault();
                        productProvider.TenantId = tenantId;
                        productProvider.Application = dbContext.Applications.Where(a => a.ApplicationId == updateProductProvider.ApplicationId)
                                        .Select(a => a.ApplicationName).FirstOrDefault();


                        return productProvider;
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            return productProvider;

        }


        public int DeleteProductProvider(int productProviderId)
        {
            using (var dbContext = new TerminalDBEntities.Entities())
            {
                IEnumerable<TerminalDBEntities.ProductProviderMap> ProductProviders = dbContext.ProductProviderMaps
                   .RemoveRange(dbContext.ProductProviderMaps
                   .Where(se => (se.ProductProviderMapId == productProviderId)));

                return AuditLogHelper.SaveChanges(dbContext);
            }
            
        }


    }
}
