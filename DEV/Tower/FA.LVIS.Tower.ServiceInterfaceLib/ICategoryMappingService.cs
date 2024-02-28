using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface ICategoryMappingService : Core.IServiceBase
    {       
        List<DC.CategoryMapping> GetCategoryMappings(int tenantId);

        DC.CategoryMapping AddCategory(DC.CategoryMapping UserProfile, int tenantId, int iEmployeeid);

        int DeleteCategory(int value);

        int ConfirmDelete(int ID);

        int UpdateCategory(DC.CategoryMapping UserProfile, int iEmployeeid);
    }
}
