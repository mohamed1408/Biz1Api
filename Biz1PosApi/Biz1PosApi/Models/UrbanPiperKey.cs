using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class UrbanPiperKey
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ApiKey { get; set; }
        public bool Zomato { get; set; }
        public bool Swiggy { get; set; }
        public bool FoodPanda { get; set; }
        public bool UberEats { get; set; }
        public bool UrbanPiper { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public virtual Accounts Accounts { get; set; }


    }
}
