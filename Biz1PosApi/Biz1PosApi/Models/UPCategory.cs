using Biz1PosApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class UPCategory
    {
        public string ref_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? sort_order { get; set; }
        public bool active { get; set; }
        public List<JObject> translations { get; set; }
        public List<string> parent_ref_id { get; set; }
    }
}
