using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAllProductosAsync();
        Task<Producto> GetProductoByIdAsync(int id);
        Task<List<ProductoViewModel>> GetProductosByEmpresaIdAsync(int empresaId);
        Task AddProductoAsync(Producto producto);
        Task UpdateProductoAsync(Producto producto);
        Task DeleteProductoAsync(int id);
        Task<List<ProductoSeleccionadoViewModel>> GetProductosByCitaIdAsync(int citaId);
    }


}
