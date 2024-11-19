// See https://aka.ms/new-console-template for more information
using CycleTLS;
using CycleTLS.HttpClient;
using CycleTLS.Interfaces;
using RestSharp;
using System.Net;


ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));

var proxy = new WebProxy("http://127.0.0.1:8080");
//proxy.Credentials = new NetworkCredential("eren", "baba");

var httpHandler = new CycleHandler(new CycleHandlerOptions()
{
    CycleClient = cycleClient,
})
{
    //Proxy = proxy,
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => false,
};

RestClient restClient = new(httpHandler, configureRestClient: (x) =>
{
    x.BaseUrl = new Uri("https://example.org");
    x.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 OPR/114.0.0.0";
});

restClient.AddDefaultHeader("JA3", "772,4865-4866-4867-49195-49199-49196-49200-52393-52392-49171-49172-156-157-47-53,65037-65281-5-18-27-23-35-10-45-16-11-17513-51-13-0-43,25497-29-23-24,0");

RestRequest getRequest = new RestRequest(){
    Method = Method.Get
};


var resGet = await restClient.ExecuteAsync(getRequest);
Console.WriteLine(resGet.Content);
Console.ReadLine();

RestRequest postRequest= new RestRequest()
{
    Method = Method.Post,
    Resource = "https://httpbin.org/post",
};
postRequest.AddJsonBody(new
{
    test = "1"
});

var resPost = await restClient.ExecuteAsync(postRequest);
Console.WriteLine(resPost.Content);
Console.ReadLine();