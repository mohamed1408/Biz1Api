using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UPLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Json { get; set; }
        public string ReferenceId { get; set; }

        [DataType(DataType.Date)]
        public DateTime LogDateTime { get; set; }

        [ForeignKey("Brand")]
        public int? BrandId { get; set; }
        public virtual Brand Brand { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
