using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IUnidadDeMedidaService
    {
        Task<List<UnidadDeMedida>> GetAllUnidadesAsync();
        Task<UnidadDeMedida> GetUnidadByIdAsync(int id);
    }

}
