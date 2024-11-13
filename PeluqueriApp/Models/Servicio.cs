using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PeluqueriApp.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int DuracionEstimada { get; set; }
        public DateTime FechaUltModif { get; set; }

        [ValidateNever]
        public Empresa Empresa { get; set; }
    }

}
