using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PeluqueriApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
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
        public DbSet<InsumosXservicio> InsumosXservicio { get; set; }
        public DbSet<ServiciosXcita> ServiciosXcita { get; set; }
        public DbSet<ProductosXcita> ProductosXcita { get; set; }
        public DbSet<InsumosXserviciosXcita> InsumosXserviciosXcitas { get; set; }
        public DbSet<SlotReservado> SlotsReservados { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Importante: Llamar al base para conservar la configuración de Identity
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().HasQueryFilter(c => c.Activo);
            modelBuilder.Entity<Cita>().HasQueryFilter(c => c.Activo);
            modelBuilder.Entity<Empresa>().HasQueryFilter(e => e.Activo);
            modelBuilder.Entity<Insumo>().HasQueryFilter(i => i.Activo);
            modelBuilder.Entity<Producto>().HasQueryFilter(p => p.Activo);
            modelBuilder.Entity<Servicio>().HasQueryFilter(s => s.Activo);

            // Relación entre Citas y Empleados
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Empleado)
                .WithMany()
                .HasForeignKey(c => c.IdEmpleado)
                .OnDelete(DeleteBehavior.Restrict); // Evita la eliminación en cascada

            // Relación entre Citas y Clientes
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Citas y EstadosCita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.EstadoCita)
                .WithMany()
                .HasForeignKey(c => c.IdEstadoCita)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación entre Citas y Empresas
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumosXservicio>()
     .ToTable("InsumosXservicio")
     .HasKey(ixs => ixs.Id);

            modelBuilder.Entity<InsumosXservicio>()
                .HasOne(ixs => ixs.Servicio)
                .WithMany(s => s.InsumosXservicio)
                .HasForeignKey(ixs => ixs.IdServicio)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumosXservicio>()
                .HasOne(ixs => ixs.Insumo)
                .WithMany()
                .HasForeignKey(ixs => ixs.IdInsumo)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración para InsumosXserviciosXcita
            modelBuilder.Entity<InsumosXserviciosXcita>()
                .HasOne(ixsc => ixsc.Insumo)
                .WithMany()
                .HasForeignKey(ixsc => ixsc.IdInsumo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InsumosXserviciosXcita>()
                .HasOne(ixsc => ixsc.ServicioXcita)
                .WithMany()
                .HasForeignKey(ixsc => ixsc.IdServicioXcita)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de precisión para campos decimales
            modelBuilder.Entity<Cita>()
                .Property(c => c.PrecioEstimado)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cita>()
                .Property(c => c.PrecioFinal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Insumo>()
                .Property(i => i.CostoUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Costo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            //modelBuilder.Entity<ServiciosXcita>()
            //    .Property(sxc => sxc.PrecioAjustado)
            //    .HasPrecision(18, 2);

            //modelBuilder.Entity<InsumosXservicio>()
            //    .Property(ixs => ixs.CantidadNecesaria)
            //    .HasPrecision(18, 2);

            //modelBuilder.Entity<InsumosXserviciosXcita>()
            //    .Property(ixsc => ixsc.CantidadUtilizada)
            //    .HasPrecision(18, 2);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

