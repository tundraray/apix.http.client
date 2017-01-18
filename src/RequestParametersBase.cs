using System.Threading;

namespace Apix.Http.Client
{
    public abstract class RequestParametersBase
    {
        /// <summary>Cancellation token</summary>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
