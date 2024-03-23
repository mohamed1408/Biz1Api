namespace Biz1PosApi.Models
{
    public class UpOrdItm
    {
        public int Id { get; set; }
        public int OrdId { get; set; }
        public string ProdName { get; set; }
        public float Qty { get; set; }
        public float Price { get; set; }
    }
}
