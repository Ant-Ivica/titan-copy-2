using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.FASTProcessing;
using FA.LVIS.Tower.UI.ApiControllers.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("FastWebOrdersController")]
    [CustomAuthorize]
    public class FastWebOrdersController : ApiController
    {
        readonly private string userID = ConfigurationManager.AppSettings["FastWebUserID"];

        [Authorize(Roles = "SuperAdmin")]
        [Route("FastWebOrders", Name = "FastWebOrders")]
        [HttpPost]
        public FastWebOrderDetailsCanonicalDTO[] SearchFastWebOrders()
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
            {
                FastWebProcessing.FastWebClientData Clientdata = new FastWebProcessing.FastWebClientData();
                return Clientdata.GetFastWebOrderDetails().ToArray();
            }
            else
                return null;
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("GetFastWeborderDetail/{servicename}", Name = "GetFastWeborderDetail")]
        [HttpPost]
        public FastWebOrderDetailsDTO GetFastWeborderDetail([FromBody] int fastwebnum, string servicename)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
            {
                FastWebProcessing.FastWebOrderDetails Clientdata = new FastWebProcessing.FastWebOrderDetails();
                return Clientdata.GetFastWebOrderDetails(fastwebnum, servicename);
            }
            else
                return null;
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("AddForwardToOffice", Name = "AddForwardToOffice")]
        [HttpPost]
        public string AddForwardToOfc([FromBody] ADD_FORWARD_TO_OFFICE_REQEUST AddForwardOfc)
        {
            try
            {

                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
                string response = string.Empty;
                FastWebProcessing.FastWebOrderDetails Clientdata = new FastWebProcessing.FastWebOrderDetails();

                int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

                if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
                    response = Clientdata.AddForwardToOfc(AddForwardOfc);
                return response;
            }
            catch (System.Exception ex)
            {
                throw new LVISCustom(ex.Message);
            }

        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("GetFastWebOrdersBySearchType/{SearchType:maxlength(1)}", Name = "GetFastWebOrdersBySearchType")]
        [HttpPost]
        public FastWebOrderDetailsCanonicalDTO[] GetFastWebOrdersBySearchType([FromBody]string[] SearchText, string SearchType)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
            {
                FastWebProcessing.FastWebClientData Clientdata = new FastWebProcessing.FastWebClientData();
                return Clientdata.GetFastWebOrdersBySearchType(SearchText?[0], SearchType).ToArray();
            }
            else
                return null;
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("GetFastWebOffices", Name = "GetFastWebOffices")]
        [HttpPost]
        public Office[] GetFastWebOffices([FromBody] string fastwebnum)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                        Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
            {
                FastWebProcessing.FastWebClientData fastWebClientData = new FastWebProcessing.FastWebClientData();
                return fastWebClientData.GetfastWebOffices(fastwebnum);
            }
            else
                return new Office[0];
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("SubmitFastWebOffice/{fastwebnum}/{servicename}", Name = "SubmitFastWebOffice")]
        [HttpPost]
        public void SubmitFastWebOffice([FromBody]string userID, string fastwebnum, string servicename)
        {
            try
            {
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

                if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
                {
                    FastWebProcessing.FastWebClientData fastWebClientData = new FastWebProcessing.FastWebClientData();
                    fastWebClientData.SubmitForwardToOffice(userID, fastwebnum, servicename);
                }
            }
            catch (System.Exception ex)
            {
                throw new LVISCustom(ex.Message);
            }

           
        }


        [Authorize(Roles = "SuperAdmin")]
        [Route("GetFastWebUser", Name = "GetFastWebUser")]
        [HttpGet]
        public string GetFastWebUser()
        {
            try
            {
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                int Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

                if (Tenantid == (int)TenantIdEnum.LVIS || Tenantid == (int)TenantIdEnum.AirTrafficControl)
                {
                    return userID;
                }
            }
            catch (System.Exception ex)
            {
                throw new LVISCustom(ex.Message);
            }
            return "";
        }
    }
}

