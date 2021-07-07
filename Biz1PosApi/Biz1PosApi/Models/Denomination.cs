using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Denomination
    {
        public int Id { get; set; }
        public string DenomName { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }

        [ForeignKey("DenomEntry")]
        public int DenomEntryId { get; set; }
        public virtual DenomEntry DenomEntry { get; set; }
    }
}
