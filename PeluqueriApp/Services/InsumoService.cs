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
                _context.Insumos.Remove(insumo);
                await _context.SaveChangesAsync();
            }
        }
    }

}
