// See https://aka.ms/new-console-template for more information
using CycleTLS;
using CycleTLS.Interfaces;
using CycleTLS.RestSharp.Helpers;
using RestSharp;
using System.Net;


ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));

var proxy = new WebProxy("http://192.168.1.2:1453");
proxy.Credentials = new NetworkCredential("eren", "baba");

RestClient restClient = new(new RestClientOptions()
{
    BaseUrl = new Uri("https://example.org"),
    //Timeout= TimeSpan.FromSeconds(10),
    //Proxy= proxy
});
restClient.AddDefaultHeader("ja3", "ja3 value");
/*
RestRequest getRequest = new RestRequest()
{
    Resource="get"
};

var resGet = await restClient.ExecuteCycleAsync(getRequest, cycleClient);
*/
RestRequest postRequest = new()
{
    Method = Method.Post,
    Resource = "post",
};

postRequest.AddHeader("user-agent", "test-ke");
postRequest.AddQueryParameter("test", "evet");
//postRequest.AddCookie("aga", "aga", "/", "example.com");
//postRequest.AddFile("test", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.jpg"));
postRequest.AddJsonBody(new
{
    test = "1"
});
//postRequest.Timeout = TimeSpan.FromSeconds(10);

var resPost = await restClient.ExecuteCycleAsync(postRequest, cycleClient);

Console.ReadLine();