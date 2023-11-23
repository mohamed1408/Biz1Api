using Biz1BookPOS.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class StoreOB
    {
        [Key]
        public int Id { get; set; }
        public int StoreId { get; set; }
        public double? OpeningBalance { get; set; }
        public double? Expense { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderedDateTime { get; set; }
    }
}
