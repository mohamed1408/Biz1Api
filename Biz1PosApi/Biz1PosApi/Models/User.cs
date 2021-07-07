using Biz1PosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Pin { get; set; }
        
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Accounts")]
        public int AccountId { get; set; }
        public virtual Accounts Accounts { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}
