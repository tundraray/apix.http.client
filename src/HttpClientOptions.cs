using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apix.Http.Client
{
    public class HttpClientOptions
    {
        public Dictionary<string, string> Headers  { get; set; } = new Dictionary<string, string>()
        public bool ResetHeaders { get; set; } = false;
        public ProxySettings Proxy { get; set; }
    }
}
