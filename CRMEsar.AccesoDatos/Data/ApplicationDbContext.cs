using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole, Guid>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Aplica todas las configuraciones encontradas en el ensamblado, las configuraciones se de las entidades se pusieron en: AccesoDatos/Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Aqui Se deben agregar los modelos que se vayan creando
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<Entidades> Entidad { get; set; }
        public DbSet<Estados> Estado { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<ModulosSecciones> ModulosSecciones { get; set; }
        public DbSet<ApplicationRole> AspNetRoles { get; set; }
        public DbSet<PermisosModulosSecciones> PermisosModulosSecciones { get; set; }
        public DbSet<AspNetUserLogs> AspNetUserLogs { get; set; }
        public DbSet<TipoNotificaciones> TipoNotificaciones { get; set; }
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<Integraciones> Integraciones { get; set; }
        public DbSet<Paises> Paises {  get; set; }
        public DbSet<SliderPresentaciones> SliderPresentaciones { get; set; }
        public DbSet<TiposDocumentos> TipoDocumentos { get; set; }
        public DbSet<Cargos> Cargos { get; set; }
        public DbSet<Prestadores> Prestadores { get; set; }
        public DbSet<AtencionesRDA> AtencionesRDA {get; set; }
        public DbSet<SystemSetting> systemSettings { get; set; }
    }
    
}
