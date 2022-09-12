using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaPichincha.Entities
{
    public class ClienteEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Constrasenia { get; set; }
        public bool Estado { get; set; }
        public PersonaEntity? Persona { get; set; }

    }
}
