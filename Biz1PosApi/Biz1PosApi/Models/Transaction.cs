using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType { get; set; }

        [ForeignKey("StorePaymentType")]
        public int? StorePaymentTypeId { get; set; }
        public virtual StorePaymentType StorePaymentType { get; set; }

        public int TranstypeId { get; set; }
        public int? PaymentStatusId { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ModifiedDateTime { get; set; }

        [Column(TypeName = "Date")]
        public DateTime TransDate { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Store")]
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public string Notes { get; set; }

    }
}
