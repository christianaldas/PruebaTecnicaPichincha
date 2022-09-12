using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaPichincha.Entities
{
    public class MovimientoEntity
    {
        [Key]
        public int Id { get; set; }

        public string? CuentaNumeroCuenta { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        [MaxLength]
        public string? TipoMovimiento { get; set; }
        [Required, Precision(18, 2)]

        public decimal Valor { get; set; }
        [Required, Precision(18, 2)]
        public decimal Saldo { get; set; }

        [ForeignKey("CuentaNumeroCuenta")]
        public CuentaEntity? Cuenta { get; set; }
    }
}
