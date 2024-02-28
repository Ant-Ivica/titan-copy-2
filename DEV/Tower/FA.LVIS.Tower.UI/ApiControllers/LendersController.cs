﻿using System;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

using FA.LVIS.Tower.UI.ApiControllers.Filters;

namespace FA.LVIS.Tower.UI.ApiControllers
{
    public class Lender
    {
        public string LvisABEID { get; set; }
        public string LenderName { get; set; }
    }

    [CustomAuthorize]
    public class LendersController : ApiController
    {
        [HttpGet]
        public string GetClaimsInfo()
        {
            HttpCookie cookie = new HttpCookie(".AspNet.ApplicationCookie");
            cookie = HttpContext.Current.Request.Cookies[".AspNet.ApplicationCookie"];

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

            sb.AppendLine(System.Environment.MachineName);

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

                            sb.AppendLine(" ClaimType: " + type + ", ClaimValue: " + value);
                            sb.AppendLine("");
                        }


                        return sb.ToString();

                        //var identity = new ClaimsIdentity(claims, authenticationType,
                        //                              ClaimTypes.Name, ClaimTypes.Role);

                        //var principal = new ClaimsPrincipal(identity);

                        //System.Threading.Thread.CurrentPrincipal = principal;
                        //HttpContext.Current.User = principal;
                    }
                }
            }
        }

        // GET: api/Lenders/5
        public string Get(int id)
        {
            return "value";
        }

        // DELETE: api/Lenders/5
        public void Delete(int id)
        {
        }
    }
}
