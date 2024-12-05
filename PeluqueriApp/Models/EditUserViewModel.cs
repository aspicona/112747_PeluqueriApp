using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
     
        [ForeignKey("Empresa")]
        public int? IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
    }
}
