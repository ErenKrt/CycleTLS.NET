using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;

using RestSharp;
using System.Globalization;
using System.Net;
using System.Web;

namespace CycleTLS.RestSharp.Helpers
{
    public static class RestCyleExtensions
    {
        public static async Task<RestResponse> ExecuteCycleAsync(this RestClient restClient, RestRequest request, ICycleClient cycleClient)
        {
            var headers = request.Parameters
                .Where(x => x.Type == ParameterType.HttpHeader)
                .ToDictionary(x => x.Name, x => x.Value.ToString());

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in request.Parameters.Where(x => x.Type == ParameterType.QueryString))
            {
                queryString.Add(item.Name, item.Value?.ToString());
            }

            var bodyParam = request.Parameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
            var body = bodyParam?.Value?.ToString();
            var contentType = bodyParam?.ContentType?.ToString();
            if (!string.IsNullOrEmpty(contentType))
            {
                headers["Content-Type"] = contentType;
            }

            var cookies = request.CookieContainer?.GetAllCookies()?.ToList() ?? new List<Cookie>();

            var url = $"{restClient.Options.BaseUrl}{request.Resource}";
            if (queryString.HasKeys())
            {
                url += "?" + queryString;
            }

            var userAgent = request.Parameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name.ToLower() == "user-agent");
            var cycleOptions = new CycleRequestOptions
            {
                Url = url,
                Method = request.Method.ToString(),
                Headers = headers.Any() ? headers : null,
                UserAgent = userAgent?.Value?.ToString(),
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
                }).ToList() : null
            };

            var proxy = restClient.Options.Proxy;
            if (proxy is WebProxy webProxy)
            {
                cycleOptions.Proxy = webProxy.toStringWithCredentials();
            }

            var response = await cycleClient.SendAsync(cycleOptions);

            string? GetHeader(string key)
            {
                if (response.Headers.ContainsKey(key))
                {
                    return response.Headers[key];
                }

                return null;
            }

            return new RestResponse
            {
                StatusCode = response.Status,
                Content = response.Body,
                ContentHeaders = response?.Headers.Select(x => new HeaderParameter(x.Key, x.Value)).ToList(),
                ContentType = GetHeader("Content-Type"),
                ContentLength = long.Parse(GetHeader("Content-Length") ?? "0"),
                Request = request,
                ResponseUri = new Uri(url),
                Server = GetHeader("Server")
            };
        }

    }
}
