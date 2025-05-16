using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITEST
{
    public class PostResult
    {
        public int code { get; set; }
        public string message { get; set; }
        public object result { get; set; }
        public long timestamp { get; set; }
        public object continuationPoint { get; set; }
        public object totalCount { get; set; }
    }
}
