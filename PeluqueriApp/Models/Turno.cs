using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Turno
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Reservado { get; set; } 
        [ForeignKey("Cita")]
        public int IdCita { get; set; }
        [ValidateNever]
        public Cita Cita { get; set; }
    }

}
