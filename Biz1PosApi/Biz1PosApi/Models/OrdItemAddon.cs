using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class OrdItemAddon
    {
        public int Id { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; }

        [ForeignKey("ProductAddon")]
        public int AddonId { get; set; }
        public virtual Addon Addon { get; set; }

        [ForeignKey("DropDown")]
        public int? StatusId { get; set; }
        public virtual DropDown DropDown { get; set; }
        
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
