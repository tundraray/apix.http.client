using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpRequestTimeoutException : HttpClientException
    {
        public HttpRequestTimeoutException(HttpResponseMessage response) : base(response)
        {
        }
    }
}