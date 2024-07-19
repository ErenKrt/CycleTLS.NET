using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;

using RestSharp;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace CycleTLS.RestSharp.Helpers
{
    public static class RestCyleExtensions
    {
        public static async Task<RestResponse> ExecuteCycleAsync(this RestClient restClient, RestRequest request, ICycleClient cycleClient)
        {
            var allParams = request.Parameters.AddParameters(restClient.DefaultParameters).ToList();
            var headers = allParams
                .Where(x => !string.Equals(x.Name, "ja3", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Name, x => x.Value?.ToString());

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in allParams.Where(x => x.Type == ParameterType.QueryString))
            {
                queryString.Add(param.Name, param.Value?.ToString());
            }

            var bodyParam = allParams.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
            var body = bodyParam?.Value?.ToString();

            if (bodyParam is not null && !string.IsNullOrEmpty(bodyParam?.ContentType))
            {
                headers["Content-Type"] = bodyParam.ContentType;
            }

            var cookies = request.CookieContainer?.GetAllCookies()?.ToList() ?? new List<Cookie>();

            var url = restClient.Options.BaseUrl != null ? new Uri(restClient.Options.BaseUrl, request.Resource).ToString() : request.Resource;

            var userAgent = GetUserAgent(request, headers, restClient.Options.UserAgent);

            var cycleOptions = new CycleRequestOptions
            {
                Url = url,
                Method = request.Method.ToString(),
                Headers = headers.Any() ? headers : null,
                UserAgent = userAgent,
                Body = body,
                Cookies = cookies.Any() ? cookies.Select(x => new CycleRequestCookie
                {
                    Domain = x.Domain,
                    Name = x.Name,
                    Value = x.Value,
                    Expires = x.Expires.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                    HttpOnly = x.HttpOnly,
                    MaxAge = 90,
                    Path = x.Path,
                }).ToList() : null,
            };

            if (restClient.Options.Proxy is WebProxy webProxy)
            {
                cycleOptions.Proxy = webProxy.toStringWithCredentials();
            }

            var ja3 = allParams.FirstOrDefault(x => x.Name.Equals("ja3", StringComparison.OrdinalIgnoreCase));
            if (ja3 != null)
            {
                cycleOptions.Ja3 = ja3.Value.ToString();
            }

            var response = await cycleClient.SendAsync(cycleOptions);

            return new RestResponse
            {
                StatusCode = response.Status,
                Content = response.Body,
                ContentHeaders = response.Headers.Select(x => new HeaderParameter(x.Key, x.Value)).ToList(),
                ContentType = response.Headers.TryGetValue("Content-Type", out var contentType) ? contentType : null,
                ContentLength = response.Headers.TryGetValue("Content-Length", out var contentLength) ? long.Parse(contentLength) : 0,
                Request = request,
                ResponseUri = new Uri(url),
                Server = response.Headers.TryGetValue("Server", out var server) ? server : null
            };
        }

        private static string GetUserAgent(RestRequest request, IDictionary<string, string> headers, string defaultUserAgent)
        {
            var headerUserAgent = request.Parameters
                .FirstOrDefault(x => x.Type == ParameterType.HttpHeader && string.Equals(x.Name, "user-agent", StringComparison.OrdinalIgnoreCase))?.Value?.ToString();

            return headerUserAgent ?? headers.FirstOrDefault(x => string.Equals(x.Key, "user-agent", StringComparison.OrdinalIgnoreCase)).Value ?? defaultUserAgent;
        }
    }

}
