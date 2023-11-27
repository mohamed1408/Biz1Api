using Biz1BookPOS.Models;
using Biz1Retail_API.Models;
using System.Collections.Generic;

namespace Biz1PosApi.Models
{
    public class OrderPckg
    {
        public Odrs odrs { get; set; }
        public List<Otms> otms { get; set; }
    }
}
