using CycleTLS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CycleTLS.HttpClient
{
    public class CycleHandlerOptions
    {
        public ICycleClient CycleClient { get; set; }
        public TimeSpan? TimeOut { get; set; }
    }
}
