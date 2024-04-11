using System.Collections.Generic;
using System.Web.Http;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Services;
using System;
using System.Linq;
using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("FastDocs")]

    [CustomAuthorize]
    public class FastDocumentMappingsController : ApiController
    {
        [Route("FASTToLVISDocs", Name = "FASTToLVISDocs")]
        [HttpGet]
        public IEnumerable<InboundDocumentMapDTO> Get()
        {
            AuditLogHelper.sSection = "Mappings\\FAST To LVIS DocumentMapping\\FASTToLVISDocs";
            IFASTDocMappingService DocService = ServiceFactory.Resolve<IFASTDocMappingService>();
            List<InboundDocumentMapDTO> newList = new List <InboundDocumentMapDTO> ();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = DocService.GetFASTToLVISDocs(tenantId);
            return newList;
        }

        [Route("LVISToFastDocs", Name = "LVISToFastDocs")]
        [HttpGet]
        public IEnumerable<InboundDocumentMapDTO> LVISToFastDocs()
        {
            AuditLogHelper.sSection = "Mappings\\ LVIS ToFAST DocumentMapping\\LVISToFastDocs";
            IFASTDocMappingService DocService = ServiceFactory.Resolve<IFASTDocMappingService>();
            List<InboundDocumentMapDTO> newList = new List<InboundDocumentMapDTO>();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = DocService.GetLVISToFastDocs(tenantId);
            return newList;
        }

        // GET: api/OutDocumentMappings  
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public InboundDocumentMapDTO AddDoc(InboundDocumentMapDTO Doc)
        {
            AuditLogHelper.sSection = "Mappings\\ LVIS To FAST Document Mapping\\AddDoc";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;


            IFASTDocMappingService DocService = ServiceFactory.Resolve<IFASTDocMappingService>();
            return DocService.AddDoc(Doc, tenantId, userId);

        }

        
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public InboundDocumentMapDTO UpdateDoc(InboundDocumentMapDTO Doc)
        {
            AuditLogHelper.sSection = "Mappings\\ Inbound Document Mapping\\UpdateDoc";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;


            IFASTDocMappingService DocService = ServiceFactory.Resolve<IFASTDocMappingService>();
            return DocService.UpdateDoc(Doc, tenantId, userId);
        }
    }
}
