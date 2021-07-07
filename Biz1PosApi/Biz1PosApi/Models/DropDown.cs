using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Biz1BookPOS.Models
{
    public class DropDown
    {

        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("DropDownGroup")]
        public int DropDownGroupId { get; set; }
        public virtual DropDownGroup DropDownGroup { get; set; }

    }
}
