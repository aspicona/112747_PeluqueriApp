using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PeluqueriApp.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; } // Foreign key for Empresa
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        [ValidateNever]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ValidateNever]
        public Empresa Empresa { get; set; }
    }

}
