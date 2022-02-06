using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPOrderPayload
    {
        public int UPOrderId { get; set; }
        public string Platform { get; set; }
        public int StoreId { get; set; }
    }
}
