using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PeluqueriApp.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Aquí defines las entidades (tablas) que vas a usar
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<EstadoCita> EstadosCita { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<MetodoDePago> MetodosDePago { get; set; }
    }
}
