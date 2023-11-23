using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Biz1BookPOS.Models
{
    public class Odrs
    {
        [Key]
        public int OdrsId { get; set; }
        public int Id { get; set; }
        public int? on { get; set; }
        public string ino { get; set; }
        public string cr { get; set; }
        public int? soi { get; set; }
        public string aoi { get; set; }
        public long? upoi { get; set; }

        public int? si { get; set; }

        public int? dsi { get; set; }

        public int? cui { get; set; }

        public int? cai { get; set; }
        public int? dri { get; set; }

        public int? osi { get; set; }
        public int? psi { get; set; }

        public double? ba { get; set; }
        public double? ta { get; set; }
        public double? pa { get; set; }
        public double? ra { get; set; }
        public string s { get; set; }

        public double? to { get; set; }
        public double? tt { get; set; }
        public double? tth { get; set; }

        public int? bsi { get; set; }

        public int? sti { get; set; }
        public double? dp { get; set; }
        public double? da { get; set; }
        public bool? isao { get; set; }
        public string cud { get; set; }

        public int? dti { get; set; }

        public int? wi { get; set; }

        [DataType(DataType.Date)]
        public DateTime oddt { get; set; }

        [Column(TypeName = "Date")]
        public DateTime od { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? did { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ddd { get; set; }

        //public TimeSpan? odt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? didt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dddt { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? bdt { get; set; }

        //[Column(TypeName = "Date")]
        //public DateTime? bd { get; set; }

        public string n { get; set; }
        public string osd { get; set; }
        public string rsd { get; set; }
        public bool? fr { get; set; }
        public bool? c { get; set; }
        public bool? isp { get; set; }
        public string oj { get; set; }
        public string ij { get; set; }
        public string cj { get; set; }
        public double? cgesb { get; set; }
        public double? odb { get; set; }
        public double? otadb { get; set; }
        public double? otodb { get; set; }
        public double? aidb { get; set; }
        public double? aitadb { get; set; }
        public double? aitodb { get; set; }
        public long? cts { get; set; }

        [DataType(DataType.Date)]
        public DateTime md { get; set; }

        public int? ui { get; set; }

        public int? ci { get; set; }

        public int? oti { get; set; }

        public string inoj { get; set; }

        [NotMapped]
        public List<OrderItem> OrderItems { get; set; }

        public Order ToOrder()
        {
            Order o = new Order
            {
                Id = Id,
                //o.odrsid = OdrsId;
                OrderNo = (int)on,
                InvoiceNo = ino,
                CancelReason = cr,
                SourceId = soi,
                AggregatorOrderId = aoi,
                UPOrderId = upoi,
                StoreId = si,
                DeliveryStoreId = dsi,
                CustomerId = cui,
                CustomerAddressId = cai,
                DiscountRuleId = dri,
                OrderStatusId = (int)osi,
                PreviousStatusId = psi,
                BillAmount = (double)ba,
                TotalAmount = ta,
                PaidAmount = (double)pa,
                RefundAmount = (double)ra,
                Source = s,
                Tax1 = (double)to,
                Tax2 = (double)tt,
                Tax3 = (double)tth,
                BillStatusId = (int)bsi,
                SplitTableId = sti,
                DiscPercent = (double)dp,
                DiscAmount = (double)da,
                IsAdvanceOrder = (bool)isao,
                CustomerData = cud,
                DiningTableId = dti,
                WaiterId = wi,
                OrderedDateTime = oddt,
                OrderedDate = od,
                DeliveryDate = did,
                DeliveredDate = ddd,
                DeliveryDateTime = didt,
                DeliveredDateTime = dddt,
                Note = n,
                OrderStatusDetails = osd,
                RiderStatusDetails = rsd,
                FoodReady = (bool)fr,
                Closed = (bool)c,
                isPaid = (bool)isp,
                OrderJson = oj,
                ItemJson = ij,
                ChargeJson = cj,
                Charges = cgesb,
                OrderDiscount = odb,
                OrderTaxDisc = otadb,
                OrderTotDisc = otodb,
                AllItemDisc = aidb,
                AllItemTaxDisc = aitadb,
                AllItemTotalDisc = aitodb,
                CreatedTimeStamp = cts,
                ModifiedDate = md,
                UserId = ui,
                CompanyId = (int)ci,
                OrderTypeId = (int)oti
            };
            return o;
        }
    }
}
