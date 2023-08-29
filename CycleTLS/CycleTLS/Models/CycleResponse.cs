using System.Net;

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
