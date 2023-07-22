using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPCoupon
    {
        public int Id { get; set; }
        public string Coupon { get; set; }
        public double Value { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
