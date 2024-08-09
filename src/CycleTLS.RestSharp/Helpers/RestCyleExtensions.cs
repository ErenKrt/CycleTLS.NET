﻿using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;

using RestSharp;
using System.Collections.Specialized;
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
            var allParameters = request.Parameters
                .AddParameters(restClient.DefaultParameters)
                .ToList();

            var ja3Param = allParameters.TryFind("ja3") ?? throw new Exception("You need set the JA3 to default header or request");
            var userAgentParam = allParameters.TryFind("User-Agent") ?? throw new Exception("You need set the UserAgent of JA3 to default header or request");

            var queryStringParams = allParameters
                .Where(x => x.Type == ParameterType.QueryString)
                .Select(x => $"{HttpUtility.UrlEncode(x.Name)}={HttpUtility.UrlEncode(x.Value?.ToString())}")
                .ToList();


            var finalUrl = request.Resource;

            if (restClient.Options.BaseUrl is not null)
            {
                finalUrl = (new UriBuilder(restClient.Options.BaseUrl)
                {
                    Path = $"{restClient.Options.BaseUrl.AbsolutePath.TrimEnd('/')}/{request.Resource.TrimStart('/')}",
                    Query = string.Join("&", queryStringParams),
                }).Uri.AbsoluteUri;
            }

            var bodyParam = allParameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
            if (bodyParam is not null)
            {
                allParameters.RemoveAll(x => x.Name == "Content-Type");
                allParameters.Add(new HeaderParameter("Content-Type", bodyParam.ContentType));
            }
            
            var acceptParam= allParameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name== "Accept");
            if (acceptParam is null)
            {
                allParameters.Add(new HeaderParameter("Accept", string.Join(',', restClient.AcceptedContentTypes)));
            }

            var acceptEncodingParam = allParameters.FirstOrDefault(x => x.Type == ParameterType.HttpHeader && x.Name == "Accept-Encoding");
            if (acceptEncodingParam is null)
            {
                var methods = restClient.Options.AutomaticDecompression;
                var decompressionMethods = new List<string>();

                if (methods.HasFlag(DecompressionMethods.GZip))
                    decompressionMethods.Add("gzip");
                if (methods.HasFlag(DecompressionMethods.Deflate))
                    decompressionMethods.Add("deflate");
                if (methods.HasFlag(DecompressionMethods.Brotli))
                    decompressionMethods.Add("br");

                var acceptEncodingValue = string.Join(", ", decompressionMethods);

                allParameters.Add(new HeaderParameter("Accept-Encoding", acceptEncodingValue));
            }

            var cookies = request.CookieContainer?.GetAllCookies()?.ToList() ?? new List<Cookie>();

            var cycleOptions = new CycleRequestOptions
            {
                Url = finalUrl,
                Method = request.Method.ToString(),
                Headers = allParameters.Where(x => x.Type == ParameterType.HttpHeader && x.Name!="ja3" ).ToDictionary(x => x.Name, x => x.Value?.ToString()),
                UserAgent = userAgentParam.Value.ToString(),
                Ja3 = ja3Param.Value.ToString(),
                Body = bodyParam?.Value.ToString(),
                Cookies = cookies.Any()
                ? cookies.Select(x => new CycleRequestCookie
                {
                    Domain = x.Domain,
                    Name = x.Name,
                    Value = x.Value,
                    Expires = x.Expires.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                    HttpOnly = x.HttpOnly,
                    MaxAge = 90,
                    Path = x.Path,
                }).ToList()
                : null,
                Proxy = restClient.Options.Proxy is WebProxy webProxy ? webProxy.toStringWithCredentials() : null,
                InsecureSkipVerify = true,
            };


            var response = await cycleClient.SendAsync(cycleOptions);

            return new RestResponse
            {
                StatusCode = response.Status,
                Content = response.Body,
                ContentHeaders = response.Headers.Select(x => new HeaderParameter(x.Key, x.Value)).ToList(),
                ContentType = response.Headers.TryGetValue("Content-Type", out var contentType) ? contentType : null,
                ContentLength = response.Headers.TryGetValue("Content-Length", out var contentLength) ? long.Parse(contentLength) : 0,
                Request = request,
                ResponseUri = new Uri(finalUrl),
                Server = response.Headers.TryGetValue("Server", out var server) ? server : null
            };
        }

        public static Parameter TryFind(this List<Parameter> parameters, string name)
        {
            return parameters.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

    }

}
