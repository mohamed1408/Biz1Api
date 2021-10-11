using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class CakeQuantity
    {
        public int Id { get; set; }
        public string QuantityText { get; set; }
        public double Quantity { get; set; }
        public double FreeQuantity { get; set; }
        public double TotalQuantity { get; set; }
    }
}
