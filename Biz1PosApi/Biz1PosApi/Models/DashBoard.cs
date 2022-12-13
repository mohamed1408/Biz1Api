using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class DashBoard
    {
        public int Id { get; set; }
        public double Sales { get; set; }
        public double Payments { get; set; }
        public int BillCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime? OrderedDateTime { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? OrderedDate { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
