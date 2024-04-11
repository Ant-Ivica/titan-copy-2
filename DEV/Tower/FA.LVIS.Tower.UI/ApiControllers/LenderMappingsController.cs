using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Services;
using FA.LVIS.Tower.Data;
using System.Web.Mvc;
using FA.LVIS.Tower.UI.Identity.Provider;
using System.Web;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    public class LenderMappingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //private ApplicationServiceClient webApiServiceClient;

        //public LenderMappingsController()
        //{
        //    webApiServiceClient = new ApplicationServiceClient("http://localhost:61078/api/lenders");
        //}

        [AllowAnonymous]
        public ActionResult Lender()
        {
            //var result = webApiServiceClient.Get<string>("getclaimsinfo"); // webApiProtectedResource(AccessToken);

            HttpCookie cookie = new HttpCookie(".AspNet.ApplicationCookie");
            cookie = Request.Cookies[".AspNet.ApplicationCookie"];

            //var cookie = request.Cookies.Get(".AspNet.ApplicationCookie");
            var ticket = cookie.Value;
            ticket = ticket.Replace('-', '+').Replace('_', '/');

            var padding = 3 - ((ticket.Length + 3) % 4);
            if (padding != 0)
                ticket = ticket + new string('=', padding);

            var bytes = Convert.FromBase64String(ticket);

            bytes = System.Web.Security.MachineKey.Unprotect(bytes,
                "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
                    "ApplicationCookie", "v1");
            StringBuilder sb = new StringBuilder();


            using (var memory = new MemoryStream(bytes))
            {
                using (var compression = new GZipStream(memory,
                                                    CompressionMode.Decompress))
                {
                    using (var reader = new BinaryReader(compression))
                    {
                        reader.ReadInt32();
                        string authenticationType = reader.ReadString();
                        reader.ReadString();
                        reader.ReadString();

                        int count = reader.ReadInt32();

                        var claims = new Claim[count];
                        for (int index = 0; index != count; ++index)
                        {
                            string type = reader.ReadString();
                            type = type == "\0" ? ClaimTypes.Name : type;

                            string value = reader.ReadString();

                            string valueType = reader.ReadString();
                            valueType = valueType == "\0" ?
                                           "http://www.w3.org/2001/XMLSchema#string" :
                                             valueType;

                            string issuer = reader.ReadString();
                            issuer = issuer == "\0" ? "LOCAL AUTHORITY" : issuer;

                            string originalIssuer = reader.ReadString();
                            originalIssuer = originalIssuer == "\0" ?
                                                         issuer : originalIssuer;

                            claims[index] = new Claim(type, value,
                                                   valueType, issuer, originalIssuer);

                            sb.AppendLine(" ClaimType: " + type + ", ClaimValue: " + value).AppendLine();
                        }

                        var result = sb.ToString();
                        ViewBag.ActualRights = result;
                        Response.Write(result);

                        return View();
                    }
                }
            }
        }

        // GET: api/LenderMappings/5
        public string Get(int id)
        {
            return "value";
        }

        // DELETE: api/LenderMappings/5
        public void Delete(int id)
        {
        }
    }
}
