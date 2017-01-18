using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpServiceUnavailableException : HttpClientException
    {
        public HttpServiceUnavailableException(HttpResponseMessage response) : base(response)
        {
        }
    }
}