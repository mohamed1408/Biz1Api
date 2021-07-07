using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int DeliveryBoyId { get; set; }
        public virtual User DeliveryBoy { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string Location { get; set; }

        public int StatusId { get; set; }

    }
}
