using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{

    public interface ICategoryMappingDataProvider : IDataProviderBase
    {
        List<DC.CategoryMapping> GetCategoryMappings(int tenantId);

        DC.CategoryMapping AddCategory(DC.CategoryMapping UserProfile, int tenantId, int iEmployeeid);

        int ConfirmDelete(int ID);


        int UpdateCategory(DC.CategoryMapping UserProfile, int iEmployeeid);


        int DeleteCategory(int value);





    }
}
