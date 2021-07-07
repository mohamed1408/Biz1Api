using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPOption
    {
        public string ref_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        //public double weight { get; set; }
        public int food_type { get; set; }
        public bool sold_at_store { get; set; }
        public bool available { get; set; }
        public double price { get; set; }
        public List<string> opt_grp_ref_ids { get; set; }
    }
}