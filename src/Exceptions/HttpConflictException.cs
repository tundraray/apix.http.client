using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    internal class HttpConflictException : HttpClientException
    {
        public HttpConflictException(HttpResponseMessage response) : base(response)
        {
        }
    }
}