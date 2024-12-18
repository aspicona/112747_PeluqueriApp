using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; } = true;

        [ValidateNever]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
    }

}
