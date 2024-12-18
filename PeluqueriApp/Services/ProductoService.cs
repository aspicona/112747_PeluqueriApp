using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            return await _context.Productos
                                  .Include(p => p.Empresa) // Carga ansiosa de la Empresa
                                  .ToListAsync();
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await _context.Productos
                               .Include(p => p.Empresa) // Cargar la empresa relacionada
                               .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductoViewModel>> GetProductosByEmpresaIdAsync(int empresaId)
        {
            return await _context.Productos
        .Where(p => p.EmpresaId == empresaId)
        .Select(p => new ProductoViewModel
        {
            ProductoId = p.Id,
            Descripcion=p.Descripcion,
            NombreProducto = p.Nombre,
            Costo=p.Costo,
            Precio = p.Precio,
            EmpresaId = p.EmpresaId,
            StockDisponible=p.StockDisponible
        })
        .ToListAsync();
        }

        public async Task AddProductoAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                producto.Activo = false; // Marcar como inactivo
                _context.Productos.Update(producto); // Actualizar el estado en la base de datos
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<ProductoSeleccionadoViewModel>> GetProductosByCitaIdAsync(int citaId)
        {
            var productos = await _context.Productos.ToListAsync(); // Todos los productos
            var productosXCita = await _context.ProductosXcita
                .Where(pxc => pxc.IdCita == citaId)
                .ToListAsync(); // Productos asignados a la cita

            return productos.Select(p => new ProductoSeleccionadoViewModel
            {
                ProductoId = p.Id,
                NombreProducto = p.Nombre,
                Seleccionado = productosXCita.Any(pxc => pxc.IdProducto == p.Id),
                Cantidad = productosXCita.FirstOrDefault(pxc => pxc.IdProducto == p.Id)?.Cantidad ?? 0
            }).ToList();
        }
    }
}
