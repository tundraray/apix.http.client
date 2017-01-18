using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpForbiddenException : HttpClientException
    {
        public HttpForbiddenException(HttpResponseMessage response) : base(response)
        {
        }
    }
}