using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Apix.Http.Client.Exceptions
{
    public class HttpClientException: HttpRequestException
    {
        public HttpResponseMessage Response { get; set; }

        /// <summary>Response status code</summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return Response.StatusCode;
            }
        }

        /// <summary>Response reason phrase</summary>
        public string ReasonPhrase
        {
            get
            {
                return Response.ReasonPhrase;
            }
        }

        /// <summary>Response content</summary>
        public string Content { get; set; }

        /// <summary>Request message</summary>
        public HttpRequestMessage Request { get; set; }

        public HttpClientException(HttpResponseMessage response)
        {
            if (response == null)
                return;
            Response = response;
            Content = response.Content != null ? ReadAsString(response.Content.ReadAsByteArrayAsync().Result) : string.Empty;
            Request = response.RequestMessage;
        }

        private string ReadAsString(byte[] buffer)
        {
            var byteArray = buffer.ToArray();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
        }

        /// <summary>Get exception from response</summary>
        /// <param name="response">Internal response</param>
        /// <returns>Exception object</returns>
        public static HttpClientException FromResponse(HttpResponseMessage response)
        {
            if (response == null)
                return new HttpClientException(null);
            switch (response.StatusCode)
            {
                case (HttpStatusCode)429:
                    return new HttpTooManyRequestsException(response);
                case HttpStatusCode.InternalServerError:
                    return new HttpInternalServerErrorException(response);
                case HttpStatusCode.NotImplemented:
                    return new HttpNotImplementedException(response);
                case HttpStatusCode.ServiceUnavailable:
                    return new HttpServiceUnavailableException(response);
                case HttpStatusCode.BadRequest:
                    return new HttpBadReqestException(response);
                case HttpStatusCode.Unauthorized:
                    return new HttpUnauthorizedException(response);
                case HttpStatusCode.Forbidden:
                    return new HttpForbiddenException(response);
                case HttpStatusCode.NotFound:
                    return new HttpNotFoundException(response);
                case HttpStatusCode.MethodNotAllowed:
                    return new HttpMethodNotAllowedException(response);
                case HttpStatusCode.NotAcceptable:
                    return new HttpNotAcceptableException(response);
                case HttpStatusCode.RequestTimeout:
                    return new HttpRequestTimeoutException(response);
                case HttpStatusCode.Conflict:
                    return new HttpConflictException(response);
                case HttpStatusCode.Gone:
                    return new HttpGoneException(response);
                case HttpStatusCode.UnsupportedMediaType:
                    return new HttpUnsupportedMediaTypeException(response);
                case (HttpStatusCode)422:
                    return new HttpUnprocessableEntityException(response);
                default:
                    return new HttpClientException(response);
            }
        }

        /// <inheritdoc />
        public virtual string ToString()
        {
            string str = (Response == null || Response.RequestMessage == null ? 0 : (Response.RequestMessage.RequestUri != (Uri)null ? 1 : 0)) != 0 ? Response.RequestMessage.RequestUri.ToString() : string.Empty;
            if (Response == null)
                return Message;
            return string.Format("Message:{0}{1}{0}Url:{0}{2}{0}Response:{0}{3}{0}Response content:{0}{4}", (object)Environment.NewLine, (object)Message, (object)str, (object)Response, (object)Content);
        }
    }
}
