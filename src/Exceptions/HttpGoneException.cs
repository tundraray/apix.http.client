using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpGoneException : HttpClientException
    {
        public HttpGoneException(HttpResponseMessage response) : base(response)
        {
        }
    }
}