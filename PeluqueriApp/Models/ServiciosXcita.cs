using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class ServiciosXcita
    {
        public int Id { get; set; }

        [ForeignKey("Cita")]
        public int IdCita { get; set; }
        [ValidateNever]
        public Cita Cita { get; set; }

        [ForeignKey("Servicio")]
        public int IdServicio { get; set; }
        [ValidateNever]
        public Servicio Servicio { get; set; }

        public decimal PrecioAjustado { get; set; }
    }

}
