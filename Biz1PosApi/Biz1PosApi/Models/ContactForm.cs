
using Biz1BookPOS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models

{
    public class ContactForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string RepliedMessage { get; set; }
        public int OrderId { get; set; }
        public float Rating { get; set; }

        public bool isshow { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
