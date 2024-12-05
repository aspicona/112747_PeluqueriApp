using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class ProductosXcita
    {
        public int Id { get; set; }

        [ForeignKey("Cita")]
        public int IdCita { get; set; }
        [ValidateNever]
        public Cita Cita { get; set; }

        [ForeignKey("Producto")]
        public int IdProducto { get; set; }
        [ValidateNever]
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }
    }

}
