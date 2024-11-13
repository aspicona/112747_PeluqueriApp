using System.ComponentModel.DataAnnotations;

namespace PeluqueriApp.Models
{
    public class UnidadDeMedida
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } // Ejemplo: "Gramo", "Unidad", "Litro"
    }
}

