using Biz1PosApi.Models;
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

        public int OrderStatusId { get; set; }//------------------
        public int? PreviousStatusId { get; set; }//------------------

        public double BillAmount { get; set; }//------------------
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
    }
}