using CycleTLS.Models;

namespace CycleTLS.Interfaces
{
    public interface ICycleClient : IDisposable
    {
        Task<CycleResponse> SendAsync(CycleRequestOptions options);
    }
}
