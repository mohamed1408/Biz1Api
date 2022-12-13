using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biz1PosApi.Models
{
    public class Career
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string PhoneNo { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }

        public string EducationDetails { get; set; }
        public string WorkDetails { get; set; }

        public string AdditionalDetails { get; set; }
        public string Note { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}
