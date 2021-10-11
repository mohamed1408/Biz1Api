using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class PredefinedQuantity
    {
        public int Id { get; set; }
        public string QuantityText { get; set; }
        public double Quantity { get; set; }
        public double FreeQuantity { get; set; }
        public double TotalQuantity { get; set; }
        public double? Price { get; set; }
        public int CakeQuantityId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [NotMapped]
        public bool isdeleted { get; set; }
    }
}