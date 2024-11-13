using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IEstadoCitaService
    {
        Task<IEnumerable<EstadoCita>> GetAllEstadosAsync();
        Task<EstadoCita> GetEstadoByIdAsync(int id);
    }

}
