using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int MsgTransType { get; set; }
        public int MessageType { get; set; }
        public int RecieverStatus { get; set; }
        public string Content { get; set; }

        [ForeignKey("Img")]
        public int? ImgId { get; set; }
        public virtual Img Img { get; set; }

        //[ForeignKey("Store")]
        public int? StoreId { get; set; }
        //public virtual Store Store { get; set; }

        //[ForeignKey("User")]
        public int? UserId { get; set; }
        //public virtual User User { get; set; }

        [Column(TypeName = "Date")]
        public DateTime MessageDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime MessageDateTime { get; set; }
    }
}