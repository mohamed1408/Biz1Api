using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class OptionQtyGroup
    {
        public int Id { get; set; }
        public double? Factor { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsOnline { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("Option")]
        public int OptionId { get; set; }
        public virtual Option Option { get; set; }

        [ForeignKey("QuantityGroup")]
        public int QuantityGroupId { get; set; }
        public virtual QuantityGroup QuantityGroup { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
