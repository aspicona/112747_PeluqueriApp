﻿using PeluqueriApp.Models;

namespace PeluqueriApp.Services
{
    

    public interface IEmpleadoService
    {
        Task<IEnumerable<Empleado>> GetAllEmpleadosAsync();
        Task<Empleado> GetEmpleadoByIdAsync(int id);
        Task AddEmpleadoAsync(Empleado empleado);
        Task UpdateEmpleadoAsync(Empleado empleado);
        Task DeleteEmpleadoAsync(int id);
    }


}