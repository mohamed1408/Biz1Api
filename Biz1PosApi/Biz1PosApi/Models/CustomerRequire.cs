using Biz1BookPOS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class CustomerRequire
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
        public string Reason { get; set; }


        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
