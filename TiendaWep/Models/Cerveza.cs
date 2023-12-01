using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaWep.Models
{
    public class Cerveza
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingresa el Nombre de la cerveza")]
        [Display(Name ="Nombre Cerveza")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Ingresa el % de la alcohol")]
        [Display(Name = "% de alcohol")]
        public double alcohol { get; set; }
        [Display(Name = "Estilo")]
        public int idEstilo { get; set; }
        [ForeignKey("idEstilo")]
        public Estilo? Estilo { get; set; }
        [Required(ErrorMessage = "Ingresa el precio de la cerveza")]
        [Display(Name = "Precio")]
        public double Precio{ get; set; }
        [Display(Name = "Imagen")]
        public string? UrlImagen { get; set; }
    }
}
