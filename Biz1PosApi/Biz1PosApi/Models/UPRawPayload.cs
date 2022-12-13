using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPRawPayload
    {
        public string Payload { get; set; }
        public string PayloadType { get; set; }
        public int retry_count { get; set; }
    }
}
