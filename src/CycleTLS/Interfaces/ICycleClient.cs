using CycleTLS.Models;
using System;
using System.Threading.Tasks;

namespace CycleTLS.Interfaces
{
    public interface ICycleClient : IDisposable
    {
        Task<CycleResponse> SendAsync(CycleRequestOptions options);
    }
}
