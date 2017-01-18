using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpNotImplementedException : HttpClientException
    {
        public HttpNotImplementedException(HttpResponseMessage response) : base(response)
        {
        }
    }
}