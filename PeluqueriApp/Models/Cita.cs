using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriApp.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        [ValidateNever]
        public Cliente Cliente { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey("EstadoCita")]
        public int IdEstadoCita { get; set; }
        [ValidateNever]
        public EstadoCita EstadoCita { get; set; }
        public decimal PrecioEstimado { get; set; }
        public decimal PrecioFinal { get; set; }
        public bool Activo { get; set; } = true;

        [ForeignKey("Empleado")]
        public int IdEmpleado { get; set; }
        [ValidateNever]
        public Empleado Empleado { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        [ValidateNever]
        public Empresa Empresa { get; set; }
        [NotMapped]
        public string Detalle => $"{Fecha.ToShortDateString()} - {Cliente?.Nombre} {Cliente?.Apellido}";
        public int DuracionEstimada { get; set; }       
        public List<ServiciosXcita> ServiciosXCita { get; set; } = new List<ServiciosXcita>();
        public List<ProductosXcita> ProductosXCita { get; set; } = new List<ProductosXcita>();
    }

    public class CitaViewModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }
        public int IdEstadoCita { get; set; }
        public DateTime Fecha { get; set; }
        public string HorarioSeleccionado { get; set; }
        public decimal PrecioEstimado { get; set; }
        public decimal PrecioFinal { get; set; }
        public List<int> ServiciosSeleccionados { get; set; }
        public List<ProductoSeleccionadoViewModel>? ProductosSeleccionados { get; set; } = new List<ProductoSeleccionadoViewModel>();
        public List<ProductoViewModel> ProductosDisponibles { get; set; } = new List<ProductoViewModel>();
    }

    public class ProductoViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string? Descripcion { get; set; }
        public bool Seleccionado { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Costo { get; set; }
        public int StockDisponible { get; set; }
        public int EmpresaId { get; set; }
    }

    public class ProductoSeleccionadoViewModel
    {
        public int ProductoId { get; set; } // ID del producto seleccionado
        public string NombreProducto { get; set; } // Nombre del producto (opcional, solo para mostrar)
        public bool Seleccionado { get; set; } // Indica si el producto fue seleccionado en el modal
        public int Cantidad { get; set; } // Cantidad seleccionada del producto
    }
}
