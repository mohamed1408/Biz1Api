using Biz1BookPOS.Models;

namespace Biz1PosApi.Models
{
    public class OldProducts
    {
        public int Id { get; set; }
        public int OldId { get; set; }
        public string Name { get; set; }
        public int TaxGroupId { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public int groupid { get; set; }
    }
}
