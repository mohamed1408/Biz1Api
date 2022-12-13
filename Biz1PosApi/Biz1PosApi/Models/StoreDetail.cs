using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Biz1BookPOS.Models
{
    public class StoreDetail
    {
        public int Id { get; set; }

        [ForeignKey("Store")]
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Area { get; set; }
        public string Note { get; set; }
    }
}
