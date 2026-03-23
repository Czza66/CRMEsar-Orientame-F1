using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Prestadores;
using CRMEsar.Models.ViewModels.CrudEntidades.Usuarios;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public UsuarioServices(UserManager<ApplicationUser> userManager, ProtectorUtils protectorUtils, IContenedorTrabajo contenedorTrabajo)
        {
            _userManager = userManager;
            _protectorUtils = protectorUtils;   
            _contenedorTrabajo = contenedorTrabajo;
        }

        // Obtener todos los usuarios
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.Pais)
                .Include(u => u.Estado)
                .Include(u => u.Cargo)
                .ToListAsync();
        }

        // Obtener usuario por Id
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _userManager.FindByIdAsync(id);
        }

        // Crear usuario
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _userManager.CreateAsync(user, password);
        }

        // Actualizar usuario
        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _userManager.UpdateAsync(user);
        }

        // Obtener usuarios para la tabla
        public async Task<List<UsuariosTablaVM>> GetUsuariosTablaAsync(Guid usuarioActualId)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Where(u =>
                    u.Id != usuarioActualId &&
                    u.Estado != null &&
                    u.Estado.Nombre != "Eliminado"
                )
                .Select(u => new UsuariosTablaVM
                {
                    UsuarioIdEncriptado = _protectorUtils.EncriptarGuid(u.Id, "User"),

                    NombreCompleto = u.NombreCompleto,
                    correo = u.Email,
                    Sexo = u.Sexo,
                    Ciudad = u.Ciudad,
                    TipoUsuario = u.TipoUsuario,
                    NumeroDocumento = u.NumeroDocumento,
                    FechaModificacion = u.FechaModificacion,

                    Estado = u.Estado != null ? u.Estado.Nombre : "Sin Estado",
                    Pais = u.Pais != null ? u.Pais.Nombre : "Sin país",
                    Cargo = u.Cargo != null ? u.Cargo.Cargo : "Sin cargo"
                })
                .ToListAsync();
        }

        public async Task<List<UsuarioDisponibleVM>> UsuariosDisponiblesPrestadores()
        {
            var UsuarioConPrestador = _contenedorTrabajo.Prestadores
                .GetAll(includeProperties: "Estado")
                .Where(p => p.Estado.NormalizedName != "ELIMINADO")
                .Select(p => p.UserID)
                .ToHashSet();

            return await _userManager.Users
                .Include(u => u.Pais)
                .Include(u => u.Estado)
                .Where(u => !UsuarioConPrestador.Contains(u.Id) &&
                u.Estado.Nombre != "Eliminado" &&
                u.Estado.Nombre != "Inactivo")
                .Select(u => new UsuarioDisponibleVM 
                {
                    UserId = _protectorUtils.EncriptarGuid(Guid.Parse(u.Id.ToString()), "PrestadorId"),
                    NumeroDocumento = u.NumeroDocumento,
                    NombreCompleto = u.NombreCompleto,
                    Email = u.Email,
                    Ciudad = u.Ciudad,
                    Pais = u.Pais != null ? u.Pais.Nombre : string.Empty
                }).ToListAsync();
                
        }
    }
}
