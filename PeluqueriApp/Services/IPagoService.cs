using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IPagoService
    {
        Task<IEnumerable<Pago>> GetAllPagosAsync();
        Task<Pago> GetPagoByIdAsync(int id);
        Task AddPagoAsync(Pago pago);
        Task UpdatePagoAsync(Pago pago);
        Task DeletePagoAsync(int id);
        Task ActualizarEstadoPagoAsync(int idPago, bool pagado);
    }

}
