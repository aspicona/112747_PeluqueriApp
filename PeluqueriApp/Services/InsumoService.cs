using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class InsumoService : IInsumoService
    {
        private readonly AppDbContext _context;

        public InsumoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Insumo>> GetAllInsumosAsync()
        {
            return await _context.Insumos
        .Include(i => i.UnidadDeMedida)
        .Include(i => i.Empresa)        
        .ToListAsync();
        }

        public async Task<Insumo> GetInsumoByIdAsync(int id)
        {
            return await _context.Insumos
                               .Include(i => i.UnidadDeMedida)
                               .Include(i => i.Empresa) // Cargar la empresa relacionada
                               .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Insumo>> GetInsumosByEmpresaIdAsync(int empresaId)
        {
            return await _context.Insumos
                .Include(i => i.UnidadDeMedida)
                .Where(i => i.EmpresaId == empresaId)
                .ToListAsync();
        }

        public async Task<List<InsumoAsignadoViewModel>> GetInsumosByServicioIdAsync(int servicioId)
        {
            var insumos = await _context.Insumos
                .Include(i => i.UnidadDeMedida) // Incluir la relación con UnidadesDeMedida
                .ToListAsync(); // Todos los insumos

            var insumosXServicio = await _context.InsumosXservicio
                .Where(ixs => ixs.IdServicio == servicioId)
                .ToListAsync(); // Insumos asignados al servicio

            return insumos.Select(i => new InsumoAsignadoViewModel
            {
                InsumoId = i.Id,
                NombreInsumo = i.Nombre,
                UnidadDeMedida = i.UnidadDeMedida?.Nombre, // Obtener el nombre de la unidad de medida
                Seleccionado = insumosXServicio.Any(ixs => ixs.IdInsumo == i.Id),
                CantidadNecesaria = insumosXServicio.FirstOrDefault(ixs => ixs.IdInsumo == i.Id)?.CantidadNecesaria ?? 0
            }).ToList();
        }

        public async Task AddInsumoAsync(Insumo insumo)
        {
            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInsumoAsync(Insumo insumo)
        {
            _context.Insumos.Update(insumo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInsumoAsync(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo != null)
            {
                insumo.Activo = false; // Marcar como inactivo
                _context.Insumos.Update(insumo); // Actualizar el estado en la base de datos
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<InsumoUtilizadoViewModel>> ObtenerInsumosUtilizadosAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var serviciosRealizados = await _context.Citas
                .Where(c => c.Fecha >= fechaInicio && c.Fecha <= fechaFin)
                .Include(c => c.ServiciosXCita)
                .ThenInclude(sc => sc.Servicio)
                .ToListAsync();

            var insumosUtilizados = new Dictionary<int, InsumoUtilizadoViewModel>();

            foreach (var cita in serviciosRealizados)
            {
                foreach (var servicioXCita in cita.ServiciosXCita)
                {
                    var insumosXServicio = await _context.InsumosXservicio
                        .Where(ixs => ixs.IdServicio == servicioXCita.IdServicio)
                        .Include(ixs => ixs.Insumo)
                        .ToListAsync();

                    foreach (var insumoXServicio in insumosXServicio)
                    {
                        if (!insumosUtilizados.ContainsKey(insumoXServicio.IdInsumo))
                        {
                            insumosUtilizados[insumoXServicio.IdInsumo] = new InsumoUtilizadoViewModel
                            {
                                IdInsumo = insumoXServicio.IdInsumo,
                                NombreInsumo = insumoXServicio.Insumo.Nombre,
                                CantidadUtilizada = 0
                            };
                        }

                        insumosUtilizados[insumoXServicio.IdInsumo].CantidadUtilizada += insumoXServicio.CantidadNecesaria;
                    }
                }
            }

            return insumosUtilizados.Values.ToList();
        }
    }
}
