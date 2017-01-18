using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apix.Http.Client
{
    public sealed class RequestParameters : RequestParametersBase
    {
        /// <summary>Gets or sets the handle response action.</summary>
        /// <value>The handle response action.</value>
        public Func<HttpResponseMessage, CancellationToken, Task> OnSuccess { get; set; }

        /// <summary>Gets or sets the bad response action.</summary>
        /// <value>The bad response action.</value>
        public Func<HttpResponseMessage, CancellationToken, Task> OnError { get; set; }

        internal static RequestParameters Default
        {
            get
            {
                RequestParameters requestParameters = new RequestParameters();
                CancellationToken none = CancellationToken.None;
                requestParameters.CancellationToken = none;
                return requestParameters;
            }
        }
    }

    public sealed class RequestParameters<T> : RequestParametersBase
    {
        /// <summary>Handle success response function</summary>
        public Func<HttpResponseMessage, CancellationToken, Task<T>> OnSuccess { get; set; }

        /// <summary>Handle bad response function</summary>
        public Func<HttpResponseMessage, CancellationToken, Task<T>> OnError { get; set; }

        internal static RequestParameters<T> Default
        {
            get
            {
                RequestParameters<T> requestParameters = new RequestParameters<T>();
                CancellationToken none = CancellationToken.None;
                requestParameters.CancellationToken = none;
                return requestParameters;
            }
        }
    }
}
