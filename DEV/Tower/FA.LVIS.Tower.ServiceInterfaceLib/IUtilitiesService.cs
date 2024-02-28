using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Services
{
    public interface IUtilitiesService : Core.IServiceBase
    {

        int UpdateExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId);

        int UpdateandAcceptExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId);

        int UpdateServiceRequestInfo(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid, string customerRefnum, int userId, int status,bool chkUniqueID, bool chkExternalRefNum);

        

        ServiceRequestDTO GetServiceReqInfo(int servicerequestid);
        IEnumerable<ExceptionStatus> GetStatus();
        bool ConfirmService(int servicerequestid, int userid);
        IEnumerable<ApplicationMappingDTO> GetApplications();
        bool AddCredentials(AddCredentialDTO addCredential,int userId);
        FastAppDetailsDTO PopulateFastAppEnvironmentDetails();
    }
}
