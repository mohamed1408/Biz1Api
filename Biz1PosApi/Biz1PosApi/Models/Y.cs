using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class Y
    {
        public int Id { get; set; }
        public double? a { get; set; }

        public int? oi { get; set; }
        public int? cui { get; set; }
        public int? ptid { get; set; }
        public int? sptid { get; set; }
        public int? ttid { get; set; }
        public int? psid { get; set; }

        [DataType(DataType.Date)]
        public DateTime tdt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? mdt { get; set; }

        [Column(TypeName = "Date")]
        public DateTime td { get; set; }

        public int? ui { get; set; }
        public int? ci { get; set; }
        public int? si { get; set; }
        public string n { get; set; }
    }
}
