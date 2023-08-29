// See https://aka.ms/new-console-template for more information
using CycleTLS;
using CycleTLS.Interfaces;
using CycleTLS.Models;


ICycleServer server = new CycleServer(new()
{
    Port = 9112,
    Path = "D:\\Tools\\cycleTLS\\server.exe"
});

server.Start();

ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));

var resGet = await cycleClient.SendAsync(new CycleRequestOptions()
{
    Url = "https://httpbin.org/get"
});

Console.WriteLine(resGet.Body);
Console.ReadLine();

server.Dispose();