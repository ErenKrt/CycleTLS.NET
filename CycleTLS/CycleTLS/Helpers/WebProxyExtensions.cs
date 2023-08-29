using System.Net;

namespace CycleTLS.Helpers
{
    public static class WebProxyExtensions
    {
        public static string toStringWithCredentials(this WebProxy proxy)
        {
            var proxyURL = proxy.GetProxy(new Uri("http://example.com"));
            NetworkCredential proxyCreds = (NetworkCredential)proxy.Credentials;

            return string.Format("{0}://{1}:{2}@{3}",
                         proxyURL.Scheme,
                         proxyCreds.UserName,
                         proxyCreds.Password,
                         proxyURL.Authority);
        }
    }
}
