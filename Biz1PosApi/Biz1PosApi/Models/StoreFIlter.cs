using Biz1BookPOS.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class StoreFIlter
    {
        public int Id { get; set; }
        public double FIlterValue { get; set; }
        public int valuetype { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
