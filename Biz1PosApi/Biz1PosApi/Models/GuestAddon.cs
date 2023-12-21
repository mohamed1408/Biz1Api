using Biz1BookPOS.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    [Table("Addons", Schema = "guest")]
    public class GuestAddon
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int AddonGroupId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        public int CompanyId { get; set; }
    }
}
