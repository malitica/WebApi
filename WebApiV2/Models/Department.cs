using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiV2.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [MinLength(2)]
        [MaxLength(100)]
        public string DepartmentName { get; set; }


    }
}
