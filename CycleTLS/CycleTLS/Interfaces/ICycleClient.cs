using CycleTLS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleTLS.Interfaces
{
    public interface ICycleClient : IDisposable
    {
        Task<CycleResponse> SendAsync(CycleRequestOptions options);
    }
}
