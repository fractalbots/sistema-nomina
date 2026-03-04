using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaNomina.Models
{
    [Table("Log_AuditoriaSalarios")]
    public class LogAuditoriaSalarios
    {
        [Key]
        public int id { get; set; }

        [Required, StringLength(100)]
        public string usuario { get; set; }

        [Required]
        public DateTime fechaActualizacion { get; set; }

        [Required, StringLength(250)]
        public string DetalleCambio { get; set; }

        [Required]
        public long salario { get; set; }

        [Required]
        public int emp_no { get; set; }
    }
}
