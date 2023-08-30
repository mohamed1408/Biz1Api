using System;
using System.ComponentModel.DataAnnotations;

namespace Biz1PosApi.Models
{
    public class ProdGrp
    {
        public int Id { get; set; }
        public string Product { get; set; }

        public float amount { get; set; }
        public string ProductGroup { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int CompanyId { get; set; }

    }
}
