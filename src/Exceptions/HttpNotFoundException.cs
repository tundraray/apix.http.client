using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpNotFoundException : HttpClientException
    {
        public HttpNotFoundException(HttpResponseMessage response) : base(response)
        {
        }
    }
}