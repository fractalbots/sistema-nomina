using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace SistemaNomina.Models
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        public int emp_no { get; set; }
        [Required, StringLength(50)]
        public string ci { get; set; }
        [Required]
        public DateTime birth_date { get; set; }
        [Required, StringLength(50)]
        public string first_name { get; set; }
        [Required, StringLength(50)]
        public string last_name { get; set; }
        [Required, StringLength(1)]
        public string gender { get; set; }
        [Required]
        public DateTime hire_date { get; set; }
        [Required, EmailAddress, StringLength(100)]
        public string correo { get; set; }

        public ICollection<Salary>? Salaries { get; set; }
        public ICollection<DeptEmp>? DeptEmps { get; set; }
        public ICollection<Title>? Titles { get; set; }
        public ICollection<DeptManager>? DeptManagers { get; set; }
    }
}