using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpMethodNotAllowedException : HttpClientException
    {
        public HttpMethodNotAllowedException(HttpResponseMessage response) : base(response)
        {
        }
    }
}