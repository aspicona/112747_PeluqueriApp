using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IServicioService
    {
        Task<IEnumerable<Servicio>> GetAllServiciosAsync();
        Task<Servicio> GetServicioByIdAsync(int id);
        Task<List<Servicio>> GetServiciosByEmpresaIdAsync(int empresaId);
        Task AddServicioAsync(Servicio servicio, List<InsumosXservicio> insumos);
        Task UpdateServicioAsync(Servicio servicio, List<InsumosXservicio> insumos);
        Task DeleteServicioAsync(int id);
        Task<List<ServicioViewModel>> GetServiciosByCitaIdAsync(int citaId);
        Task<int> CalcularDuracionTotalAsync(List<int> servicioIds);
    }

}
