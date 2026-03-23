using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.Logs;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.InfoUser;
using CRMEsar.Models.ViewModels.CrudEntidades.LogsUsuario;
using CRMEsar.Models.ViewModels.CrudEntidades.NotificacionesUsuario;
using CRMEsar.Utilidades.ConfirmarCorreo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class InfoUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogService _logService;
        private readonly IConfirmarCorreo _confirmarCorreo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public InfoUserController(UserManager<ApplicationUser> userManager,
            ILogService logService,
            IConfirmarCorreo confirmarCorreo,
            SignInManager<ApplicationUser> signInManager,
            IContenedorTrabajo contenedorTrabajo)
        {
            _userManager = userManager;
            _logService = logService;
            _confirmarCorreo = confirmarCorreo;
            _signInManager = signInManager;
            _contenedorTrabajo = contenedorTrabajo;
        }

        public async Task<IActionResult> Index()
        {
            var InfoUser = await _userManager.GetUserAsync(User);
            if (InfoUser != null)
            {
                if (InfoUser.EmailConfirmed == false)
                {
                    TempData["ConfirmarCorreo"] = "Es necesario que confirmes tu dirección de correo";
                }
            }
            var notificaciones = _contenedorTrabajo.Notificaciones
                .GetAll(n => n.UserId == InfoUser.Id)
                .Select(n => new NotificacionesTablaVM
                {
                    titulo = n.Titulo,
                    mensaje = n.Mensaje,
                    estaleido = n.EstaLeido,
                    fecha = n.FechaCreacion.ToString("dd/MM/yyyy")
                });

            var log = _contenedorTrabajo.Log.GetAll(
                l => l.UserId == InfoUser.Id
                ).OrderByDescending(l => l.FechaCreacion
                ).Select(l => new LogsUsuarioTablaVM
                {
                    NombreTabla = l.NombreTabla,
                    exitoso = l.Exitoso,
                    respuesta = l.Respuesta,
                    fechaCreacion = l.FechaCreacion.ToString("dd/MM/yyyy")
                });

            var model = new infoUserVM
            {
                usuario = InfoUser,
                notificaciones = notificaciones,
                logs = log
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarImagenPerfil(IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0) 
            {
                var user = await _userManager.GetUserAsync(User);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
                var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/users");
                var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.fotoUser) && user.fotoUser != null)
                {
                    var rutaAnterior = Path.Combine(rutaCarpeta, user.fotoUser);
                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }

                user.fotoUser = nombreArchivo;
                await _userManager.UpdateAsync(user);
                await _logService.RegistrarAsync<ApplicationUser>(
                     user.Id.ToString(),
                     "EDICIONDEFOTODEPERFIL",
                     user,
                     true,
                     "La usuaria actualizo su foto de perfil correctamente"
                     );
                TempData["CambioDeFoto"] = "Imagen de perfil actualizada";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarInformacionPersonal(infoUserVM model) 
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) 
            {
                user.Nombre1 = model.usuario.Nombre1;
                user.Nombre2 = model.usuario.Nombre2;
                user.Apellido1 = model.usuario.Apellido1;
                user.Apellido2 = model.usuario.Apellido2;
                user.NumeroDocumento = model.usuario.NumeroDocumento;
                user.PhoneNumber = model.usuario.PhoneNumber;
                user.FechaModificacion = DateTime.Now;

                var resultado = await _userManager.UpdateAsync(user);
                if (resultado.Succeeded)
                {
                    await _logService.RegistrarAsync<ApplicationUser>(
                         user.Id.ToString(),
                         "EDICIONDATOSPERSONALES",
                         user,
                         true,
                         "La usuaria edito sus datos personales correctamente"
                         );
                    TempData["InformacionPersonal"] = "Datos personales actualizados correctamente";
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarCorreo()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.EmailConfirmed)
            {
                TempData["ConfirmarCorreo"] = "Usuario inválido o ya confirmado.";
                return RedirectToAction(nameof(Index));
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var CallBackURL = Url.Action(
                "ConfirmarCorreoCallBack",
                "InfoUser",
                new { userId = user.Id, token = encodedToken },
                    protocol: Request.Scheme);

            await _confirmarCorreo.EnviarConfirmacionCorreoAsync(user.Id.ToString(), user.Email, CallBackURL);
            TempData["ConfirmarCorreoEnviada"] = "Se ha enviado el enlace de confirmación a tu correo.";

            var fragmento = Uri.EscapeDataString("SeccionCorreoContraseña"); 
            return Redirect(Url.Action("Index", "InfoUser", new { area = "Panel" }) + "#" + fragmento);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarCorreoCallback(string userId, string token) 
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["MensajeError"] = "Parámetros inválidos para confirmar el correo.";
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["MensajeError"] = "Usuario no encontrado.";
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var resultado = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (resultado.Succeeded)
            {
                TempData["MensajeExito"] = "¡Correo electrónico confirmado exitosamente!";
                await _logService.RegistrarAsync<ApplicationUser>(
                     user.Id.ToString(),
                     "CONFIRMACIONCORREO",
                     user,
                     true,
                     "La usuaria confirmo su direccion de correo exitosamente"
                     );
            }
            else
            {
                TempData["MensajeError"] = "Error al confirmar el correo electrónico.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarCorreoContrasena(string Email, string NuevaContrasena, string ConfirmarContrasena)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("Index");
            }

            bool cerrarSesion = false;

            // Cambiar correo si es diferente
            if (user.Email != Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Email);
                if (!setEmailResult.Succeeded)
                {
                    TempData["Error"] = "No se pudo actualizar el correo.";
                    return RedirectToAction("Index");
                }

                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);

                await _logService.RegistrarAsync<ApplicationUser>(
                     user.Id.ToString(),
                     "CAMBIOCORREO",
                     user,
                     true,
                     "La usuaria cambió su correo exitosamente"
                );

                cerrarSesion = true;
            }

            // Validar la contraseña con las reglas de Identity
            var passwordValidator = HttpContext.RequestServices.GetService<IPasswordValidator<ApplicationUser>>();
            var validationResult = await passwordValidator.ValidateAsync(_userManager, user, NuevaContrasena);

            if (!validationResult.Succeeded)
            {
                TempData["Error"] = "La contraseña no cumple con los criterios de seguridad: * @ - _ / A a";
                return RedirectToAction("Index");
            }

            // Cambiar contraseña
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var cambioPassResult = await _userManager.ResetPasswordAsync(user, token, NuevaContrasena);

            if (!cambioPassResult.Succeeded)
            {
                TempData["Error"] = "No se pudo actualizar la contraseña.";
                return RedirectToAction("Index");
            }

            await _logService.RegistrarAsync<ApplicationUser>(
                 user.Id.ToString(),
                 "CAMBIOCONTRASEÑA",
                 user,
                 true,
                 "La usuaria cambió su contraseña exitosamente"
            );

            cerrarSesion = true;

            // Cerrar sesión si hubo cambios
            if (cerrarSesion)
            {
                await _signInManager.SignOutAsync();
                TempData["CerrarSesion"] = "Debes iniciar sesion de nuevo";
                return RedirectToAction("Index", "Home", new { area = "User" });
            }

            TempData["MensajeExitoCambioContraseñaCorreo"] = "No se detectaron cambios.";
            return RedirectToAction("Index");
        }
    }
}
