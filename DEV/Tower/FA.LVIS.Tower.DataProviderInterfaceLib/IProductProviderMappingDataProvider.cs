using FA.LVIS.Tower.Core;
using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IProductProviderMappingDataProvider : IDataProviderBase
    {
        List<DC.ProductProviderMap> GetProductProviderMappings(string providerId, int tenantId);

        DC.ProductProviderMap AddProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId);

        DC.ProductProviderMap UpdateProductProvider(DC.ProductProviderMap productProvider, int employeeId, int tenantId);

        int DeleteProductProvider(int productProviderId);
    }
}
