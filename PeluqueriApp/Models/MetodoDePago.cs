using System.ComponentModel.DataAnnotations;

namespace PeluqueriApp.Models
{
    public class MetodoDePago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}
