using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class InsumosXservicio
    {
        public int Id { get; set; }

        [ForeignKey("Servicio")]
        public int IdServicio { get; set; }
        [ValidateNever]
        public Servicio Servicio { get; set; }

        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        [ValidateNever]
        public Insumo Insumo { get; set; }

        public decimal CantidadNecesaria { get; set; } // Cantidad estándar requerida para el servicio
    }

}
