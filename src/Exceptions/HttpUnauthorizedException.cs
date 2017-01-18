using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpUnauthorizedException : HttpClientException
    {
        public HttpUnauthorizedException(HttpResponseMessage response) : base(response)
        {
        }
    }
}