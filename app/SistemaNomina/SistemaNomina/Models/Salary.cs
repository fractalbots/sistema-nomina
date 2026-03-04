using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaNomina.Models
{
    [Table("salaries")]
    public class Salary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int emp_no { get; set; }

        [Required]
        public long salary { get; set; }

        [Required, StringLength(50)]
        public string from_date { get; set; }

        public string? to_date { get; set; }

        [ForeignKey("emp_no")]
        public Employee? Employee { get; set; }
    }
}
