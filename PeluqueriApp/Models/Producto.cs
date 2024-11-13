using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PeluqueriApp.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public int StockDisponible { get; set; }

        [ValidateNever]
        public Empresa Empresa { get; set; }
    }


}
