using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Insumo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [ForeignKey("UnidadDeMedida")]
        public int UnidadDeMedidaId { get; set; }
        [ValidateNever]
        public UnidadDeMedida UnidadDeMedida { get; set; } // Relación con UnidadDeMedida

        public decimal CostoUnitario { get; set; }
        public int StockDisponible { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
       
    }
    public class InsumoUtilizadoViewModel
    {
        public int IdInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public decimal CantidadUtilizada { get; set; }
    }

}
