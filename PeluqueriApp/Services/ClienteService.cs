using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _context.Clientes
                .Include(c => c.Empresa) // Para incluir la relación con Empresa
                .Where(c => c.Activo) // Filtrar solo los clientes activos
                .ToListAsync();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Empresa) // Para incluir la relación con Empresa
                .FirstOrDefaultAsync(c => c.Id == id && c.Activo); // Filtrar solo el cliente activo
        }

        public async Task<List<Cliente>> GetClientesByEmpresaIdAsync(int empresaId)
        {
            return await _context.Clientes
                .Include(c => c.Empresa)
                .Where(c => c.EmpresaId == empresaId && c.Activo) // Filtrar solo los clientes activos de la empresa
                .ToListAsync();
        }

        public async Task AddClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            var cliente = await GetClienteByIdAsync(id);
            if (cliente != null)
            {
                cliente.Activo = false; // Marcar como inactivo
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
        }

    }
}