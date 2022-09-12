using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaPichincha.Entities
{
    public class PersonaEntity
    {
        [Key]
        public string? Identificacion { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Nombre { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Genero { get; set; }
        [Required]
        public int Edad { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Direccion { get; set; }
        [Required]
        [MaxLength(10)]
        public string? Telefono { get; set; }
        [Required]
        public bool Estado { get; set; }
        public List<ClienteEntity>? Clientes { get; set; }
    }
}
