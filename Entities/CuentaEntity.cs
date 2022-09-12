using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaPichincha.Entities
{
    public class CuentaEntity
    {
        [Key]
        [MaxLength(50)]
        public string? NumeroCuenta { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [MaxLength(10)]
        public string? TipoCuenta { get; set; }
        [Required,Precision(18,2)]
        public decimal SaldoInicial { get; set; }

        public bool Estado { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteEntity? Cliente { get; set; }

    }
}
