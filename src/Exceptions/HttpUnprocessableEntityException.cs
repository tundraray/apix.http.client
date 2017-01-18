using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpUnprocessableEntityException : HttpClientException
    {
        public HttpUnprocessableEntityException(HttpResponseMessage response) : base(response)
        {
        }
    }
}