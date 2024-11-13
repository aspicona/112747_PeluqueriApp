using System.ComponentModel.DataAnnotations;

namespace PeluqueriApp.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }
    }
}

