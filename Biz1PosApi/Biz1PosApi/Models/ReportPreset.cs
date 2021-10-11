using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class ReportPreset
    {
        public int Id { get; set; }
        public string report_type { get; set; }
        public string preset_name { get; set; }
        public string preset { get; set; }

        [ForeignKey("Store")]
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
