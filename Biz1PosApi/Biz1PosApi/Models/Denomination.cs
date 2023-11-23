using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Denomination
    {
        [Key]
        public int DenominationId { get; set; }
        public int Id { get; set; }
        public string DenomName { get; set; }
        public int Count { get; set; }
        public double Amount { get; set; }
        public int DenomEntryId { get; set; }
    }
}
