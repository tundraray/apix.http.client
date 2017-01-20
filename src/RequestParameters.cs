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

        internal static RequestParameters Default => new RequestParameters { CancellationToken = CancellationToken.None };
    }

    public sealed class RequestParameters<T> : RequestParametersBase
    {
        /// <summary>
        /// Handle success response function
        /// </summary>
        public Func<HttpResponseMessage, CancellationToken, Task<T>> OnSuccess { get; set; }

        /// <summary>
        /// Handle bad response function
        /// </summary>
        public Func<HttpResponseMessage, CancellationToken, Task<T>> OnError { get; set; }

        internal static RequestParameters<T> Default => new RequestParameters<T> { CancellationToken = CancellationToken.None };
    }
}
