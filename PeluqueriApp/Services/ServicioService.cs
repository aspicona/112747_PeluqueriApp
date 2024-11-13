using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class ServicioService : IServicioService
    {
        private readonly AppDbContext _context;

        public ServicioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Servicio>> GetAllServiciosAsync()
        {
            return await _context.Servicios.Include(s => s.Empresa).ToListAsync();
        }

        public async Task<Servicio> GetServicioByIdAsync(int id)
        {
            return await _context.Servicios
                               .Include(s => s.Empresa) // Cargar la empresa relacionada
                               .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddServicioAsync(Servicio servicio)
        {
            servicio.FechaUltModif = DateTime.Now;
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServicioAsync(Servicio servicio)
        {
            servicio.FechaUltModif = DateTime.Now;
            _context.Servicios.Update(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServicioAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }
    }

}
