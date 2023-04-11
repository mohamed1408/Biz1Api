using Biz1BookPOS.Models;
using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class KOTInstruction
    {
        public int Id { get; set; }
        public int InstructionType { get; set; }
        public string url { get; set; }

        [DataType(DataType.Date)]
        public DateTime InstructionDateTime { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}