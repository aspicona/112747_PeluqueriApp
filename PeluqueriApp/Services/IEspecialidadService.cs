using PeluqueriApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PeluqueriApp.Services
{
    public interface IEspecialidadService
    {
        Task<IEnumerable<Especialidad>> GetAllEspecialidadesAsync();
        Task<Especialidad> GetEspecialidadByIdAsync(int id);
    }
}
