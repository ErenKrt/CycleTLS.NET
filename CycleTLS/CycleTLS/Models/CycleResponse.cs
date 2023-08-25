using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CycleTLS.Models
{
    public class CycleResponse
    {
        public string RequestID { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
