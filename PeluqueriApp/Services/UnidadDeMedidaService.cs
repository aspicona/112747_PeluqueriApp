using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class UnidadDeMedidaService : IUnidadDeMedidaService
    {
        private readonly AppDbContext _context;

        public UnidadDeMedidaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnidadDeMedida>> GetAllUnidadesAsync()
        {
            return await _context.UnidadesDeMedida.ToListAsync();
        }

        public async Task<UnidadDeMedida> GetUnidadByIdAsync(int id)
        {
            return await _context.UnidadesDeMedida.FindAsync(id);
        }
    }
}