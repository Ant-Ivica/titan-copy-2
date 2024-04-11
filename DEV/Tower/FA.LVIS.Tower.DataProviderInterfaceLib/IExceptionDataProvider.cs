using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IExceptionDataProvider : IDataProviderBase
    {
        IEnumerable<DC.DashBoardExceptionDTO> GetBEQExceptions(int tenantId);

        IEnumerable<DC.DashBoardExceptionDTO> GetTEQExceptions(int tenantId);

        IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetBEQGraphicalExceptions(int tenantId);

        IEnumerable<DC.DashBoardGraphicalExceptionDTO> GetTEQGraphs(int tenantId);

        IEnumerable<DC.ExceptionDTO> GetTEQExceptions(DC.SearchDetail Details, int tenantId);

        IEnumerable<DC.ExceptionDTO> GetBEQExceptions(DC.SearchDetail Details, int tenantId);

        IEnumerable<DC.MessageLogDetailDTO> GetMessageDetails(int iExceptionid);

        IEnumerable<DC.ExceptionDTO> GetTEQExceptions(string sFilter, int tenantId, bool Typecodestatus);

        IEnumerable<DC.ExceptionDTO> GetBEQExceptions(string sFilter, int tenantId, bool Typecodestatus);

        DC.ExceptionDTO SaveExceptionComments(DC.ExceptionDTO ExceptionDet, int employeeId);

        IEnumerable<DC.ExceptionStatus> GetExceptionStatus();

        DC.ExceptionDTO SaveBEQExceptionComments(DC.ExceptionDTO Exception, int userId);

        DC.ExceptionDTO ResubmitException(DC.ExceptionDTO ExceptionDetails, int userId);
        // PDL
        DC.ExceptionDTO ResubmitExceptionToHttp(DC.ExceptionDTO ExceptionDetails, int userId);

        DC.ExceptionDTO BEQResubmitException(DC.ExceptionDTO ExceptionDetails, int userId);

        Tuple<DC.ResubmitBulkExceptionDTO, List<DC.ExceptionDTO>>  BulkResubmitException(List<DC.ExceptionDTO> ExceptionDetails, int userId);

        DC.BEQParseXMLDTO BEQParse(long documentObjectid);

        String GetMessageContent(long documentObjectid);

        bool BindMatch(DC.PotentialMatchDTO bindMatch, int Userid, string FileNotes);

        DC.BEQParseXMLDTO BEQParseParent(DC.ExceptionDTO parent);

        bool BEQBindAllOrder(string fileNumber, string fileID, DC.ExceptionDTO bindMatch, int userId, string FileNotes);

        bool BEQRejectOrder(DC.ExceptionDTO matchException, int userId, string fileNotes);

        IEnumerable<DC.ExceptionDTO> GetTEQExceptionByReferenceNum(DC.SearchDetail value, int tenantId);

        string BEQParentXml(int exceptionid);

        IEnumerable<DC.ExceptionDTO> GetBEQExceptionByReferenceNum(DC.SearchDetail details, int tenantId);

        bool BEQUnBindOrder(DC.PotentialMatchDTO bindMatch, int userId, int itenantid);

        IEnumerable<DC.ExceptionDTO> GetBEQExceptionsbyTypeName(string sFilter, int tenantId, bool typecodestatus, string exceptionType);

        IEnumerable<DC.ExceptionDTO> GetBEQExceptionsbyType(DC.SearchDetail details, int tenantId, string exceptionType);

        bool TEQRejectOrder(DC.ExceptionDTO matchException, int userId, string fileNotes);

        bool BEQUpdate(DC.ExceptionDTO matchException,string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid);

        bool BEQUpdateReject(DC.ExceptionDTO matchException,string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid);

        bool BEQDeleteOrderdetails(DC.ExceptionDTO matchException, int userId, string fileNotes);

        bool BEQCreateOrder(DC.ExceptionDTO matchException, int userId, int tenantid,string  FileNote);

        List<string> GetExceptionComments(DC.ExceptionDTO matchException);

        List<string> GetExceptionNotes(int Excpetionid);

        IEnumerable<DC.ExceptionDTO> GetTEQExceptionsbyTypeName(int tenantId, DC.SearchDetail value, string exceptionType, string sFilter, bool includeResolved);
        bool sendMail(string subject, string emailTo, string body);

        IEnumerable<DC.ExceptionDTO> GetTEQExceptionsbyCondition(string exceptionType,string status,string messagetype,int tenantId, DC.SearchDetail value);

        Tuple<DC.ResubmitBulkExceptionDTO, List<DC.ExceptionDTO>> BulkResolveException(string typeCodeId,List<DC.ExceptionDTO> ExceptionDetails, int userId);

        IEnumerable<DC.ExceptionType> GetExceptionList();

    }
}
