using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllCitasAsync();
        Task<Cita> GetCitaByIdAsync(int id);
        Task AddCitaAsync(Cita cita);
        Task UpdateCitaAsync(Cita cita);
        Task DeleteCitaAsync(int id);
    }

}
