using System.Collections.Generic;
using System.Net.Http;

namespace CycleTLS.Models
{
    public class CycleRequestOptions
    {
        public string Url { get; set; } = string.Empty;
        public string Method { get; set; } = HttpMethod.Get.ToString().ToUpper();
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public string Ja3 { get; set; } = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0";
        public string UserAgent { get; set; } = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36";
        public string Proxy { get; set; }
        public List<CycleRequestCookie> Cookies { get; set; }
        public int? Timeout { get; set; }
        public bool? DisableRedirect { get; set; }
        public List<string> HeaderOrder { get; set; }
        public bool? InsecureSkipVerify { get; set; }
        public bool? ForceHTTP1 { get; set; }
    }
}
