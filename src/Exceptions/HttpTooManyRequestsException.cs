using System.Net.Http;

namespace Apix.Http.Client.Exceptions
{
    public class HttpTooManyRequestsException : HttpClientException
    {
        /// <inheritdoc />
        public HttpTooManyRequestsException(HttpResponseMessage response)
            : base(response)
        {
        }
    }

}