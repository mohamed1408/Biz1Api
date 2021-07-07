using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class testorder
    {
        public int Id { get; set; }
        public int UPOrderId { get; set; }

        [ForeignKey("StoreId")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        public int OrderStatusId { get; set; }
        public string Json { get; set; }
        public string RiderDetails { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderedDateTime { get; set; }

        public string AcceptedTimeStamp { get; set; }
    }
}
