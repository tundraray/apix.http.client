using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpUnsupportedMediaTypeException : HttpClientException
    {
        public HttpUnsupportedMediaTypeException(HttpResponseMessage response) : base(response)
        {
        }
    }
}