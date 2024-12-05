using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
    }
}
