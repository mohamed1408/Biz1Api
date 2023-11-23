using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biz1Retail_API.Models
{
    public class Otms
    {
        [Key]
        public int OtmsId { get; set; }
        public int Id { get; set; }
        public float? qy { get; set; }
        public float? pr { get; set; }

        public int? ob { get; set; }

        public int? pi { get; set; }

        public double? to { get; set; }
        public double? tt { get; set; }
        public double? tth { get; set; }
        public double? dp { get; set; }
        public double? da { get; set; }
        public double? imd { get; set; }
        public double? tid { get; set; }
        public double? od { get; set; }
        public double? tod { get; set; }

        public int? sti { get; set; }

        public int? kui { get; set; }

        public int? ki { get; set; }

        public string n { get; set; }
        public string msg { get; set; }
        public double? ta { get; set; }
        public double? ext { get; set; }
        public string kri { get; set; }
        public string ri { get; set; }
        public bool? issu { get; set; }

        public int? cati { get; set; }

        public string oj { get; set; }
        public float? cqy { get; set; }

        //[NotMapped]
        //public List<OrdItemAddon> OrdItemAddons { get; set; }

        //[NotMapped]
        //public List<OrdItemVariant> OrdItemVariants { get; set; }
    }
}
