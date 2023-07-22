using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class DiscountVerification
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Comments { get; set; }

        [ForeignKey("User")]
        public int? VerifiedBy { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        [DataType(DataType.Date)]
        public DateTime? VerifiedDatetime { get; set; }//------------------
    }
}