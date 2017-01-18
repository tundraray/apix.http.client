using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpBadReqestException : HttpClientException
    {
        public HttpBadReqestException(HttpResponseMessage response) : base(response)
        {
        }
    }
}