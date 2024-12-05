using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllCitasAsync();
        Task<Cita> GetCitaByIdAsync(int id);
        Task<List<Cita>> GetCitasByEmpresaIdAsync(int empresaId);
        Task AddCitaAsync(Cita cita);
        Task UpdateCitaAsync(Cita cita);
        Task DeleteCitaAsync(int id);
        Task<List<IngresoPorCita>> ObtenerIngresosPorCitasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<List<string>> GetAvailableSlotsAsync(DateTime fecha, int empleadoId, int duracionEstimada);
        Task<List<ServicioRealizadoViewModel>> ObtenerServiciosRealizadosAsync(DateTime startDate, DateTime endDate, int empresaId);
        Task ReservarSlotsAsync(DateTime fecha, int empleadoId, TimeSpan horarioInicio, int duracionMinutos, int citaId);
        Task CancelarReservasAsync(int citaId);
        Task EnviarMensajeWhatsAppAsync(string numeroCliente);
    }

}
