// See https://aka.ms/new-console-template for more information
using CycleTLS;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using CycleTLS.RestSharp.Helpers;
using RestSharp;
using System.Net;

ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));

RestClient restClient = new(new RestClientOptions()
{
    BaseUrl = new Uri("https://httpbin.org"),
    Proxy= new WebProxy("http://192.168.1.2")
});
/*
RestRequest getRequest = new RestRequest()
{
    Resource="get"
};

var resGet = await restClient.ExecuteCycleAsync(getRequest, cycleClient);
*/
RestRequest postRequest = new RestRequest()
{
    Resource = "post",
};

postRequest.AddJsonBody(new
{
    test = "1"
});

var resPost= await restClient.ExecuteCycleAsync(postRequest, cycleClient);

Console.ReadLine();