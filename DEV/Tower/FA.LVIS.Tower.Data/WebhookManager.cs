using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Data
{
    //please define all OpenAPI supported webhook actions here
    public static class WebHookActions
    {
        //public const string All = "*";
        public const string DocumentDelivery = "DocumentDelivery";
        public const string OrderCreated = "OrderCreated";
        public const string MessageAdded = "MessageAdded";
        public const string CurativeCleared = "CurativeCleared";
        public const string FundsDisbursed = "FundsDisbursed";
        public const string CurativeInfoPending = "CurativeInfoPending";

        public static bool IsValidAction(string action) => typeof(WebHookActions).GetFields().Select(x => x.GetValue(null).ToString())
                                             .Any(c => string.Equals(c, action, StringComparison.InvariantCultureIgnoreCase));
    }
    public class WebhookManager
    {

        private Microsoft.AspNet.WebHooks.Diagnostics.ILogger Logger = new Microsoft.AspNet.WebHooks.Diagnostics.TraceLogger();
        public IWebHookManager Manager { get; set; }
        public IWebHookStore Store { get; set; }

        internal const string SignatureHeaderKey = "sha256";
        internal const string SignatureHeaderValueTemplate = SignatureHeaderKey + "={0}";
        internal const string SignatureHeaderName = "ms-signature";

        internal const string TransmissionidHeaderName = "transmissionid";

        private const string BodyIdKey = "Id";
        private const string BodyAttemptKey = "RetryCount";
        //private const string BodyPropertiesKey = "Properties";
        private const string BodyNotificationsKey = "Notifications";
        private const string PropertyKey_Base64Cert = "Base64Cert";


        public WebhookManager()
        {
            Store = SqlWebHookStore.CreateStore(Logger, false);
            var webhookSender = new DataflowWebHookSender(Logger);
            Manager = new WebHookManager(Store, webhookSender, Logger);
        }

        public NotificationDictionary[] ComposeNotification(string action, string orderExternalTrackingId, string notification)
        {
            if (!WebHookActions.IsValidAction(action))
                throw new ArgumentOutOfRangeException(nameof(action), action, "invalid action");

            if (string.IsNullOrWhiteSpace(orderExternalTrackingId))
                throw new ArgumentOutOfRangeException(nameof(orderExternalTrackingId), action, "invalid order ExternalTrackingId");

            var notificationDictionary = new NotificationDictionary()
            {
                Action = action
            };

            var messageBodyJObject = JObject.Parse(notification);

            notificationDictionary["ExternalTrackingId"] = orderExternalTrackingId;
            notificationDictionary["Message"] = messageBodyJObject;

            return new NotificationDictionary[]
            {
                notificationDictionary
            };
        }

        public async Task<List<Tuple<bool, string,string>>> SendNotification(string appSource, string action, string orderExternalTrackingId, string notification)
        {
            var notifications = ComposeNotification(action, orderExternalTrackingId, notification);

            List<Tuple<bool, string, string>> results = new List<Tuple<bool, string, string>>();

            var webHooks = await Store.QueryWebHooksAsync(appSource, notifications.Select(n => n.Action), predicate: (wh, u) => wh.IsPaused == false);

            foreach (var webHook in webHooks)
            {
                webHook.Headers.Add(TransmissionidHeaderName, ComputeShaHashValue(notification));
            }

            var wkItems = GetWorkItems(webHooks, notifications);

            foreach (var item in wkItems)
            {
                results.Add(await LaunchWebHook(item));
            }

            return results;
        }

        public async Task<StoreResult> InsertWebhook(string appSource, string webhookUrl, string secret, string actionFilter, string base64Cert = null)
        {
            if (!WebHookActions.IsValidAction(actionFilter))
                throw new ArgumentOutOfRangeException(nameof(actionFilter), actionFilter, "invalid action filter");

            if (string.IsNullOrWhiteSpace(secret))
                throw new ArgumentException("Empty parameter", nameof(secret));

            var webhook = new WebHook
            {
                Secret = secret,
                WebHookUri = new Uri(webhookUrl),
            };

            // *  stands for match all
            webhook.Filters.Add(actionFilter);
            
            if (!string.IsNullOrWhiteSpace(base64Cert))
            {
                webhook.Properties.Add(PropertyKey_Base64Cert, base64Cert);
            }

            return await Store.InsertWebHookAsync(appSource, webhook);
        }

        // PDL added w/webhook
        public async Task<StoreResult> InsertWebhook(string appSource, WebHook webHook)
        {
            string actionFilter = webHook.Filters.FirstOrDefault();
            if (!WebHookActions.IsValidAction(actionFilter))
                throw new ArgumentOutOfRangeException(nameof(actionFilter), actionFilter, "invalid action filter");

            if (string.IsNullOrWhiteSpace(webHook.Secret))
                throw new ArgumentException("Empty parameter", nameof(webHook.Secret));

            // note: description can be empty isPaused and Uri are strongly typed and cannot be empty

            return await Store.InsertWebHookAsync(appSource, webHook);
        }


        public System.Security.Cryptography.X509Certificates.X509Certificate2 Webhook509Certificate(IDictionary<string,object> properties)
        {
            try
            {
                if (properties.ContainsKey(PropertyKey_Base64Cert) == false
                    || string.IsNullOrWhiteSpace(properties[PropertyKey_Base64Cert].ToString()))
                    return null;

                var base64Cert = properties[PropertyKey_Base64Cert].ToString(); 
                
                return new System.Security.Cryptography.X509Certificates.X509Certificate2(System.Convert.FromBase64String(base64Cert));
            }
            catch 
            {
            }
            return null;
        }

        public async Task<ICollection<WebHook>> GetWebhooks(string appSource)
        {
            return await Store.GetAllWebHooksAsync(appSource);
        }

        // PDL
        public async Task<WebHook> GetWebhook(string appSource, string id)
        {
            return await Store.LookupWebHookAsync(appSource, id);
        }


        public async Task RemoveAllWebhooks(string appSource)
        {
            await Store.DeleteAllWebHooksAsync(appSource);
        }

        public async Task<StoreResult> RemoveWebhook(string appSource, string webhookId)
        {
            return await Store.DeleteWebHookAsync(appSource, webhookId);
        }

        public async Task<StoreResult> UpdateWebHookAsync(string appSource, WebHook webHook)
        {
            string actionFilter = webHook.Filters.FirstOrDefault();
            if (!WebHookActions.IsValidAction(actionFilter))
                throw new ArgumentOutOfRangeException(nameof(actionFilter), actionFilter, "invalid action filter");


            return await Store.UpdateWebHookAsync(appSource, webHook);
        }

        private async Task<Tuple<bool, string, string>> LaunchWebHook(WebHookWorkItem workItem)
        {
            string requestString = string.Empty;
            string responseString = string.Empty;

            try
            {
                var authenticCert = Webhook509Certificate(workItem.WebHook.Properties);

                var request = CreateWebHookRequest(workItem);
                requestString = await HttpObjectToString(request);

                using (var _httpClient = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            //to validate "certificate" is same as the authentic one provided by client in Webhook definition.
                            if (workItem.WebHook.WebHookUri.Scheme == Uri.UriSchemeHttp)
                                return true;

                            if (authenticCert == null)
                                throw new ArgumentNullException("SSLCertificate", $"Invalid SSL Certificate for {workItem.WebHook.WebHookUri}");

                            if (string.Equals(certificate.GetPublicKeyString(), authenticCert.GetPublicKeyString(), StringComparison.OrdinalIgnoreCase))
                                return true;

                            return false;
                        };
                    
                    var response = await _httpClient.SendAsync(request);
                    responseString = await HttpObjectToString(response);
                    return new Tuple<bool, string, string>(response.IsSuccessStatusCode, $"{responseString} {Environment.NewLine} {Environment.NewLine} {requestString}", $"{responseString}");
                }
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, string>(false, $"{responseString} {Environment.NewLine} {Environment.NewLine} {requestString}", $"{ex}");
            }
        }

        private async Task<string> HttpObjectToString(dynamic httpObject)
        {
            var result = httpObject.ToString();
            if (httpObject.Content != null)
            {
                result += Environment.NewLine;
                result += await httpObject.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return result;
        }

        protected virtual HttpRequestMessage CreateWebHookRequest(WebHookWorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            WebHook hook = workItem.WebHook;

            // Create WebHook request
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, hook.WebHookUri);

            // Fill in request body based on WebHook and work item data
            JObject body = CreateWebHookRequestBody(workItem);
            SignWebHookRequest(workItem, request, body);

            // Add extra request or entity headers
            foreach (var kvp in hook.Headers)
            {
                if (!request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value))
                {
                    if (!request.Content.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value))
                    {
                        //string msg = string.Format(CultureInfo.CurrentCulture, CustomResources.Manager_InvalidHeader, kvp.Key, hook.Id);
                        //_logger.Error(msg);
                    }
                }
            }

            return request;
        }

        protected JObject CreateWebHookRequestBody(WebHookWorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            Dictionary<string, object> body = new Dictionary<string, object>();

            // Set properties from work item
            body[BodyIdKey] = workItem.Id;
            body[BodyAttemptKey] = workItem.Offset + 1;

            // Set properties from WebHook
            //Don't make public unuseful properties 
            //IDictionary<string, object> properties = workItem.WebHook.Properties;
            //if (properties != null)
            //{
            //    body[BodyPropertiesKey] = new Dictionary<string, object>(properties);
            //}

            // Set notifications
            body[BodyNotificationsKey] = workItem.Notifications;

            return JObject.FromObject(body);
        }

        protected virtual void SignWebHookRequest(WebHookWorkItem workItem, System.Net.Http.HttpRequestMessage request, JObject body)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }
            if (workItem.WebHook == null)
            {
                string msg = "WebHook";
                throw new ArgumentException(msg, "workItem");
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            byte[] secret = Encoding.UTF8.GetBytes(workItem.WebHook.Secret);
            using (var hasher = new System.Security.Cryptography.HMACSHA256(secret))
            {
                string serializedBody = body.ToString();
                request.Content = new System.Net.Http.StringContent(serializedBody, Encoding.UTF8, "application/json");

                byte[] data = Encoding.UTF8.GetBytes(serializedBody);
                byte[] sha256 = hasher.ComputeHash(data);
                //string headerValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, SignatureHeaderValueTemplate, EncodingUtilities.ToHex(sha256));
                string headerValue = string.Format(System.Globalization.CultureInfo.InvariantCulture, SignatureHeaderValueTemplate, Convert.ToBase64String(sha256));
                request.Headers.Add(SignatureHeaderName, headerValue);
            }
        }

        public static string ComputeShaHashValue(string originalContent, string key="FALVIS")
        {
            using (var hasher = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] data = Encoding.UTF8.GetBytes(originalContent);
                byte[] sha256 = hasher.ComputeHash(data);
                return Convert.ToBase64String(sha256);
            }
        }

        internal static IEnumerable<WebHookWorkItem> GetWorkItems(ICollection<WebHook> webHooks, ICollection<NotificationDictionary> notifications)
        {
            List<WebHookWorkItem> workItems = new List<WebHookWorkItem>();
            foreach (WebHook webHook in webHooks)
            {
                ICollection<NotificationDictionary> webHookNotifications;

                // Pick the notifications that apply for this particular WebHook. If we only got one notification
                // then we know that it applies to all WebHooks. Otherwise each notification may apply only to a subset.
                if (notifications.Count == 1)
                {
                    webHookNotifications = notifications;
                }
                else
                {
                    webHookNotifications = notifications.Where(n => webHook.MatchesAction(n.Action)).ToArray();
                    if (webHookNotifications.Count == 0)
                    {
                        continue;
                    }
                }

                WebHookWorkItem workItem = new WebHookWorkItem(webHook, webHookNotifications);
                workItems.Add(workItem);
            }
            return workItems;
        }

        public async Task<ICollection<WebHook>> GetFilteredActiveWebhooks(string appSource, string[] actions)
                => await Store.QueryWebHooksAsync(appSource, actions, predicate: (wh, u) => wh.IsPaused == false);
    }

}
