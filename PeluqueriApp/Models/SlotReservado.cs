using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class SlotReservado
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("Empleado")]
        public int IdEmpleado { get; set; }
        [ValidateNever]
        public Empleado Empleado { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        [ForeignKey("Cita")]
        public int IdCita { get; set; } // Relación obligatoria con una cita
        [ValidateNever]
        public Cita Cita { get; set; }
    }

}
