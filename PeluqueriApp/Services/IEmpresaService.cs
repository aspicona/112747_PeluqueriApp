using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    public interface IEmpresaService
    {
        Task<IEnumerable<Empresa>> GetAllEmpresasAsync();
        Task<Empresa> GetEmpresaByIdAsync(int id);
        Task<Empresa> GetEmpresaByUserIdAsync(string id);
        Task AddEmpresaAsync(Empresa empresa);
        Task UpdateEmpresaAsync(Empresa empresa);
        Task DeleteEmpresaAsync(int id);
    }

}
