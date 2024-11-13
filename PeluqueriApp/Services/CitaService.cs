using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class CitaService : ICitaService
    {
        private readonly AppDbContext _context;

        public CitaService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las citas
        public async Task<IEnumerable<Cita>> GetAllCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Empleado)
                .Include(c => c.Empresa)
                .Include(c => c.EstadoCita)
                .ToListAsync();
        }

        // Obtener una cita por su ID
        public async Task<Cita> GetCitaByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Empleado)
                .Include(c => c.Empresa)
                .Include(c => c.EstadoCita)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Agregar una nueva cita
        public async Task AddCitaAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
        }

        // Actualizar una cita existente
        public async Task UpdateCitaAsync(Cita cita)
        {
            _context.Citas.Update(cita);
            await _context.SaveChangesAsync();
        }

        // Eliminar una cita
        public async Task DeleteCitaAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
        }
    }
}
