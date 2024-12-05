using Microsoft.EntityFrameworkCore;
using PeluqueriApp.Models;
using PeluqueriApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmpleadoService : IEmpleadoService
{
    private readonly AppDbContext _context;

    public EmpleadoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Empleado>> GetAllEmpleadosAsync()
    {
        return await _context.Empleados.Include(e => e.Empresa).Include(e => e.Especialidad).ToListAsync();
    }

    public async Task<Empleado> GetEmpleadoByIdAsync(int id)
    {
        return await _context.Empleados.Include(e => e.Empresa).Include(e => e.Especialidad).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Empleado>> GetEmpleadosByEmpresaIdAsync(int idEmpresa)
    {
        return await _context.Empleados
            .Where(e => e.IdEmpresa == idEmpresa)
            .Include(e => e.Especialidad) // Incluye relaciones necesarias
            .ToListAsync();
    }

    public async Task AddEmpleadoAsync(Empleado empleado)
    {
        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmpleadoAsync(Empleado empleado)
    {
        _context.Empleados.Update(empleado);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmpleadoAsync(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado != null)
        {
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
        }
    }
}
