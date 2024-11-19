using CycleTLS.Helpers;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
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

			var finalUrl = restClient.Options.BaseUrl != null
			? new UriBuilder(restClient.Options.BaseUrl)
			{
				Path = request.Resource,
				Query = string.Join("&", queryStringParams)
			}.Uri.ToString()
			: request.Resource;


			var bodyParam = allParameters.FirstOrDefault(x => x.Type == ParameterType.RequestBody);
			if (bodyParam != null)
			{
				allParameters.Add(new HeaderParameter("Content-Type", bodyParam.ContentType));
			}

            var cookies = request.CookieContainer?.GetAllCookies().ToList() ?? new List<Cookie>();

            var cycleOptions = new CycleRequestOptions
			{
				Url = finalUrl,
				Method = request.Method.ToString(),
				Headers = allParameters.Where(x => x.Type == ParameterType.HttpHeader).ToDictionary(x => x.Name, x => x.Value?.ToString()),
				UserAgent = userAgentParam.Value.ToString(),
				Ja3 = ja3Param.Value.ToString(),
				Body = bodyParam?.Value.ToString(),
				Timeout= request.Timeout ?? restClient.Options.Timeout,
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
				Proxy = restClient.Options.Proxy is WebProxy webProxy ? webProxy.ToStringWithCredentials() : null,
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
       
	}

}
