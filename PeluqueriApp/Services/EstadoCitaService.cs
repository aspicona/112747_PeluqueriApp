using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class EstadoCitaService : IEstadoCitaService
    {
        private readonly AppDbContext _context;

        public EstadoCitaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EstadoCita>> GetAllEstadosAsync()
        {
            return await _context.EstadosCita.ToListAsync();
        }

        public async Task<EstadoCita> GetEstadoByIdAsync(int id)
        {
            return await _context.EstadosCita.FindAsync(id);
        }
    }
}