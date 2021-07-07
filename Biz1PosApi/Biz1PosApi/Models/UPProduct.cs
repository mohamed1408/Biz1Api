using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPProduct
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("StoreId")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("BrandId")]
        public int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
