using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class PagoService : IPagoService
    {
        private readonly AppDbContext _context;

        public PagoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pago>> GetAllPagosAsync()
        {
            return await _context.Pagos
                .Include(p => p.MetodoDePago)
                .Include(p => p.Cita).ThenInclude(c => c.Cliente).ToListAsync();
        }

        public async Task<Pago> GetPagoByIdAsync(int id)
        {
            return await _context.Pagos
                .Include(p => p.MetodoDePago)
                .Include(p => p.Cita).ThenInclude(c => c.Cliente).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPagoAsync(Pago pago)
        {
            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePagoAsync(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePagoAsync(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ActualizarEstadoPagoAsync(int idPago, bool pagado)
        {
            // Buscar el pago por su ID
            var pago = await _context.Pagos.FindAsync(idPago);
            if (pago != null)
            {
                pago.Pagado = pagado; // Actualizar el estado del pago
                _context.Pagos.Update(pago);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Pago no encontrado");
            }
        }

    }
}
