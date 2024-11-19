
using CycleTLS;
using CycleTLS.HttpClient;
using CycleTLS.Interfaces;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));

var cookieContainer = new CookieContainer();
var handler = new CycleHandler(new CycleHandlerOptions()
{
    CycleClient = cycleClient,
    TimeOut = TimeSpan.FromSeconds(10),
})
{
    CookieContainer = cookieContainer,
    /*Proxy = new WebProxy("http://127.0.0.1:8080")
    {
        Credentials = new NetworkCredential("eren", "baba")
    }*/
};


cookieContainer.Add(new Cookie("test", "test", "/", "example.com"));

var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri("http://example.com"),
    DefaultRequestHeaders =
    {
        { "Test", "test-ke" }
    },
};

using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            userId = 77,
            id = 1,
            title = "write code sample",
            completed = false
        }),
        Encoding.UTF8,
        "application/json");

var response = await httpClient.GetAsync("/");
Console.WriteLine(await response.Content.ReadAsStringAsync());