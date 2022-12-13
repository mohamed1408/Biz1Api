using Biz1BookPOS.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class StoreConfig
    {
        public int Id { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string Note { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location_Url { get; set; }
        public string Review_Url { get; set; }
    }
}
