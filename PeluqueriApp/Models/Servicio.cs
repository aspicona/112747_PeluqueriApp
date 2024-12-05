using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int DuracionEstimada { get; set; }
        public DateTime FechaUltModif { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }

        public ICollection<InsumosXservicio> InsumosXservicio { get; set; }
    }

    public class ServicioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Seleccionado { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public int DuracionEstimada { get; set; }
        public List<InsumoAsignadoViewModel> InsumosAsignados { get; set; } = new List<InsumoAsignadoViewModel>();
    }

    public class InsumoAsignadoViewModel
    {
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; }
        public bool Seleccionado { get; set; }
        public decimal CantidadNecesaria { get; set; }
        public string UnidadDeMedida { get; set; }
    }

    public class ServicioRealizadoViewModel
    {
        public int Id { get; set; }
        public string NombreServicio { get; set; }
        public int Cantidad { get; set; }
    }

}
