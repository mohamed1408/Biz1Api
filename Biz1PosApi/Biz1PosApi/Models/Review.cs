using Biz1BookPOS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string Message { get; set; }

        public string Star { get; set; }

        public bool isshow { get; set; }
    }
}