using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class OrderTypePreference
    {
        public int Id { get; set; }

        public bool iscustomisable { get; set; }

        [ForeignKey("OrderType")]
        public int OrderTypeId { get; set; }
        public virtual OrderType OrderType { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
