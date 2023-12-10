
# CycleTLS.NETs

A .NET client for [CycleTLS](https://github.com/Danny-Dasilva/CycleTLS).

.NET 7.0 | You can build if you need another version of .net



## Installation

1. Download the latest release and import it into your project.

2. Next, you'll need to download the executable for [CycleTLS](https://www.npmjs.com/package/cycletls?activeTab=code). Install CycleTLS using NPM by following these steps:

    - Open your terminal or command prompt.
    - Navigate to your project directory using the `cd` command.
    - Run the following command to install CycleTLS: `npm install cycletls`

3. Once CycleTLS is installed, locate the executable file in the installation folder.

By following these steps, you'll have CycleTLS installed and ready to use in your project.
## Usage

#### Server

```csharp
using CycleTLS;
using CycleTLS.Interfaces;
using CycleTLS.Models;

ICycleServer server= new CycleServer(new()
{
    Port= 9112,
    Path= "D:\\Tools\\cycleTLS\\index.exe"
});
server.start()
```
#### Client
```csharp
ICycleClient cycleClient = new CycleClient(new Uri($"ws://127.0.0.1:9112"));
var resGet = await cycleClient.SendAsync(new CycleRequestOptions()
{
    Url= "https://httpbin.org/get"
});
```

You can use the client without the server. You need to start the CycleTLS process and set the port using the WS_PORT variable. After that, set the URL of CycleTLS in the client.

#### ResClient Support

You need to import the "CycleTLS.RestSharp" namespace into your project.

```csharp
RestClient restClient = new(new RestClientOptions()
{
    BaseUrl = new Uri("https://example.org"),
});

RestRequest request = new RestRequest()
{
    Method= Method.GET,
};

/* You can use all features of RestRequest object | For more information look examples */

var response= await restClient.ExecuteCycleAsync(request, cycleClient);
```
## Files you use

* CylceTLS
    * [CycleRequestOptions](https://github.com/ErenKrt/CycleTLS.NET/blob/main/CycleTLS/CycleTLS/Models/CycleRequestOptions.cs)
    * [RestCyleExtensions](https://github.com/ErenKrt/CycleTLS.NET/blob/main/CycleTLS/Example.RestSharp/Example.RestSharp.csproj)