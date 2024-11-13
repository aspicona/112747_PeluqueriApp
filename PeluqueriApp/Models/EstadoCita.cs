using System.ComponentModel.DataAnnotations;

namespace PeluqueriApp.Models
{
    public class EstadoCita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }
    }
}
