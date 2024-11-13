using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly AppDbContext _context;

        public EspecialidadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especialidad>> GetAllEspecialidadesAsync()
        {
            return await _context.Especialidades.ToListAsync();
        }

        public async Task<Especialidad> GetEspecialidadByIdAsync(int id)
        {
            return await _context.Especialidades.FindAsync(id);
        }
    }
}