using System.Collections.Generic;
using System.Threading.Tasks;
using PeluqueriApp.Models;

public interface IMetodoDePagoService
{
    Task<IEnumerable<MetodoDePago>> GetAllMetodosDePagoAsync();
    Task<MetodoDePago> GetMetodoDePagoByIdAsync(int id);
}

