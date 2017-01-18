using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpInternalServerErrorException : HttpClientException
    {
        public HttpInternalServerErrorException(HttpResponseMessage response) : base(response)
        {
        }
    }
}