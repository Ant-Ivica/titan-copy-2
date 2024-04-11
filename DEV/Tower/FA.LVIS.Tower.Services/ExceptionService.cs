using System.Collections.Generic;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System;

namespace FA.LVIS.Tower.Services
{
    public class ExceptionService : Core.ServiceBase, IExceptionService
    {
        public IEnumerable<ExceptionDTO> GetTEQExceptions(string filter, int tenantId, bool Typecodestatus)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetTEQExceptions(filter,  tenantId,Typecodestatus);
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptions(SearchDetail Details, int tenantId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetTEQExceptions(Details, tenantId);
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptions(string filter, int tenantId, bool Typecodestatus)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetBEQExceptions(filter, tenantId, Typecodestatus);
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptions(SearchDetail Details, int tenantId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetBEQExceptions(Details, tenantId);
        }

        public ExceptionDTO BEQResubmitException(ExceptionDTO ExceptionDetails, int userId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQResubmitException(ExceptionDetails, userId);
        }

        public IEnumerable<MessageLogDetailDTO> GetMessageDetails(int exceptionId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetMessageDetails(exceptionId);
        }

        public ExceptionDTO SaveExceptionComments(ExceptionDTO ExceptionDet, int userId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().SaveExceptionComments(ExceptionDet, userId);
        }

        public IEnumerable<ExceptionStatus> GetExceptionStatus()
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetExceptionStatus();
        }

        public ExceptionDTO ResubmitException(ExceptionDTO ExceptionDetails, int userId)
        {
            if (ExceptionDetails.ExceptionTypeid == (int)Data.TerminalDBEntities.ExceptionTypeEnum.DomainNotConfigured)
            {
                //// resubmit to OpenApi/webhooks enpoint
                //WebhookAdapter webhookAdapter = new WebhookAdapter();
                //webhookAdapter.PostWebhook(ExceptionDetails.MessageContent);
                //return null;
                return DataProviderFactory.Resolve<IExceptionDataProvider>().ResubmitExceptionToHttp(ExceptionDetails, userId);
            }
            else
            {
                return DataProviderFactory.Resolve<IExceptionDataProvider>().ResubmitException(ExceptionDetails, userId);
            }
        }


        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResubmitException(List<ExceptionDTO> ExceptionDetails, int userId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BulkResubmitException(ExceptionDetails, userId);
        }


        public BEQParseXMLDTO BEQParse(long documentObjectid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQParse(documentObjectid);
        }

        public String GetMessageContent(long documentObjectid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetMessageContent(documentObjectid);
        }

        public bool BindMatch(PotentialMatchDTO bindMatch, int Userid, string FileNotes)
        {
           return DataProviderFactory.Resolve<IExceptionDataProvider>().BindMatch(bindMatch, Userid, FileNotes);
        }

        public BEQParseXMLDTO BEQParseParent(ExceptionDTO parent)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQParseParent(parent);
        }

        public bool BEQBindAllOrder(string fileNumber, string fileID, ExceptionDTO bindMatch, int userId, string FileNotes)
        {

            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQBindAllOrder(fileNumber, fileID,bindMatch, userId,FileNotes);
        }

        public ExceptionDTO SaveBEQExceptionComments(ExceptionDTO Exception, int userId)
        {

            return DataProviderFactory.Resolve<IExceptionDataProvider>().SaveBEQExceptionComments(Exception, userId);
        }

        public bool BEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQRejectOrder(matchException, userId, fileNotes);
        }

        public IEnumerable<ExceptionDTO> GetTEQExceptionByReferenceNum(SearchDetail Details, int tenantId)
        {

            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetTEQExceptionByReferenceNum(Details, tenantId);
        }


        public IEnumerable<ExceptionDTO> GetBEQExceptionByReferenceNum(SearchDetail Details, int tenantId)
        {

            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetBEQExceptionByReferenceNum(Details, tenantId);
        }

        public string BEQParentXml(int exceptionid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQParentXml(exceptionid);
        }

        public bool BEQUnBindOrder(PotentialMatchDTO bindMatch, int userId, int itenantid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQUnBindOrder(bindMatch, userId, itenantid);
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyTypeName(string sFilter, int tenantId, bool typecodestatus, string exceptionType)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetBEQExceptionsbyTypeName(sFilter, tenantId, typecodestatus, exceptionType);
        }

        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyType(SearchDetail value, int tenantId, string exceptionType)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetBEQExceptionsbyType(value, tenantId, exceptionType);
        }

        public bool TEQRejectOrder(ExceptionDTO matchException, int userId, string fileNotes)
        {
           return DataProviderFactory.Resolve<IExceptionDataProvider>().TEQRejectOrder(matchException, userId, fileNotes);
        }

        public bool BEQUpdateReject(ExceptionDTO matchException, string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQUpdateReject(matchException,externalRefnum, internalRefNum, internalRefId, userId, tenantid);

        }

        public bool BEQUpdate(ExceptionDTO matchException,string externalRefnum, string internalRefNum, int internalRefId, int userId, int tenantid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQUpdate(matchException,externalRefnum, internalRefNum, internalRefId, userId, tenantid);
        }

        public bool BEQDeleteOrderdetails(ExceptionDTO matchException, int userId, string fileNotes)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQDeleteOrderdetails(matchException, userId, fileNotes);
        }

        public bool BEQCreateOrder(ExceptionDTO matchException, int userId, int tenantid,string FileNote)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BEQCreateOrder(matchException, userId, tenantid, FileNote);
        }

        public List<string> GetExceptionComments(ExceptionDTO matchException)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetExceptionComments(matchException);
        }

        public List<string> GetExceptionNotes(int exceptionid)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetExceptionNotes(exceptionid);

        }

        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyTypeName(int tenantId, SearchDetail value, string exceptionType, string sFilter, bool includeResolved)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetTEQExceptionsbyTypeName(tenantId, value, exceptionType, sFilter,includeResolved);
        }

        public bool SendMail(string subject, string emailTo, string body)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().sendMail(subject, emailTo, body);
        }
        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResolveException(string typeCodeId,List<ExceptionDTO> ExceptionDetails, int userId)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().BulkResolveException(typeCodeId,ExceptionDetails, userId);
        }
        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyCondition(string exceptionType,string status,string messagetype,int tenantId, SearchDetail value)
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetTEQExceptionsbyCondition(exceptionType,status,messagetype,tenantId,value);
        }
        public IEnumerable<ExceptionType> GetExceptionList()
        {
            return DataProviderFactory.Resolve<IExceptionDataProvider>().GetExceptionList();
        }
    } 
}
