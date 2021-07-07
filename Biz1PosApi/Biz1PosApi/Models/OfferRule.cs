using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class OfferRule
    {
        public int Id { get; set; }

        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }

        public int RuleId { get; set; }
        public int Type { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
