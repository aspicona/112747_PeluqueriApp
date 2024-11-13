using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IServicioService
    {
        Task<IEnumerable<Servicio>> GetAllServiciosAsync();
        Task<Servicio> GetServicioByIdAsync(int id);
        Task AddServicioAsync(Servicio servicio);
        Task UpdateServicioAsync(Servicio servicio);
        Task DeleteServicioAsync(int id);
    }

}
