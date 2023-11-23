using Biz1PosApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class Order
    {
        private readonly IServiceProvider provider;
        public Order()
        {
            //this.provider = provider;
        }
        public int Id { get; set; }//------------------
        //public int InvoiceNo { get; set; }

        public int OrderNo { get; set; }//------------------

        //[ForeignKey("OrderType")]
        //public int OrderTypeId { get; set; }
        //public virtual DropDown OrdeType { get; set; }
        public string InvoiceNo { get; set; }//------------------
        public string CancelReason { get; set; }//------------------

        public int? SourceId { get; set; }//------------------
        public string AggregatorOrderId { get; set; }//------------------
        public long? UPOrderId { get; set; }//------------------

        [ForeignKey("Store")]
        public int? StoreId { get; set; }//------------------
        public virtual Store Store { get; set; }

        [ForeignKey("DeliveryStore")]
        public int? DeliveryStoreId { get; set; }//------------------
        public virtual Store DeliveryStore { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("CustomerAddress")]
        public int? CustomerAddressId { get; set; }
        public virtual CustomerAddress CustomerAddress { get; set; }

        //[ForeignKey("DiscountRule")]
        public int? DiscountRuleId { get; set; }
        //public virtual DiscountRule DiscountRule{ get; set; }

        public int OrderStatusId { get; set; }//------------------
        public int? PreviousStatusId { get; set; }//------------------

        public double BillAmount { get; set; }//------------------
        public double? TotalAmount { get; set; }//------------------
        public double PaidAmount { get; set; }//------------------
        public double RefundAmount { get; set; }
        public string Source { get; set; }//------------------

        public double Tax1 { get; set; }//------------------
        public double Tax2 { get; set; }//------------------
        public double Tax3 { get; set; }//------------------

        public int BillStatusId { get; set; }//------------------

        public int? SplitTableId { get; set; }
        public double DiscPercent { get; set; }//------------------
        public double DiscAmount { get; set; }//------------------
        public bool IsAdvanceOrder { get; set; }//------------------
        public string CustomerData { get; set; }//------------------

        [ForeignKey("DiningTable")]
        public int? DiningTableId { get; set; }
        public virtual DiningTable DiningTable { get; set; }

        //[ForeignKey("Cashier")]
        //public int CashierId { get; set; }
        //public virtual User Cashier { get; set; }

        [ForeignKey("POSUser")]
        public int? WaiterId { get; set; }
        public virtual User POSUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderedDateTime { get; set; }//------------------

        [Column(TypeName = "Date")]
        public DateTime OrderedDate { get; set; }//------------------

        [Column(TypeName = "Date")]
        public DateTime? DeliveryDate { get; set; }//------------------

        [Column(TypeName = "Date")]
        public DateTime? DeliveredDate { get; set; }//------------------

        public TimeSpan OrderedTime { get; set; }//------------------

        [DataType(DataType.Date)]
        public DateTime? DeliveryDateTime { get; set; }//------------------

        [DataType(DataType.Date)]
        public DateTime? DeliveredDateTime { get; set; }//------------------

        [DataType(DataType.Date)]
        public DateTime BillDateTime { get; set; }//------------------

        [Column(TypeName = "Date")]
        public DateTime BillDate { get; set; }//------------------

        public string Note { get; set; }//------------------
        public string OrderStatusDetails { get; set; }
        public string RiderStatusDetails { get; set; }
        public bool FoodReady { get; set; }//------------------
        public bool Closed { get; set; }//------------------
        public bool isPaid { get; set; }//------------------
        public string OrderJson { get; set; }
        public string ItemJson { get; set; }
        public string ChargeJson { get; set; }
        public double? Charges { get; set; }
        public double? OrderDiscount { get; set; }
        public double? OrderTaxDisc { get; set; }
        public double? OrderTotDisc { get; set; }
        public double? AllItemDisc { get; set; }
        public double? AllItemTaxDisc { get; set; }
        public double? AllItemTotalDisc { get; set; }
        public long? CreatedTimeStamp { get; set; }
        public int? ord_Day { get; set; }
        public int? ord_Month { get; set; }
        public int? ord_Year { get; set; }
        public int? ord_Week { get; set; }
        public int? ord_Quarter { get; set; }
        public int? del_Day { get; set; }
        public int? del_Month { get; set; }
        public int? del_Year { get; set; }
        public int? del_Week { get; set; }
        public int? del_Quarter { get; set; }

        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }//??????????????
        public virtual User User { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }//------------------
        public virtual Company Company { get; set; }

        [ForeignKey("OrderType")]
        public int OrderTypeId { get; set; }//------------------
        public virtual OrderType OrderType { get; set; }

        [NotMapped]
        public List<OrderItem> OrderItems { get; set; }

        public Odrs ToOdrs()
        {
            int odrsid = 0;
            Odrs o = new Odrs
            {
                //using (var scope = provider.CreateScope())
                //{
                //    var db = scope.ServiceProvider.GetRequiredService<POSDbContext>();
                //    odrsid = db.Odrs.Where(x => x.Id == Id && x.ino == InvoiceNo).FirstOrDefault().OdrsId;
                //}
                Id = Id,
                OdrsId = odrsid,
                on = OrderNo,
                ino = InvoiceNo,
                cr = CancelReason,
                soi = SourceId,
                aoi = AggregatorOrderId,
                upoi = UPOrderId,
                si = StoreId,
                dsi = DeliveryStoreId,
                cui = CustomerId,
                cai = CustomerAddressId,
                dri = DiscountRuleId,
                osi = OrderStatusId,
                psi = PreviousStatusId,
                ba = BillAmount,
                ta = TotalAmount,
                pa = PaidAmount,
                ra = RefundAmount,
                s = Source,
                to = Tax1,
                tt = Tax2,
                tth = Tax3,
                bsi = BillStatusId,
                sti = SplitTableId,
                dp = DiscPercent,
                da = DiscAmount,
                isao = IsAdvanceOrder,
                cud = CustomerData,
                dti = DiningTableId,
                wi = WaiterId,
                oddt = OrderedDateTime,
                od = OrderedDate,
                did = DeliveryDate,
                ddd = DeliveredDate,
                didt = DeliveryDateTime,
                dddt = DeliveredDateTime,
                n = Note,
                osd = OrderStatusDetails,
                rsd = RiderStatusDetails,
                fr = FoodReady,
                c = Closed,
                isp = isPaid,
                oj = OrderJson,
                ij = ItemJson,
                cj = ChargeJson,
                cgesb = Charges,
                odb = OrderDiscount,
                otadb = OrderTaxDisc,
                otodb = OrderTotDisc,
                aidb = AllItemDisc,
                aitadb = AllItemTaxDisc,
                aitodb = AllItemTotalDisc,
                cts = CreatedTimeStamp,
                md = ModifiedDate,
                ui = UserId,
                ci = CompanyId,
                oti = OrderTypeId
            };
            return o;
        }
    }
}