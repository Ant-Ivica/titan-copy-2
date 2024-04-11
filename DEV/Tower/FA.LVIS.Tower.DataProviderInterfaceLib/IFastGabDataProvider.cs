using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IFastGabDataProvider : IDataProviderBase
    {
        List<DC.FASTGABMap> GetFastGabDetails(string Locationid);

        DC.FASTGABMap AddFastGab(DC.FASTGABMap value, int iEmployeeid);

        DC.FASTGABMap UpdateFastGab(DC.FASTGABMap value, int iEmployeeid);

        List<DC.TypeCodeDTO> GetLoanTypeDetatils();

        int DeleteGab(int value);

        int ConfirmDeleteGab(int value);

        //IEnumerable<DC.FASTGABMap> GetFastGabDetails(int stateFipsId, int CountyFipsId);

        DC.FASTGABMap GetFastGabMap(int gabId);

        IEnumerable<DC.FASTGABMap> GetFastGabSearchResults(string locationId, string stateFipsId, string countyFipsId, int tenantId);
    }
}
