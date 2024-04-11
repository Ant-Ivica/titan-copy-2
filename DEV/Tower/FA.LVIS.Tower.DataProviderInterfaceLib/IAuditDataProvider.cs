using FA.LVIS.Tower.Core;
using FA.LVIS.Tower.DataContracts;
using System.Collections.Generic;

namespace FA.LVIS.Tower.Data
{
    public interface IAuditDataProvider : IDataProviderBase
    {
        List<AuditingDTO> GetAuditDetails(string sFilter, int tenantId);

        List<AuditingDTO> GetAuditDetails(SearchDetail SearchDetails);

        void AddUserAudit(AuditingDTO log);
    }
}