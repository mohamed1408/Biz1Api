using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class E
    {
        public int Id { get; set; }

        public int? ksid { get; set; }

        public string Ins { get; set; }

        public string Kn { get; set; }

        public string rid { get; set; }

        public string orid { get; set; }

        public string j { get; set; }

        public int? oi { get; set; }

        [DataType(DataType.Date)]
        public DateTime cd { get; set; }

        [DataType(DataType.Date)]
        public DateTime md { get; set; }

        public int? ci { get; set; }

        public int? si { get; set; }


        public int? kgi { get; set; }


        [NotMapped]
        public string OrderItems { get; set; }
    }
}
