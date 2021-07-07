﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class OrdItemVariant
    {
        public int Id { get; set; }

        [ForeignKey("ProductVariant")]
        public int VariantId { get; set; }
        public virtual Variant Variant { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; }

        public double Price { get; set; }
    }
}