using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IFastGabMappingService : Core.IServiceBase
    {
        List<DC.FASTGABMap> GetFastGabDetails(string Locationid);
        DC.FASTGABMap AddFastGab(DC.FASTGABMap value ,int iEmployeeid);
        DC.FASTGABMap UpdateFastGab(DC.FASTGABMap value, int iEmployeeid);
        List<DC.TypeCodeDTO> GetLoanTypeDetatils();
        int DeleteGab(int value);

        int ConfirmDeleteGab(int value);

        IEnumerable<DC.FASTGABMap> GetFastGabDetails(string locationId, string stateFipsId, string countyFipsId, int tenantId);

        DC.FASTGABMap GetFastGabMap(int gabId);
    }
}
