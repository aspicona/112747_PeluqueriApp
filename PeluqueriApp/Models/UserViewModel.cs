using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
    }

}
