using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [ValidateNever]
        public Cliente Cliente { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("EstadoCita")]
        public int IdEstadoCita { get; set; }
        [ValidateNever]
        public EstadoCita EstadoCita { get; set; }

        public decimal PrecioEstimado { get; set; }
        public decimal PrecioFinal { get; set; }

        [ForeignKey("Empleado")]
        public int IdEmpleado { get; set; }
        [ValidateNever]
        public Empleado Empleado { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
        [NotMapped]
        public string Detalle => $"{Fecha.ToShortDateString()} - {Cliente?.Nombre} {Cliente?.Apellido}";
    }
}
