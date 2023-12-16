
# CycleTLS.NETs

A .NET client for [CycleTLS](https://github.com/Danny-Dasilva/CycleTLS).

![GitHub Workflow](https://img.shields.io/github/actions/workflow/status/erenkrt/CycleTLS.NET/publish.yml)
[![CycleTLS.NET](https://img.shields.io/nuget/dt/CycleTLS?label=NuGet%20CycleTLS)](https://www.nuget.org/packages/CycleTLS/)
[![CycleTLS.NET.RestSharp](https://img.shields.io/nuget/dt/CycleTLS.RestSharp?label=NuGet%20CycleTLS.RestSharp)](https://www.nuget.org/packages/CycleTLS.RestSharp/)
![GitHub tag](https://img.shields.io/github/v/tag/erenkrt/cycletls.net?label=Version)

Supports .NET 8.0 (Build available for other .Net versions)
## Installation

### With Nuget
1. In Visual Studio, right-click on your project's Solution Explorer and select 'Manage NuGet Packages'.
2. Go to the 'Browse' tab, search for `CycleTLS` and `CycleTLS.RestSharp`.
3. Click 'Install' for each package.
4. Accept the prompts to complete installation.

Or, via Package Manager Console:
```bash
Install-Package CycleTLS
```

### Manual Installation

1. Download the latest release of CycleTLS.NET's and import it into your project.
2. Install CycleTLS using NPM:
   - Open a terminal/command prompt.
   - Navigate to your project directory (`cd your_project_path`).
   - Run `npm install cycletls`.

Locate the CycleTLS executable in the installation folder after installation.

## Usage

### Server

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

restClient.AddDefaultHeader("ja3", "ja3 value"); // Set JA3 all requests

RestRequest request = new RestRequest()
{
    Method= Method.GET,
};

request.AddHeader("ja3", "ja3 value"); // Set JA3 for specific request

/* You can use all features of RestRequest object | For more information look examples */

var response= await restClient.ExecuteCycleAsync(request, cycleClient);
```
## Files you use

* CylceTLS
    * [CycleRequestOptions](https://github.com/ErenKrt/CycleTLS.NET/blob/main/src/CycleTLS/Models/CycleRequestOptions.cs)
    * [RestCyleExtensions](https://github.com/ErenKrt/CycleTLS.NET/blob/main/src/CycleTLS.RestSharp/Helpers/RestCyleExtensions.cs)

[![Developer](https://img.shields.io/badge/-Developer-E4405F?style=flat-square&logo=Instagram&logoColor=white)](https://www.instagram.com/ep.eren)