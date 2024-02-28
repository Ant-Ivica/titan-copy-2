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
    [RoutePrefix("InboundDocs")]

    [CustomAuthorize]
    public class InDocumentMappingsController : ApiController
    {
        [Route("GetInboundDocs", Name = "GetInboundDocs")]
        [HttpGet]
        public IEnumerable<InboundDocumentMapDTO> Get()
        {
            AuditLogHelper.sSection = "Mappings\\Inbound Document Mapping\\GetInboundDocs";
             IInboundDocMappingService DocService = ServiceFactory.Resolve<IInboundDocMappingService>();
            List<InboundDocumentMapDTO> newList = new List <InboundDocumentMapDTO> ();

            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            newList = DocService.GetLvisInboundDocMaps(tenantId);
            return newList;
        }
        
        [Route("GetDocTypes/{ApplicationId}", Name = "GetDocTypes")]
        [HttpGet]
        public IEnumerable<DocumentType> Get(int ApplicationId)
        {
            AuditLogHelper.sSection = "Mappings\\ Inbound Document Mapping\\GetDocTypes";
            IInboundDocMappingService DocService = ServiceFactory.Resolve<IInboundDocMappingService>();
            return DocService.GetDocTypes(ApplicationId);
            
        }

        [Route("GetServices", Name = "GetServices")]
        [HttpGet]
        public IEnumerable<Service> GetServices()
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Mappings\\Inbound Document Mapping\\GetServices";
            IInboundDocMappingService DocService = ServiceFactory.Resolve<IInboundDocMappingService>();
            return DocService.GetServices(tenantId);
        }

        // GET: api/OutDocumentMappings  
        ////[Route("AddIboundDoc", Name = "AddIboundDoc")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public InboundDocumentMapDTO AddDocs(InboundDocumentMapDTO Doc)
        {
            AuditLogHelper.sSection = "Mappings\\Inbound Document Mapping\\AddIboundDoc";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;

            
            IInboundDocMappingService outDoc = ServiceFactory.Resolve<IInboundDocMappingService>();
            return outDoc.AddDoc(Doc, tenantId, userId);

        }


        ////[Route("UpdateInboundDoc")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public InboundDocumentMapDTO UpdateInboundDoc(InboundDocumentMapDTO Doc)
        {
            AuditLogHelper.sSection = "Mappings\\Inbound Document Mapping\\UpdateInboundDoc";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;

            
            IInboundDocMappingService outDoc = ServiceFactory.Resolve<IInboundDocMappingService>();
            return outDoc.UpdateDoc(Doc, tenantId, userId);
        }

        [Route("DeleteInboundDoc", Name = "DeleteInboundDoc")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public int DeleteInboundDoc([FromBody]int value)
        {
            AuditLogHelper.sSection = "Mappings\\Inbound Document Mapping\\DeleteInboundDoc";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault() != null) ?
           Convert.ToInt32(claims.Where(c => c.Type == Constants.USER_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IInboundDocMappingService>().DeleteDocument(value, userId);
        }
    }
}
