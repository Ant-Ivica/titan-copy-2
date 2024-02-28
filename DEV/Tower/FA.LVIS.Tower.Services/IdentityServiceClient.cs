using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Services
{
    public sealed class IdentityServiceClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the base request URI.
        /// </summary>
        /// <value>
        /// The base request URI.
        /// </value>
        private string BaseRequestUri { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        private HttpClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServiceClient"/> class.
        /// </summary>
        /// <param name="requestUri">Uri to WebAPI Controller ex:http://localhost/api/Files</param>
        public IdentityServiceClient(string requestUri)
        {
            Init(requestUri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServiceClient"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="accessToken">The access token.</param>
        public IdentityServiceClient(string requestUri, string accessToken)
        {
            Init(requestUri, accessToken);
        }

        /// <summary>
        /// Initializes the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="accessToken">The access token.</param>
        private void Init(string requestUri, string accessToken = null)
        {
            BaseRequestUri = requestUri;
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }


        #region Post
        /// <summary>
        /// Posts the specified action.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="data">The data.</param>
        /// <returns>The result data</returns>
        public TResult Post<TResult, T>(string action, T data)
        {
            var requestUri = string.Format("{0}/{1}", this.BaseRequestUri, action);
            return PostAsync<TResult, T>(requestUri, data).Result;
        }

        /// <summary>
        /// Posts the asynchronous data to requestUri
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="data">The data.</param>
        /// <returns>Task result</returns>
        private async Task<TResult> PostAsync<TResult, T>(string requestUri, T data)
        {
            var response = await Client.PostAsJsonAsync(requestUri, data).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<TResult>();
            return result;
        }
        #endregion

        #region Get
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T Get<T>(string id)
        {
            var requestUri = string.Format("{0}/{1}", this.BaseRequestUri, id);
            return GetAsync<T>(requestUri).Result;
        }

        /// <summary>
        /// Gets the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T Get<T>(string action, string id)
        {
            var requestUri = string.Format("{0}/{1}/{2}", this.BaseRequestUri, action, id);
            return GetAsync<T>(requestUri).Result;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <returns></returns>
        private async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await Client.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<T>();
            return result;
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }
        }
    }
}