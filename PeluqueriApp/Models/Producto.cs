using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int StockDisponible { get; set; }

        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
    }


}
