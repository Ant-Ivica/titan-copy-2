using System.Collections.Generic;
using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IUtilitiesDataProvider : IDataProviderBase
    {

        int UpdateExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId);

        int UpdateandAcceptExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId);
        bool ConfirmService(int servicerequestid, int userid);
        int UpdateServiceRequestInfo(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid, string customerRefnum, int userId,int status,bool chkUniqueID, bool chkExternalRefNum);

        DC.ServiceRequestDTO GetServiceReqInfo(int servicerequestid);
        IEnumerable<DC.ExceptionStatus> GetStatus();
        IEnumerable<DC.ApplicationMappingDTO> GetApplications();
        bool AddCredentials(string applicationName, string userName, string password,int userId);
    }
}
