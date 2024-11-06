using System;

namespace CycleTLS.Interfaces
{
    public interface ICycleServer : IDisposable
    {
        public bool Start();
    }
}
