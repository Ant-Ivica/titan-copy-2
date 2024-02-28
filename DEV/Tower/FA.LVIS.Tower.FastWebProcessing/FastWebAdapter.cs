using FA.LVIS.CommonHelper;
using LVIS.Common;
using LVIS.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.FastWebProcessing
{
    public class FastWebAdapter
    {
        readonly Utils utils = new Utils();
        private static ILogger sLogger = LoggerFactory.GetLogger(typeof(FastWebAdapter));

        public string SendFastWebResponse(string json)
        {
            var FastWebEndPoint = ConfigurationManager.AppSettings["FastWebOrderDetailsURL"];
            var credentials = ConfigurationManager.AppSettings["credentials"].Decrypt();

            sLogger.Debug($"Posting to FastWeb Endpoint: {FastWebEndPoint}");
            var response = ProcessPOSTRequestInternal(FastWebEndPoint, json, credentials);
            sLogger.Debug($"FastWeb Response: {response}");
            return response;
        }

        private string ProcessPOSTRequestInternal(string serviceURL, string requestJSON, string credentials)
        {
            string ResponseJSON = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                using (var content = new StringContent(requestJSON, Encoding.UTF8, "application/json"))
                {
                    try
                    {
                        var response = client.PostAsync(serviceURL, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            sLogger.Debug($"Received success response from FastWeb Endpoint");
                            ResponseJSON = response.Content.ReadAsStringAsync().Result;
                            return ResponseJSON;
                        }
                        else
                        {
                            sLogger.Debug($"Received failure response from FastWeb Endpoint");
                            string errordata = (!string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result)) ? response.Content.ReadAsStringAsync().Result : response.ReasonPhrase;
                            throw new System.Exception(errordata);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }

            }
        }
    }
}
