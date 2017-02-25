using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apix.Http.Client.Exceptions;

namespace Apix.Http.Client
{
    public class HttpClientBase : IHttpClient, IDisposable
    {
        private readonly Dictionary<string, string> _headers;

        #region Fields

        private bool _disposed;
        protected readonly HttpClient _client;

        #endregion

        #region Properties

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 10, 0);
        public HttpMessageHandler MessageHandler { get; private set; }
        public ProxySettings Proxy { get; set; }

        protected IHttpClient HttpClient => this;

        protected virtual Func<HttpResponseMessage, CancellationToken, Task> DefaultBadResponseAction
        {
            get
            {
                return (response, cancellationToken) =>
                {

                    throw HttpClientException.FromResponse(response);
                };
            }
        }

        #endregion

        #region Constructor

        public HttpClientBase(Dictionary<string, string> headers, bool clearHeader = false, ProxySettings proxy = null)
        {
            Timeout = new TimeSpan(0, 10, 0);
            MessageHandler = new HttpClientHandler();
            MessageHandler = new HttpClientHandler()
            {
                Proxy = proxy == null ? null : new WebProxy(proxy),
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None,
                UseProxy = proxy != null
            };

            _client = CreateClient(headers, clearHeader);
        }

        public HttpClientBase(HttpClientOptions options) : this(options.Headers, options.ResetHeaders, options.Proxy)
        {

        }

        public HttpClientBase(bool clearHeader = false, ProxySettings proxy = null) 
            : this(new Dictionary<string, string>(), clearHeader, proxy)
        {
        }

        #endregion


        protected HttpClient CreateClient(Dictionary<string, string> headers, bool clearHeader = false)
        {
            var httpClient = new HttpClient(MessageHandler);
            httpClient.Timeout = Timeout;

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }

            return httpClient;
        }


        public async Task RequestAsync(string methodCallString, object context, HttpMethod method, RequestParameters requestParameters)
        {
            await this.CallActionAsync(methodCallString, context, method, requestParameters ?? RequestParameters.Default);
        }

        public async Task<T> RequestAsync<T>(string methodCallString, object context, HttpMethod method, RequestParameters<T> requestParameters)
        {
            return await this.CallFunctionAsync<T>(methodCallString, context, method, requestParameters ?? RequestParameters<T>.Default);
        }

        public async Task<T> GetAsync<T>(string methodCallString, object context, RequestParameters<T> requestParameters)
        {
            return await RequestAsync<T>(methodCallString, context, HttpMethod.Get, requestParameters ?? RequestParameters<T>.Default);
        }

        public async Task GetAsync(string methodCallString, object context, RequestParameters requestParameters)
        {
            await RequestAsync(methodCallString, context, HttpMethod.Get, requestParameters ?? RequestParameters.Default);
        }

        public async Task<T> PostAsync<T>(string methodCallString, object context, RequestParameters<T> requestParameters)
        {
            return await RequestAsync<T>(methodCallString, context, HttpMethod.Post, requestParameters ?? RequestParameters<T>.Default);
        }

        public async Task PostAsync(string methodCallString, object context, RequestParameters requestParameters)
        {
            await RequestAsync(methodCallString, context, HttpMethod.Post, requestParameters ?? RequestParameters.Default);
        }


        public async Task<T> PutAsync<T>(string methodCallString, object context, RequestParameters<T> requestParameters)
        {
            return await RequestAsync<T>(methodCallString, context, HttpMethod.Put, requestParameters ?? RequestParameters<T>.Default);
        }

        public async Task PutAsync(string methodCallString, object context, RequestParameters requestParameters)
        {
            await RequestAsync(methodCallString, context, HttpMethod.Put, requestParameters ?? RequestParameters.Default);
        }

        public async Task<T> PatchAsync<T>(string methodCallString, object context, RequestParameters<T> requestParameters)
        {
            return await RequestAsync<T>(methodCallString, context, HttpHelpers.Patch, requestParameters ?? RequestParameters<T>.Default);
        }

        public async Task DeleteAsync(string methodCallString, object context, RequestParameters requestParameters)
        {
            await RequestAsync(methodCallString, context, HttpMethod.Delete, requestParameters ?? RequestParameters.Default);
        }

        public async Task<T> DeleteAsync<T>(string methodCallString, object context, RequestParameters<T> requestParameters)
        {
            return await this.RequestAsync<T>(methodCallString, context, HttpMethod.Delete, requestParameters ?? RequestParameters<T>.Default);
        }

        #region Make Request

        private async Task CallActionAsync(string methodUrl, object context, HttpMethod method, RequestParameters requestParameters)
        {
            HttpMethod httpMethod = method;
            if ((object)httpMethod == null)
                httpMethod = HttpMethod.Post;
            method = httpMethod;
            switch (method.GetRequestMethod())
            {
                case RequestMethods.GET:
                    methodUrl += GetQueryParameters(context);
                    await MakeGetRequestAsync(methodUrl, requestParameters);
                    break;
                case RequestMethods.POST:
                    await MakePostRequestAsync(methodUrl, context, requestParameters);
                    break;
                case RequestMethods.PUT:
                    await MakePutRequestAsync(methodUrl, context, requestParameters);
                    break;
                case RequestMethods.DELETE:
                    methodUrl += GetQueryParameters(context);
                    await MakeDeleteRequestAsync(methodUrl, requestParameters);
                    break;
                case RequestMethods.PATCH:
                    await MakePatchRequestAsync(methodUrl, context, requestParameters);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }

        private async Task<T> CallFunctionAsync<T>(string methodUrl, object context, HttpMethod method, RequestParameters<T> requestParameters)
        {
            HttpMethod httpMethod = method;
            if ((object)httpMethod == null)
                httpMethod = HttpMethod.Post;
            method = httpMethod;
            switch (method.GetRequestMethod())
            {
                case RequestMethods.GET:
                    methodUrl += GetQueryParameters(context);
                    return await MakeGetRequestAsync<T>(methodUrl, requestParameters);
                case RequestMethods.POST:
                    return await MakePostRequestAsync<T>(methodUrl, context, requestParameters);
                case RequestMethods.PUT:
                    return await MakePutRequestAsync<T>(methodUrl, context, requestParameters);
                case RequestMethods.DELETE:
                    methodUrl += GetQueryParameters(context);
                    return await MakeDeleteRequestAsync<T>(methodUrl, requestParameters);
                case RequestMethods.PATCH:
                    return await MakePatchRequestAsync<T>(methodUrl, context, requestParameters);
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }

        private async Task MakeGetRequestAsync(string methodUrl, RequestParameters requestParameters)
        {
            HttpResponseMessage response = await GetAsync(methodUrl, requestParameters.CancellationToken);
            try
            {
                await ProcessResponseAsync(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            response = null;
        }

        private async Task<T> MakeGetRequestAsync<T>(string methodUrl, RequestParameters<T> requestParameters)
        {
            T obj;
            using (HttpResponseMessage async = await GetAsync(methodUrl, requestParameters.CancellationToken))
                obj = await ProcessResponseAsync<T>(async, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            return obj;
        }

        private async Task MakePostRequestAsync(string methodUrl, object context, RequestParameters requestParameters)
        {
            HttpResponseMessage response = await PostAsync(methodUrl, context, requestParameters.CancellationToken);
            try
            {
                await ProcessResponseAsync(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            response = null;
        }

        private async Task<T> MakePostRequestAsync<T>(string methodUrl, object context, RequestParameters<T> requestParameters)
        {
            T obj;
            using (HttpResponseMessage response = await PostAsync(methodUrl, context, requestParameters.CancellationToken))
                obj = await ProcessResponseAsync<T>(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            return obj;
        }

        private async Task MakePutRequestAsync(string methodUrl, object context, RequestParameters requestParameters)
        {
            HttpResponseMessage response = await PutAsync(methodUrl, context, requestParameters.CancellationToken);
            try
            {
                await ProcessResponseAsync(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            response = null;
        }

        private async Task<T> MakePutRequestAsync<T>(string methodUrl, object context, RequestParameters<T> requestParameters)
        {
            T obj;
            using (HttpResponseMessage response = await PutAsync(methodUrl, context, requestParameters.CancellationToken))
                obj = await ProcessResponseAsync<T>(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            return obj;
        }

        private async Task MakeDeleteRequestAsync(string methodUrl, RequestParameters requestParameters)
        {
            HttpResponseMessage response = await DeleteAsync(methodUrl, requestParameters.CancellationToken);
            try
            {
                await ProcessResponseAsync(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            response = null;
        }

        private async Task<T> MakeDeleteRequestAsync<T>(string methodUrl, RequestParameters<T> requestParameters)
        {
            T obj;
            using (HttpResponseMessage response = await DeleteAsync(methodUrl, requestParameters.CancellationToken))
                obj = await ProcessResponseAsync<T>(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            return obj;
        }

        private async Task MakePatchRequestAsync(string methodUrl, object context, RequestParameters requestParameters)
        {
            HttpResponseMessage response = await PatchAsync(methodUrl, context, requestParameters.CancellationToken);
            try
            {
                await ProcessResponseAsync(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            }
            finally
            {
                if (response != null)
                    response.Dispose();
            }
            response = null;
        }

        private async Task<T> MakePatchRequestAsync<T>(string methodUrl, object context, RequestParameters<T> requestParameters)
        {
            T obj;
            using (HttpResponseMessage response = await PatchAsync(methodUrl, context, requestParameters.CancellationToken))
                obj = await ProcessResponseAsync<T>(response, requestParameters.OnSuccess, requestParameters.OnError, requestParameters.CancellationToken);
            return obj;
        }

        private Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, object onSuccess, object onError, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                return DefaultHandleResponseFunctionAsync<T>(response, cancellationToken);
            }
            return DefaultBadResponseFunctionAsync<T>(response);
        }

        #endregion

        #region Request Methods


        private Task<HttpResponseMessage> PatchAsync<T>(string requestUri, T value, CancellationToken cancellationToken)
        {

            HttpContent httpContent = (object)value is HttpContent ? (object)value as HttpContent : new JsonContent<T>(value);
            HttpRequestMessage request = new HttpRequestMessage(HttpHelpers.Patch, new Uri(requestUri))
            {
                Content = httpContent
            };
            return _client.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUri));
            return _client.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value, CancellationToken cancellationToken)
        {
            HttpContent httpContent = (object)value is HttpContent ? (object)value as HttpContent : new JsonContent<T>(value);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri))
            {
                Content = httpContent
            };
            return _client.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> PutAsync<T>(string requestUri, T value, CancellationToken cancellationToken)
        {
            HttpContent httpContent = (object)value is HttpContent ? (object)value as HttpContent : new JsonContent<T>(value);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, new Uri(requestUri))
            {
                Content = httpContent
            };
            return _client.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, new Uri(requestUri));
            return _client.SendAsync(request, cancellationToken);
        }

        #endregion

        #region Methods

        private static string GetQueryParameters(object parameters)
        {
            if (parameters != null && parameters is UrlParameterCollection)
                return "?" + ((UrlParameterCollection)parameters).ToQueryString(true);
            return "";
        }

        private async Task ProcessResponseAsync(HttpResponseMessage response, Func<HttpResponseMessage, CancellationToken, Task> onSuccess, Func<HttpResponseMessage, CancellationToken, Task> onError, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                if (onSuccess == null)
                    return;
                await onSuccess(response, cancellationToken);
            }
            else
            {
                onError = onError ?? DefaultBadResponseAction;
                await onError(response, cancellationToken);
            }
        }

        #endregion

        #region Dispose 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected virtual methods
        /// <summary>
        /// Default bad response action
        /// </summary>
        protected virtual Task<T> DefaultBadResponseFunctionAsync<T>(HttpResponseMessage response)
        {
            var tcs = new TaskCompletionSource<T>();
            ThreadPool.QueueUserWorkItem(_ => tcs.SetException(HttpClientException.FromResponse(response)));
            return tcs.Task;
        }
        /// <summary>
        /// Default async handle response function
        /// </summary>
        /// <typeparam name="T">Content type</typeparam>
        /// <param name="response">Input response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Output value</returns>
        protected virtual Task<T> DefaultHandleResponseFunctionAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return response.Content.ReadAsAsync<T>(new[] { new JsonMediaTypeFormatter() { SupportedEncodings = { Encoding.GetEncoding("windows-1251"), Encoding.UTF8 } } }, cancellationToken);
        }
        #endregion

    }
}
