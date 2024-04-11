using System;
using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
    public interface IAuditService : Core.IServiceBase
    {
        List<DC.AuditingDTO> GetAuditDetails(string sFilter, int tenantId);

        List<DC.AuditingDTO> GetAuditDetails(DC.SearchDetail SearchDetails);

        void AddUserAudit(DC.AuditingDTO log);
    }
}
