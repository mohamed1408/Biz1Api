using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class DenomEntry
    {
        public int Id { get; set; }
        public double TotalAmount { get; set; }
        public double OpeningBalance { get; set; }
        public double CashIn { get; set; }
        public double SalesCash { get; set; }
        public double CashOut { get; set; }
        public double ExpectedBalance { get; set; }
        public string CashInJson { get; set; }
        public string CashOutJson { get; set; }
        public string TransactionJson { get; set; }
        public int? EntryTypeId { get; set; }
        public double? errorvalue { get; set; }
        public double? lastcomparedvalue { get; set; }

        [DataType(DataType.Date)]
        public DateTime EntryDateTime { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("StoreId")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("EditedForId")]
        public int? EditedForId { get; set; }
        public virtual DenomEntry EditedFor { get; set; }
    }
}
