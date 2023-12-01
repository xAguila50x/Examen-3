using System.ComponentModel.DataAnnotations;

namespace TiendaWep.Models
{
    public class Estilo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese el N0mbre del Estilo")]
        [Display(Name ="Nombre Estilo")]
        public string nombre { get; set; }
    }
}
