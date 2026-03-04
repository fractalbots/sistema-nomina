using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaNomina.Models
{
    [Table("users")]
    public class AppUser
    {
        [Key]
        public int emp_no { get; set; }

        [Required, StringLength(100)]
        public string usuario { get; set; }

        [Required, StringLength(100)]
        public string clave { get; set; }

        [Required, StringLength(50)]
        public string rol { get; set; }
    }
}
