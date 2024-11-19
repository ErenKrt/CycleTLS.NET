using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CycleTLS.HttpClient
{
    public sealed class CycleHandler : HttpClientHandler
    {
        private readonly CycleHandlerOptions _options;
        private ICycleClient _cycleClient => _options.CycleClient;

        public CycleHandler(CycleHandlerOptions options) : base() {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cookies = this.CookieContainer?.GetAllCookies().ToList() ?? new List<Cookie>();

            var cycleOptions = new CycleRequestOptions()
            {
                Url= request.RequestUri.ToString(),
                Method = request.Method.ToString(),
                Cookies = cookies.Select(x => new CycleRequestCookie
                {
                    Domain = x.Domain,
                    Name = x.Name,
                    Value = x.Value,
                    Expires = x.Expires != DateTime.MinValue ? x.Expires.ToString("ddd, dd-MMM-yyyy HH:mm:ss 'EST'", CultureInfo.InvariantCulture) : null,
                    HttpOnly = x.HttpOnly,
                    MaxAge = 90,
                    Path = x.Path,
                }).ToList(),
                Timeout = _options.TimeOut,
                Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault()),
                Proxy = (this.UseProxy && this.Proxy != null) ? this.Proxy.ToStringWithCredentials() : null
            };
            

            if (request.Headers.Contains("User-Agent"))
            {
                cycleOptions.UserAgent = request.Headers.UserAgent.ToString();
            }

            if (request.Headers.Contains("JA3"))
            {
                cycleOptions.Ja3 = request.Headers.GetValues("JA3").FirstOrDefault();
                request.Headers.Remove("JA3");
            }


            if (request.Content != null)
            {
                cycleOptions.Body = await request.Content.ReadAsStringAsync();
                foreach (var header in request.Content.Headers)
                {
                    cycleOptions.Headers[header.Key] = header.Value.FirstOrDefault();
                }
            }


            var cycleResponse = await _cycleClient.SendAsync(cycleOptions);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(cycleResponse.Body),
                StatusCode = cycleResponse.Status,
                RequestMessage = request,
            };

            foreach (var header in cycleResponse.Headers)
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return response;
        }
    }
}
