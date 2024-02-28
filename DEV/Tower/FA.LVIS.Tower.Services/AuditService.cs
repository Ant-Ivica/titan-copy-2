using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.Services
{
    public class AuditService : Core.ServiceBase, IAuditService
    {
        public void AddUserAudit(DC.AuditingDTO log)
        {
            IAuditDataProvider outDocProvider = DataProviderFactory.Resolve<IAuditDataProvider>();
             outDocProvider.AddUserAudit(log);
        }

        public List<DC.AuditingDTO> GetAuditDetails(string sFilter, int tenantId)
        {
            IAuditDataProvider outDocProvider = DataProviderFactory.Resolve<IAuditDataProvider>();
            return outDocProvider.GetAuditDetails(sFilter, tenantId);
        }

        public List<DC.AuditingDTO> GetAuditDetails(DC.SearchDetail SearchDetails)
        {
            IAuditDataProvider outDocProvider = DataProviderFactory.Resolve<IAuditDataProvider>();
            return outDocProvider.GetAuditDetails(SearchDetails);
        }
    }

}
