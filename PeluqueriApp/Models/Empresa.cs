using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PeluqueriApp.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Logo { get; set; }
        public string ColorPrincipal { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string? ConfigAdicionales { get; set; }
        public bool Activo { get; set; } = true;

        [ValidateNever]
        public ICollection<Producto> Productos { get; set; }
        [ValidateNever]
        public ICollection<Insumo> Insumos { get; set; }
        [ValidateNever]
        public ICollection<Servicio> Servicios { get; set; }
    }

}
