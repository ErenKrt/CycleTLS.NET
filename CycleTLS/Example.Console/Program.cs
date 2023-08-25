// See https://aka.ms/new-console-template for more information
using CycleTLS;
using CycleTLS.Interfaces;
using CycleTLS.Models;
using System.Net;

int port = 9112; 

ICycleServer server= new CycleServer(port);
server.Start();

ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:{port}"));

var resGet = await cycleClient.SendAsync(new CycleRequestOptions()
{
    Url= "https://httpbin.org/get"
});

Console.WriteLine(resGet.Body);


var resPost = await cycleClient.SendAsync(new CycleRequestOptions()
{
    Url = "https://httpbin.org/post",
    Method= HttpMethod.Post.ToString().ToUpper(),
    Body= "{'test':1}"
});

Console.WriteLine(resPost.Body);

var resJsonPost = await cycleClient.SendAsync(new CycleRequestOptions()
{
    Url = "https://httpbin.org/post",
    Method = HttpMethod.Post.ToString().ToUpper(),
    Body = "{'test':1}",
    Headers= new Dictionary<string, string>()
    {
        { "Content-Type", "application/json" }
    }
});

Console.WriteLine(resJsonPost.Body);

Console.ReadLine();