using System;
using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class CategoryMappingService : Core.ServiceBase, ICategoryMappingService
    {
        public DC.CategoryMapping AddCategory(DC.CategoryMapping UserProfile, int tenantId, int userId)
        {
            return DataProviderFactory.Resolve<ICategoryMappingDataProvider>().AddCategory(UserProfile, tenantId, userId);
        }

        public int ConfirmDelete(int ID)
        {
            return DataProviderFactory.Resolve<ICategoryMappingDataProvider>().ConfirmDelete(ID);
        }

        public int DeleteCategory(int value)
        {
            return DataProviderFactory.Resolve<ICategoryMappingDataProvider>().DeleteCategory(value);
        }

        public List<DC.CategoryMapping> GetCategoryMappings(int tenantId)
        {
            return DataProviderFactory.Resolve<ICategoryMappingDataProvider>().GetCategoryMappings(tenantId);
        }

        public int UpdateCategory(DC.CategoryMapping UserProfile, int userId)
        {
            return DataProviderFactory.Resolve<ICategoryMappingDataProvider>().UpdateCategory(UserProfile, userId);
        }
    }

}
