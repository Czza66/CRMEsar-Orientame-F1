using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Prestadores;
using CRMEsar.Models.ViewModels.CrudEntidades.Usuarios;
using Microsoft.AspNetCore.Identity;

namespace CRMEsar.Services.Usuarios
{
    public interface IUsuarioServices
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<List<UsuariosTablaVM>> GetUsuariosTablaAsync(Guid usuarioActualId);
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<List<UsuarioDisponibleVM>> UsuariosDisponiblesPrestadores();
    }
}
