using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Apix.Extensions;

namespace Apix.Http.Client
{
    public class WebProxy : IWebProxy
    {
        public WebProxy(ProxySettings settings)
        {
            var protocol = settings.UseHttps ? "https" : "http";
            ProxyUri = new Uri($"{protocol}://{settings.Address}:{settings.Port}");
            if (settings.Password.IsNotNullOrEmpty() || settings.UserName.IsNotNullOrEmpty())
            {
                Credentials = new NetworkCredential(settings.UserName,settings.Password);
            }
        }

        public Uri ProxyUri { get; set; }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return this.ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false; /* Proxy all requests */
        }
    }
}
