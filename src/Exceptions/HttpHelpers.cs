using System;
using System.Net.Http;

namespace Apix.Http.Client
{
    public static class HttpHelpers
    {

        public static HttpMethod Patch
        {
            get
            {
                return new HttpMethod("PATCH");
            }
        }

        public static RequestMethods GetRequestMethod(this HttpMethod method)
        {
            RequestMethods result;
            if (Enum.TryParse(method.Method, true, out result))
                return result;
            throw new HttpRequestException("Unknown method type.");
        }

        public static HttpMethod GetHttpMethod(string method)
        {
            if (string.IsNullOrEmpty(method))
                return null;
            if (string.Equals("GET", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Get;
            if (string.Equals("POST", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Post;
            if (string.Equals("PUT", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Put;
            if (string.Equals("DELETE", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Delete;
            if (string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Head;
            if (string.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Options;
            if (string.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase))
                return HttpMethod.Trace;
            return new HttpMethod(method);
        }
    }
}
