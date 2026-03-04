using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaNomina.Models
{
    [Table("departments")]
    public class Department
    {
        [Key]
        [StringLength(50)]
        public string dept_no { get; set; }

        [Required, StringLength(50)]
        public string dept_name { get; set; }

        public ICollection<DeptEmp>? DeptEmps { get; set; }
        public ICollection<DeptManager>? DeptManagers { get; set; }
    }
}
