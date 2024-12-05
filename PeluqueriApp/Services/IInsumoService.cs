using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IInsumoService
    {
        Task<IEnumerable<Insumo>> GetAllInsumosAsync();
        Task<Insumo> GetInsumoByIdAsync(int id);
        Task<List<Insumo>> GetInsumosByEmpresaIdAsync(int empresaId);
        Task<List<InsumoAsignadoViewModel>> GetInsumosByServicioIdAsync(int servicioId);
        Task AddInsumoAsync(Insumo insumo);
        Task UpdateInsumoAsync(Insumo insumo);
        Task DeleteInsumoAsync(int id);
        Task<List<InsumoUtilizadoViewModel>> ObtenerInsumosUtilizadosAsync(DateTime fechaInicio, DateTime fechaFin);
    }

}
