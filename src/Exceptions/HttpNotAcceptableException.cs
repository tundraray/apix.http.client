using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpNotAcceptableException : HttpClientException
    {
        public HttpNotAcceptableException(HttpResponseMessage response) : base(response)
        {
        }
    }
}