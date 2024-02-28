using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;

namespace FA.LVIS.Tower.Services
{
    public interface IExceptionService : Core.IServiceBase
    {
        IEnumerable<ExceptionDTO> GetTEQExceptions(SearchDetail Details,int tenantId);

        IEnumerable<ExceptionDTO> GetTEQExceptions(string sFilter,int iTenantid, bool Typecodestatus);

        IEnumerable<ExceptionDTO> GetBEQExceptions(SearchDetail Details, int tenantId);

        IEnumerable<ExceptionDTO> GetBEQExceptions(string sFilter, int iTenantid, bool Typecodestatus);

        IEnumerable<MessageLogDetailDTO> GetMessageDetails(int iExceptionid);

        BEQParseXMLDTO BEQParseParent(ExceptionDTO parent);

        bool BindMatch(PotentialMatchDTO bindMatch, int Userid, string FileNotes);

        BEQParseXMLDTO BEQParse(long documentObjectid);

        ExceptionDTO SaveExceptionComments(ExceptionDTO Exception, int employeeId);

        IEnumerable<ExceptionStatus> GetExceptionStatus();

        ExceptionDTO ResubmitException(ExceptionDTO exceptionDetails, int userId);

        ExceptionDTO BEQResubmitException(ExceptionDTO exceptionDetails, int userId);

        Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResubmitException(List<ExceptionDTO> ExceptionDetails, int userId);

        String GetMessageContent(long documentObjectid);

        bool BEQBindAllOrder(string fileNumber, string fileID, ExceptionDTO bindMatch, int userId, string FileNotes);

        ExceptionDTO SaveBEQExceptionComments(ExceptionDTO Exception, int userId);

        bool BEQUnBindOrder(PotentialMatchDTO bindMatch, int userId, int iTenantid);

        bool BEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes);

        bool BEQUpdateReject(ExceptionDTO matchException,string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid);

        IEnumerable<ExceptionDTO> GetTEQExceptionByReferenceNum(SearchDetail Details, int tenantId);

        IEnumerable<ExceptionDTO> GetBEQExceptionByReferenceNum(SearchDetail Details, int tenantId);

        string BEQParentXml(int exceptionid);

        IEnumerable<ExceptionDTO> GetBEQExceptionsbyTypeName(string sFilter, int tenantId, bool typecodestatus,string exceptionType);

        IEnumerable<ExceptionDTO> GetBEQExceptionsbyType(SearchDetail value, int tenantId, string exceptionType);

        bool TEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes);

        bool BEQUpdate(ExceptionDTO matchException,string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid);

        bool BEQDeleteOrderdetails(ExceptionDTO matchException, int userId, string fileNotes);

        bool BEQCreateOrder(ExceptionDTO matchException, int userId, int tenantid,string FileNote);

        List<string> GetExceptionComments(ExceptionDTO matchException);

        List<string> GetExceptionNotes(int exceptionid);

        IEnumerable<ExceptionDTO> GetTEQExceptionsbyTypeName(int tenantId, SearchDetail value, string exceptionType, string sFilter,bool includeResolved);

        bool SendMail(string subject, string emailTo, string body);

        Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResolveException(string typeCodeId,List<ExceptionDTO> ExceptionDetails, int userId);

        IEnumerable<ExceptionDTO> GetTEQExceptionsbyCondition(string exceptionType,string status,string messagetype,int tenantId, SearchDetail value);

        IEnumerable<ExceptionType> GetExceptionList();

    }
}
