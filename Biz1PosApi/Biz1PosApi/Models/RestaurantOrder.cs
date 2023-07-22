using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class RestaurantOrder
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public float BillAmount { get; set; }
        public float PaidAmount { get; set; }
        public double? Tax1 { get; set; }
        public double? Tax2 { get; set; }
        public double? Tax3 { get; set; }
        public string OrderJson { get; set; }
        public string ItemJson { get; set; }
        public int? OrderType { get; set; }
        public int? OrderStatus { get; set; }

        [Column(TypeName = "Date")]
        public DateTime OrderedDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime DeliveryDate { get; set; }

        public int? StoreId { get; set; }
        public int? CompanyId { get; set; }
        public string Note { get; set; } 



    }
}
