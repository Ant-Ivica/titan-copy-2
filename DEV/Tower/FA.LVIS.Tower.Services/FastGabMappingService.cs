using System.Collections.Generic;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    class FastGabMappingService : Core.ServiceBase, IFastGabMappingService
    {
        public FASTGABMap AddFastGab(FASTGABMap value, int iEmployeeid)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.AddFastGab(value, iEmployeeid);
        }

        public List<FASTGABMap> GetFastGabDetails(string Locationid)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.GetFastGabDetails(Locationid);
        }

        public FASTGABMap GetFastGabMap(int gabId)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.GetFastGabMap(gabId);
        }

        public FASTGABMap UpdateFastGab(FASTGABMap value, int iEmployeeid)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.UpdateFastGab(value, iEmployeeid);
        }
    
        public List<TypeCodeDTO> GetLoanTypeDetatils()
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.GetLoanTypeDetatils();
        }

        public int DeleteGab(int value)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.DeleteGab(value);
        }

        public int ConfirmDeleteGab(int value)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.ConfirmDeleteGab(value);
        }

        public IEnumerable<FASTGABMap> GetFastGabDetails(string locationId, string stateFipsId, string countyFipsId, int tenantId)
        {
            IFastGabDataProvider customerProvider = DataProviderFactory.Resolve<IFastGabDataProvider>();
            return customerProvider.GetFastGabSearchResults(locationId, stateFipsId, countyFipsId, tenantId);
        }
    }
}
