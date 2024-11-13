using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cita")]
        public int IdCita { get; set; }
        [ValidateNever]
        public Cita Cita { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [ForeignKey("MetodoDePago")]
        public int MetodoDePagoId { get; set; }
        [ValidateNever]
        public MetodoDePago MetodoDePago { get; set; }

        public bool Pagado { get; set; }
    }
}

