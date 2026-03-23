using System.Security.Claims;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Usuarios;
using CRMEsar.Services.Usuarios;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRMEsar.Areas.Panel.Controllers
{


    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private const string NombreEntidadNormalizado = "ASPNETUSERS";

        public UsuariosController(IUsuarioServices usuarioServices,
            ProtectorUtils protectorUtils,
            IEliminadoLogicoService eliminadoLogicoService,
            IContenedorTrabajo contenedorTrabajo,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _usuarioServices = usuarioServices;
            _protectorUtils = protectorUtils;
            _eliminadoLogicoService = eliminadoLogicoService;
            _contenedorTrabajo  = contenedorTrabajo; 
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioActualIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid usuarioActualId = Guid.Empty;

            if (!string.IsNullOrWhiteSpace(usuarioActualIdStr))
            {
                Guid.TryParse(usuarioActualIdStr, out usuarioActualId);
            }

            var vm = await _usuarioServices.GetUsuariosTablaAsync(usuarioActualId);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) 
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var IdReal = _protectorUtils.DesencriptarGuid(id, "User");

            var user = await _usuarioServices.GetByIdAsync(IdReal.ToString());

            if (user == null)
                return NotFound();

            var vm = new UsuariosAgregarEditarVM
            {
                UsuarioId = _protectorUtils.EncriptarGuid(user.Id,"userId"),

                Sexo = string.IsNullOrWhiteSpace(user.Sexo)
                ? "No registra"
                : user.Sexo,

                Ciudad = string.IsNullOrWhiteSpace(user.Ciudad)
                ? "No registra"
                : user.Ciudad,

                TipoUsuario = string.IsNullOrWhiteSpace(user.TipoUsuario)
                ? "No registra"
                : user.TipoUsuario,

                Nombre1 = string.IsNullOrWhiteSpace(user.Nombre1)
                ? "No registra"
                : user.Nombre1,

                Nombre2 = string.IsNullOrWhiteSpace(user.Nombre2)
                ? "No registra"
                : user.Nombre2,

                Apellido1 = string.IsNullOrWhiteSpace(user.Apellido1)
                ? "No registra"
                : user.Apellido1,

                Apellido2 = string.IsNullOrWhiteSpace(user.Apellido2)
                ? "No registra"
                : user.Apellido2,

                NumeroDocumento = string.IsNullOrWhiteSpace(user.NumeroDocumento)
                ? "No registra"
                : user.NumeroDocumento,

                correo = string.IsNullOrWhiteSpace(user.Email)
                ? "No registra"
                : user.Email,

                EmailConfirmed = user.EmailConfirmed ,
                TwoFactorEnabled = user.TwoFactorEnabled,


                PaisId = user.PaisId,
                CargoID = user.CargoId,
                TipoDocumentoID = user.TiposDocumentosId,
                EstadoId = user.EstadoId,

                ListaPaises = _contenedorTrabajo.Paises.GetListaPaises(),
                ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos(),
                ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos(),
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuariosAgregarEditarVM editVM)
        {
            if (!ModelState.IsValid)
            {
                editVM.ListaPaises = _contenedorTrabajo.Paises.GetListaPaises(); 
                editVM.ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos();
                editVM.ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos();
                editVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
                return View(editVM);
            }

            var idReal = _protectorUtils.DesencriptarGuid(editVM.UsuarioId, "userId");
            var user = await _usuarioServices.GetByIdAsync(idReal.ToString());

            if (user == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(user.SecurityStamp))
                user.SecurityStamp = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(user.ConcurrencyStamp))
                user.ConcurrencyStamp = Guid.NewGuid().ToString();

            user.Nombre1 = editVM.Nombre1;
            user.Nombre2 = editVM.Nombre2;
            user.Apellido1 = editVM.Apellido1;
            user.Apellido2 = editVM.Apellido2;

            user.TiposDocumentosId = editVM.TipoDocumentoID;
            user.NumeroDocumento = editVM.NumeroDocumento;
            user.CargoId = editVM.CargoID;
            user.PaisId = editVM.PaisId;
            user.Ciudad = editVM.Ciudad;
            user.EstadoId = editVM.EstadoId;

            user.FechaModificacion = DateTime.Now;

            user.NombreCompleto =
                $"{editVM.Nombre1} {editVM.Nombre2} {editVM.Apellido1} {editVM.Apellido2}"
                .Replace("  ", " ")
                .Trim()
                .ToUpper();

            if (!string.IsNullOrWhiteSpace(editVM.correo) &&
                editVM.correo != "No registra" &&
                editVM.correo != user.Email)
            {
                var userNameResult = await _userManager.SetUserNameAsync(user, editVM.correo);
                if (!userNameResult.Succeeded)
                {
                    editVM.ListaPaises = _contenedorTrabajo.Paises.GetListaPaises();
                    editVM.ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos();
                    editVM.ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos();
                    editVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
                    return View(editVM);
                }

                var emailResult = await _userManager.SetEmailAsync(user, editVM.correo);
                if (!emailResult.Succeeded)
                {
                    editVM.ListaPaises = _contenedorTrabajo.Paises.GetListaPaises();
                    editVM.ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos();
                    editVM.ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos();
                    editVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
                    return View(editVM);
                }
            }

            var resultado = await _usuarioServices.UpdateAsync(user);

            if (resultado.Succeeded)
            {
                TempData["RespuestaOperacion"] = "Se editaron los datos del usuario exitosamente";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(editVM);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetTwoFactor(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var IdReal = _protectorUtils.DesencriptarGuid(id, "userId");

            var user = await _usuarioServices.GetByIdAsync(IdReal.ToString());

            if (user == null)
                return NotFound();

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);

            TempData["RespuestaOperacion"] = "El 2MFA fue reiniciado correctamente. El usuario deberá configurarlo nuevamente.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "User");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<ApplicationUser>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var model = new UsuariosAgregarEditarVM
            {
                ListaPaises = _contenedorTrabajo.Paises.GetListaPaises(),
                ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos(),
                ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos(),
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuariosAgregarEditarVM model)
        {
            if (!ModelState.IsValid)
            {
                CargarListas(model);
                return View(model);
            }

            var usuario = new ApplicationUser
            {
                UserName = model.correo,
                Email = model.correo,
                EmailConfirmed = false,

                Nombre1 = model.Nombre1,
                Nombre2 = model.Nombre2,
                Apellido1 = model.Apellido1,
                Apellido2 = model.Apellido2,
                NombreCompleto = $"{model.Nombre1} {model.Nombre2} {model.Apellido1} {model.Apellido2}".Trim(),

                NumeroDocumento = model.NumeroDocumento,
                TiposDocumentosId = model.TipoDocumentoID,
                CargoId = model.CargoID,
                PaisId = model.PaisId,
                Ciudad = model.Ciudad,
                TipoUsuario = model.TipoUsuario,

                EstadoId = _contenedorTrabajo.Estado
                    .GetFirstOrDefault(
                        e => e.Nombre == "Activo" &&
                        e.Entidad.NormalizedName == NombreEntidadNormalizado,
                        includeproperties: "Entidad"
                    ).EstadoId,

                FechaRegistro = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            // Crear usuario SIN contraseña
            var resultado = await _userManager.CreateAsync(usuario);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                CargarListas(model);
                return View(model);
            }

            // Asignar rol según TipoUsuario
            string rol = model.TipoUsuario == "PRESTADOR"
                ? "Prestador"
                : "Cuenta";

            await _userManager.AddToRoleAsync(usuario, rol);

            TempData["RespuestaOperacion"] = "Usuario creado correctamente";
            return RedirectToAction(nameof(Index));
        }

        private void CargarListas(UsuariosAgregarEditarVM model)
        {
            model.ListaPaises = _contenedorTrabajo.Paises.GetListaPaises();
            model.ListaCargos = _contenedorTrabajo.Cargos.GetListaCargos();
            model.ListaTiposDocumentos = _contenedorTrabajo.TiposDocumentos.GetListaTiposDocumentos();
            model.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
        }


    }
}
