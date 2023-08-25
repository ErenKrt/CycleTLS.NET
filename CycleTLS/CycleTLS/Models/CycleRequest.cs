using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleTLS.Models
{
    public class CycleRequest
    {
        public string RequestId { get; set; }
        public CycleRequestOptions Options { get; set; }
    }
}
