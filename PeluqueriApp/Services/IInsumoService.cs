using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IInsumoService
    {
        Task<IEnumerable<Insumo>> GetAllInsumosAsync();
        Task<Insumo> GetInsumoByIdAsync(int id);
        Task AddInsumoAsync(Insumo insumo);
        Task UpdateInsumoAsync(Insumo insumo);
        Task DeleteInsumoAsync(int id);
    }

}
