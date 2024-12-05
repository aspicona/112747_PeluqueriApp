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
        public async Task<List<Servicio>> GetServiciosByEmpresaIdAsync(int empresaId)
        {
            return await _context.Servicios
                .Where(s => s.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task AddServicioAsync(Servicio servicio, List<InsumosXservicio> insumos)
        {
            // Validar o procesar insumos antes de asignarlos
            foreach (var insumo in insumos)
            {
                if (insumo.CantidadNecesaria <= 0)
                {
                    throw new InvalidOperationException("La cantidad necesaria debe ser mayor a 0.");
                }
            }

            servicio.FechaUltModif = DateTime.Now;
            servicio.InsumosXservicio = insumos;

            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateServicioAsync(Servicio servicio, List<InsumosXservicio> nuevosInsumos)
        {
            servicio.FechaUltModif = DateTime.Now;

            var insumosExistentes = _context.InsumosXservicio
                .Where(ixs => ixs.IdServicio == servicio.Id);

            _context.InsumosXservicio.RemoveRange(insumosExistentes);

            foreach (var insumo in nuevosInsumos)
            {
                insumo.IdServicio = servicio.Id; // Aseguramos que los nuevos insumos estén relacionados con el servicio
                _context.InsumosXservicio.Add(insumo);
            }

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
        public async Task<List<ServicioViewModel>> GetServiciosByCitaIdAsync(int citaId)
        {
            var servicios = await _context.Servicios.ToListAsync(); // Todos los servicios
            var serviciosXcita = await _context.ServiciosXcita
                .Where(sxc => sxc.IdCita == citaId)
                .ToListAsync(); // Servicios asignados a la cita

            return servicios.Select(s => new ServicioViewModel
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Seleccionado = serviciosXcita.Any(sxc => sxc.IdServicio == s.Id),
                PrecioBase = s.PrecioBase,
                DuracionEstimada = s.DuracionEstimada
            }).ToList();
        }
    }
}
