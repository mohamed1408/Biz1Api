using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class OrderStatusButton
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int OrderStatusId { get; set; }
        public bool Enabled { get; set; }
        public int SortOrder { get; set; }

        [ForeignKey("OrderTypePreference")]
        public int OrderTypePreferenceId { get; set; }
        public virtual OrderTypePreference OrderTypePreference { get; set; }
    }
}
