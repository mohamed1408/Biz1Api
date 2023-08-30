using System;
using System.ComponentModel.DataAnnotations;

namespace Biz1PosApi.Models
{
    public class T
    {
        public int Id { get; set; }
        public int? oi { get; set; }
        public int? acid { get; set; }
        public double? cp { get; set; }
        public double? ca { get; set; }
        public double? t1 { get; set; }
        public double? t2 { get; set; }
        public double? t3 { get; set; }

        [DataType(DataType.Date)]
        public DateTime cd { get; set; }

        [DataType(DataType.Date)]
        public DateTime? md { get; set; }

        public int? ci { get; set; }

        public int? si { get; set; }
    }
}
