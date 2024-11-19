using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Reflection;
using RestSharp;
using System.Linq;

namespace CycleTLS.Helpers
{
    public static class HelperExtensions
    {
        public static string ToStringWithCredentials(this IWebProxy proxy)
        {
            var proxyURL = proxy.GetProxy(new Uri("http://example.com"));
            var creds = proxy.Credentials as NetworkCredential;

            return creds != null
                ? $"{proxyURL.Scheme}://{creds.UserName}:{creds.Password}@{proxyURL.Authority}"
                : $"{proxyURL.Scheme}://{proxyURL.Authority}";
        }

        public static IEnumerable<Cookie> GetAllCookies(this CookieContainer c)
        {
            Hashtable k = (Hashtable)c.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            foreach (DictionaryEntry element in k)
            {
                SortedList l = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);
                foreach (var e in l)
                {
                    var cl = (CookieCollection)((DictionaryEntry)e).Value;
                    foreach (Cookie fc in cl)
                    {
                        yield return fc;
                    }
                }
            }
        }

        public static Parameter TryFind(this List<Parameter> parameters, string name)
        {
            return parameters.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
