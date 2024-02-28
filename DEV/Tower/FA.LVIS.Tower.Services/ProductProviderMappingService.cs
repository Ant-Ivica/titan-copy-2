using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    public class ProductProviderMappingService : Core.ServiceBase, IProductProviderMappingService
    {
        public List<DC.ProductProviderMap> GetProductProviderMappings(string providerId, int tenantId)
        {
            IProductProviderMappingDataProvider OffProductProvider = DataProviderFactory.Resolve<IProductProviderMappingDataProvider>();
            return OffProductProvider.GetProductProviderMappings(providerId, tenantId);
        }

        public DC.ProductProviderMap AddProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId)
        {
            IProductProviderMappingDataProvider OffProductProvider = DataProviderFactory.Resolve<IProductProviderMappingDataProvider>();
            return OffProductProvider.AddProductProvider(productProvider, employeeId, tenantId);
        }

        public DC.ProductProviderMap UpdateProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId)
        {
            IProductProviderMappingDataProvider OffProductProvider = DataProviderFactory.Resolve<IProductProviderMappingDataProvider>();
            return OffProductProvider.UpdateProductProvider(productProvider, employeeId, tenantId);
        }

        public int DeleteProductProvider(int productProviderId)
        {
            IProductProviderMappingDataProvider OffProductProvider = DataProviderFactory.Resolve<IProductProviderMappingDataProvider>();
            return OffProductProvider.DeleteProductProvider(productProviderId);
        }
    }
}
