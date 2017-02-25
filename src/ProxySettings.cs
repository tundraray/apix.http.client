using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apix.Http.Client
{
    public class ProxySettings
    {
        public string Address { get; set; }
        public bool UseHttps { get; set; } = false
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
