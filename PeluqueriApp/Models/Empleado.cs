using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        public string Domicilio { get; set; }

        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey("Especialidad")]
        public int IdEspecialidad { get; set; }
        [ValidateNever]
        public Especialidad Especialidad { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }

        public DateTime FechaIngreso { get; set; }

        public TimeSpan HorarioDesde { get; set; }
        public TimeSpan HorarioHasta { get; set; }
    }
}
