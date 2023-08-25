using CycleTLS.Interfaces;
using CycleTLS.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;

namespace CycleTLS.RestSharp.Helpers
{
    public static class RestCyleExtensions
    {
        public static async Task<RestResponse> ExecuteCycleAsync(this RestClient restClient, RestRequest request, ICycleClient cycleClient)
        {
            //var proxy= restClient.Options.Proxy?.GetProxy(new Uri("http://example.com"));
            /*
             TODO: will be add headers imp, body imp, param/query imp, proxy
             */
            var cycleOptions = new CycleRequestOptions()
            {
                Url= restClient.Options.BaseUrl + request.Resource,
                Method= request.Method.ToString().ToUpper(),
            };

            var response= await cycleClient.SendAsync(cycleOptions);

            return new RestResponse() { 
                StatusCode= response.Status,
                Content= response.Body
            };
        }
    }
}
