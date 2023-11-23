using Biz1Retail_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public double Tax1 { get; set; }
        public double Tax2 { get; set; }
        public double Tax3 { get; set; }
        public double DiscPercent { get; set; }
        public double DiscAmount { get; set; }
        public double? ItemDiscount { get; set; }
        public double? TaxItemDiscount { get; set; }
        public double? OrderDiscount { get; set; }
        public double? TaxOrderDiscount { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("KitchenUser")]
        public int? KitchenUserId { get; set; }
        public virtual User KitchenUser { get; set; }

        [ForeignKey("KOT")]
        public int? KOTId { get; set; }
        public virtual KOT KOT { get; set; }

        public string Note { get; set; }
        public string Message { get; set; }
        public double? TotalAmount { get; set; }
        public double? Extra { get; set; }
        public string kotrefid { get; set; }
        public string refid { get; set; }
        public bool IsStockUpdate { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string OptionJson { get; set; }
        public float? ComplementryQty { get; set; }

        [NotMapped]
        public List<OrdItemAddon> OrdItemAddons { get; set; }

        [NotMapped]
        public List<OrdItemVariant> OrdItemVariants { get; set; }

        public Otms ToOtms()
        {
            Otms oi = new Otms();
            oi.qy = Quantity;
            oi.pr = Price;
            oi.ob = OrderId;
            oi.pi = ProductId;
            oi.to = Tax1;
            oi.tt = Tax2;
            oi.tth = Tax3;
            oi.dp = DiscPercent;
            oi.da = DiscAmount;
            oi.sti = StatusId;
            oi.kui = KitchenUserId;
            oi.n = Note;
            oi.cqy = ComplementryQty;
            oi.ki = KOTId;
            oi.cati = CategoryId;
            oi.oj = OptionJson;
            oi.ta = TotalAmount;
            oi.imd = ItemDiscount;
            oi.od = OrderDiscount;
            oi.tid = TaxItemDiscount;
            oi.tod = TaxOrderDiscount;
            oi.ext = Extra;
            oi.msg = Message;
            oi.kri = kotrefid;
            oi.ri = refid;
            oi.issu = IsStockUpdate;
            return oi;
        }
    }
}
