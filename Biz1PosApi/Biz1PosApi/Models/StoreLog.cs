using System;
using System.ComponentModel.DataAnnotations;

namespace Biz1PosApi.Models
{
    public class StoreLog
    {
        public int Id { get; set; }
        public string Store { get; set; }
        public string Action { get; set; }

        [DataType(DataType.Date)]
        public DateTime LogedDateTime { get; set; }
    }
}
