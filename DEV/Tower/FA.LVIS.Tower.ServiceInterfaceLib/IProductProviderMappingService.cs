using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IProductProviderMappingService : Core.IServiceBase
    {
        List<DC.ProductProviderMap> GetProductProviderMappings(string providerId, int tenantId);

        DC.ProductProviderMap AddProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId);

        DC.ProductProviderMap UpdateProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId);

        int DeleteProductProvider(int productProviderId);

    }
}
