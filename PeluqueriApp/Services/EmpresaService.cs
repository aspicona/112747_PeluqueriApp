using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly AppDbContext _context;

        public EmpresaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empresa>> GetAllEmpresasAsync()
        {
            return await _context.Empresas.ToListAsync();
        }

        public async Task<Empresa> GetEmpresaByIdAsync(int id)
        {
            return await _context.Empresas.FindAsync(id);
        }

        public async Task<Empresa> GetEmpresaByUserIdAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Empresa) // Carga la relación de navegación
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Empresa; // Devuelve la empresa asociada, o null si no hay ninguna
        }

        public async Task AddEmpresaAsync(Empresa empresa)
        {
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            // Detach la entidad creada
            _context.Entry(empresa).State = EntityState.Detached;
        }

        public async Task UpdateEmpresaAsync(Empresa empresa)
        {
            var trackedEntity = _context.Empresas.Local.FirstOrDefault(e => e.Id == empresa.Id);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Empresas.Update(empresa);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteEmpresaAsync(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                empresa.Activo = false; // Marcar como inactiva
                _context.Empresas.Update(empresa); // Actualizar en la base de datos
                await _context.SaveChangesAsync();
            }
        }
    }

}
