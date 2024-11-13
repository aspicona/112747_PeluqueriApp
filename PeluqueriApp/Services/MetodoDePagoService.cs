using System.Collections.Generic;
using System.Threading.Tasks;
using PeluqueriApp.Models;
using Microsoft.EntityFrameworkCore;

public class MetodoDePagoService : IMetodoDePagoService
{
    private readonly AppDbContext _context;

    public MetodoDePagoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MetodoDePago>> GetAllMetodosDePagoAsync()
    {
        return await _context.MetodosDePago.ToListAsync();
    }

    public async Task<MetodoDePago> GetMetodoDePagoByIdAsync(int id)
    {
        return await _context.MetodosDePago.FindAsync(id);
    }
}
