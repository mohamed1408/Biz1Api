using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ExpiryDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdateDateTime { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
