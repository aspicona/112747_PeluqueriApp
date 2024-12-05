using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class InsumosXserviciosXcita
    {
        public int Id { get; set; }

        [ForeignKey("ServiciosXcita")]
        public int IdServicioXcita { get; set; }
        [ValidateNever]
        public ServiciosXcita ServicioXcita { get; set; }

        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        [ValidateNever]
        public Insumo Insumo { get; set; }

        public decimal CantidadUtilizada { get; set; } // Cantidad real utilizada en la cita
    }

}
