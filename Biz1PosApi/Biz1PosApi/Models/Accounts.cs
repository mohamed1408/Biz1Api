using Biz1BookPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1PosApi.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string UPUsername { get; set; }
        public string UPAPIKey { get; set; }
        public string SatUname { get; set; }
        public string SatPass { get; set; }
        public string FCM_Token { get; set; }
        public string bizid { get; set; }
        public bool IsConfirmed { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [NotMapped]
        public string jwt { get; set; }
    }
}
