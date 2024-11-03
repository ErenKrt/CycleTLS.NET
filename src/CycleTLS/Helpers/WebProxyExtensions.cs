using System;
using System.Net;

namespace CycleTLS.Helpers
{
    public static class WebProxyExtensions
    {
        public static string toStringWithCredentials(this WebProxy proxy)
        {
            var proxyURL = proxy.GetProxy(new Uri("http://example.com"));
            var creds = proxy.Credentials as NetworkCredential;

            return creds != null
                ? $"{proxyURL.Scheme}://{creds.UserName}:{creds.Password}@{proxyURL.Authority}"
                : $"{proxyURL.Scheme}://{proxyURL.Authority}";
        }
    }
}
