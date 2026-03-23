using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;
using System.Text.Encodings.Web;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.Logs;
using CRMEsar.AccesoDatos.Services.Notificaciones;
using CRMEsar.AccesoDatos.Services.Slider;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;
using CRMEsar.Models.ViewModels.Login;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.Areas.User.Controllers
{
    [Area("User")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public readonly UrlEncoder _urlEncoder;
        public readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ILogService _logService;
        private readonly INotificacionesService _notificaciones;
        private readonly ISliderService _SliderService;
        private readonly ProtectorUtils _protectorUtils;

        private const string NombreEntidadNormalizado = "ESTADOSREGISTROS";

        public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            UrlEncoder urlEncoder,
            IContenedorTrabajo contenedorTrabajo,
            ILogService logService,
            INotificacionesService notificaciones,
            ISliderService SliderService,
            ProtectorUtils protectorUtils)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _urlEncoder = urlEncoder;
            _contenedorTrabajo = contenedorTrabajo;
            _logService = logService;
            _notificaciones = notificaciones;
            _SliderService = SliderService;
            _protectorUtils = protectorUtils;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            var vm = new AccesoVM
            {
                Slider = sliders
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegisterVM rVM)
        {
            if (!ModelState.IsValid)
            {
                return View(rVM);
            }

            // Buscar estado "Activo" asociado a esa entidad
            var estadoActivo = _contenedorTrabajo.Estado.GetFirstOrDefault(
                e => e.Nombre == "Activo" &&
                     e.Entidad.NormalizedName == NombreEntidadNormalizado,
                includeproperties: "Entidad"
            );

            if (estadoActivo == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontró el estado activo para esta entidad.");
                return View(rVM);
            }

            // Crear el usuario
            var user = new ApplicationUser
            {
                UserName = rVM.Email,
                Email = rVM.Email,
                NombreCompleto = $"{rVM.Nombre1} {rVM.Nombre2 ?? ""} {rVM.Apellido1} {rVM.Apellido2 ?? ""}".Trim(),
                NumeroDocumento = rVM.NumeroDocumento,
                FechaRegistro = DateTime.Now,
                Nombre1 = rVM.Nombre1,
                Nombre2 = rVM.Nombre2,
                Apellido1 = rVM.Apellido1,
                Apellido2 = rVM.Apellido2,
                EstadoId = estadoActivo.EstadoId
            };

            var resultado = await _userManager.CreateAsync(user, rVM.Password);
            if (resultado.Succeeded)
            {
                await _notificaciones.CrearNotificacionAsync(
                    userId: user.Id.ToString(),
                    titulo:"Bienvenida/o a nuestro CRM",
                    mensaje:"Por favor verifica tu correo electronico para evitar bloquear tu cuenta",
                    nombreTabla:"ApplicationUser",
                    tipoNotificacionNormalizedName: "SUCCES" //MISTAKE, WARNING
                    );

                await _userManager.AddToRoleAsync(user, "Admin");
                return RedirectToAction("Index", new { correcto = true });
            }
            return View(rVM);
        }

        // Fundionalidad Login Iniciar Sesion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoVM Login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Login.Email);

                if (user == null)
                {
                    TempData["ErrorLogin"] = "Usuario o contraseña inválidos.";
                    return RedirectToAction(nameof(Index));
                }

                var estadoActivo = _contenedorTrabajo.Estado.GetFirstOrDefault(
                    e => e.Nombre == "Activo" &&
                         e.Entidad.NormalizedName == NombreEntidadNormalizado,
                    includeproperties: "Entidad"
                );

                if (estadoActivo == null || user.EstadoId != estadoActivo.EstadoId)
                {
                    TempData["ErrorLogin"] = "Tu cuenta no está activa. Contacta al administrador.";
                    return RedirectToAction(nameof(Index));
                }

                var resultado = await _signInManager.PasswordSignInAsync(user, Login.Password, Login.RememberMe, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    var tiene2FA = await _userManager.GetTwoFactorEnabledAsync(user);
                    if (!tiene2FA)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: Login.RememberMe);
                        return RedirectToAction("ActivarAutenticador", "Home", new { area = "User" });
                    }
                }
                if (resultado.RequiresTwoFactor)
                {
                    return RedirectToAction("VerificarCodigoAutenticador", "Home", new { area = "User" });
                }
                if (resultado.IsLockedOut)
                {
                    TempData["ErrorLogin"] = "Tu cuenta esta bloqueada. Contacta al administrador.";
                    return RedirectToAction(nameof(Index));
                }

                await _logService.RegistrarAsync<ApplicationUser>(
                    user.Id.ToString(),
                    "INICIOSESION",
                    user,
                    false,
                    "Inicio de sesión incorrecto"
                );

                TempData["ErrorLogin"] = "Usuario o contraseña inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var sliders = _contenedorTrabajo.SliderPresentaciones
                             .GetAll(s => s.Activo)
                             .OrderBy(s => s.Orden)
                             .Select(s => new SliderPresentacionesVM
                             {
                                 ImagenRuta = s.ImagenRuta,
                                 Titulo = s.Titulo,
                                 Descripcion = s.Descripcion,
                                 Orden = s.Orden,
                                 Activo = s.Activo,
                                 SliderPresentacionEncriptado = s.SliderId.ToString()
                             })
                             .ToList();

            Login.Slider = sliders; //se vuelven a cargar
            return View("Index", Login); //se renderiza la vista Index.cshtml correctamente
        }


        //Autenticacion en dos factores
        [HttpGet]
        public async Task<IActionResult> ActivarAutenticador()
        {
            string formatoUrlAutenticador = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            var usuario = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(usuario);
            var token = await _userManager.GetAuthenticatorKeyAsync(usuario);
            //Habilitar CodigoQR
            string urlAutenticador = string.Format(formatoUrlAutenticador, _urlEncoder.Encode("Fundacion Esar - CRM"), _urlEncoder.Encode(usuario.Email), token);

            var ADFModel = new AutenticacionDosFactoresVM() { Token = token, urlCodigoQR = urlAutenticador, Slider = _SliderService.ObtenerSlidersActivo() };
            return View(ADFModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivarAutenticador(AutenticacionDosFactoresVM adf)
        {
            if (!ModelState.IsValid)
            {
                return View(adf);
            }

            var usuario = await _userManager.GetUserAsync(User);
            var succeeded = await _userManager.VerifyTwoFactorTokenAsync(usuario, _userManager.Options.Tokens.AuthenticatorTokenProvider, adf.code);

            if (succeeded)
            {
                await _userManager.SetTwoFactorEnabledAsync(usuario, true);

                await _signInManager.SignOutAsync();

                await _signInManager.SignInAsync(usuario, isPersistent: false);

                await _logService.RegistrarAsync<ApplicationUser>(
                    usuario.Id.ToString(),
                    "2FA",
                    usuario,
                    true,
                    "El usuario configuro el 2FA de microsoft authenticator"
                    );

                TempData["VerificacionCorrecta"] = "Intenta iniciar sesion de nuevo";
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            TempData["ErrorAuth"] = "El Codigo no coincide, intentalo de nuevo";
            adf.Token = await _userManager.GetAuthenticatorKeyAsync(usuario);
            string urlAutenticador = string.Format("otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
                _urlEncoder.Encode("Fundacion Esar - CRM"),
                _urlEncoder.Encode(usuario.Email),
                adf.Token
            );
            adf.urlCodigoQR = urlAutenticador;
            return View(adf);
        }

        [HttpGet]
        public IActionResult ConfirmacionAutenticador()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerificarCodigoAutenticador(bool recordarDatos)
        {
            var usuario = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (usuario == null)
            {
                return View("Error");
            }

            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            var vm = new VerificarAutenticadorVM
            {
                RecordarDatos = recordarDatos,
                Slider = sliders
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarCodigoAutenticador(VerificarAutenticadorVM vaVM)
        {
            vaVM.returnURL = vaVM.returnURL ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return View(vaVM);
            }

            var resultado = await _signInManager.TwoFactorAuthenticatorSignInAsync(vaVM.code, vaVM.RecordarDatos, rememberClient: false);

            if (resultado.Succeeded)
            {
                var usuario = await _signInManager.GetTwoFactorAuthenticationUserAsync();

                // Esta es la clave para que se generen los claims correctamente
                var principal = await _signInManager.CreateUserPrincipalAsync(usuario);
                await _signInManager.SignOutAsync(); // Salir de cualquier sesión anterior
                await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                // Confirmamos si ahora los claims están
                var claims = principal.Claims.ToList();
                await _logService.RegistrarAsync<ApplicationUser>(
                    usuario.Id.ToString(),
                    "INICIOSESION",
                    usuario,
                    true,
                    "Inicio de sesion correcto"
                    );

                return RedirectToAction("Index", "Home", new { area = "Panel" });
            }

            if (resultado.IsLockedOut)
            {
                return View("Bloqueado");
            }

            ModelState.AddModelError(string.Empty, "Codigo Invalido");
            return View(vaVM);
        }

        [HttpGet]
        public IActionResult ResetPassword() 
        {
            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            var vm = new ValidarResetPassword
            {
                Slider = sliders
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ValidarResetPassword validarVM) 
        {
            if (!ModelState.IsValid) 
            {
                var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

                var vm = new ValidarResetPassword
                {
                    Slider = sliders
                };
                return View(vm);
            }

            var user = await _userManager.FindByEmailAsync(validarVM.Email);
            if (user == null || user.NumeroDocumento != validarVM.NumeroDocumento)
            {
                var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

                var vm = new ValidarResetPassword
                {
                    Slider = sliders
                };
                ModelState.AddModelError("", "Ups no te reconocimos");

                return View(vm);
            }
            return RedirectToAction("ResetPasswordDirecto", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult ResetPasswordDirecto(string userId)
        {
            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            return View(new ResetPasswordDirectoVM
            {
             
                Slider = sliders,
                UserId = userId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordDirecto(ResetPasswordDirectoVM model)
        {
            if (!ModelState.IsValid) 
            {
                var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

                return View(new ResetPasswordDirectoVM
                {

                    Slider = sliders,
                    UserId = model.UserId
                });
            }
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                TempData["ErrorLogin"] = "Ocurrio un error, intentalo de nuevo";
                return RedirectToAction(nameof(Index));
            }
            // eliminar contraseña anterior
            await _userManager.RemovePasswordAsync(user);

            // asignar nueva contraseña
            var result = await _userManager.AddPasswordAsync(user, model.Password);

            if (result.Succeeded) 
            {
                await _notificaciones.CrearNotificacionAsync(
                    userId: user.Id.ToString(),
                    titulo: "Contraseña reestablecida",
                    mensaje: "Su contraseña se reestablecio, en caso que no fuera usted contactese con el administrador del sistema",
                    nombreTabla: "ApplicationUser",
                    tipoNotificacionNormalizedName: "MISTAKE"
                    );

                TempData["VerificacionCorrecta"] = "Contraseña reestablecida, inicia sesion de nuevo";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


        [HttpGet]
        public IActionResult CuentaAntigua()
        {
            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            var vm = new ValidarCuentaAntiguaVM
            {
                Slider = sliders
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CuentaAntigua(ValidarCuentaAntiguaVM vm)
        {
            bool vieneDocumento = !string.IsNullOrEmpty(vm.NumeroDocumento);
            bool vienePrestador = vm.CodPrestador.HasValue;

            //Validación básica
            if (vieneDocumento == vienePrestador)
            {
                ModelState.AddModelError(
                    "",
                    "Debe ingresar solo una opción: cuenta o prestador"
                );
                return View(CargarSlider(vm));
            }
            //VALIDAR POR DOCUMENTO
            if (vieneDocumento)
            {
                var usuario = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NumeroDocumento == vm.NumeroDocumento && u.Estado.Nombre == "Activo");

                if (usuario == null)
                {
                    ModelState.AddModelError(
                        "NumeroDocumento",
                        "La cuenta no esta activa o no se encontró una cuenta con ese documento"
                    );
                    return View(CargarSlider(vm));
                }

                if (UsuarioTieneCuentaActiva(usuario))
                {
                    ModelState.AddModelError(
                        "NumeroDocumento",
                        "El usuario ya tiene una cuenta activa en el sistema"
                    );
                    return View(CargarSlider(vm));
                }
                //Usuario válido, sin cuenta activa
                return RedirectToAction(
                    "ResultadoCuenta",
                    new { id = _protectorUtils.EncriptarGuid(usuario.Id, "CuentaAntigua") }
                );
            }
            //VALIDAR POR PRESTADOR
            var prestador = _contenedorTrabajo.Prestadores
                .GetFirstOrDefault(p => p.Codigo == vm.CodPrestador.Value && p.User.Estado.Nombre == "Activo");

            if (prestador == null)
            {
                ModelState.AddModelError(
                    "CodPrestador",
                    "No se encontró una prestadora con ese código o la prestadora se encuentra inactiva"
                );
                return View(CargarSlider(vm));
            }

            if (prestador.UserID == null)
            {
                ModelState.AddModelError(
                    "CodPrestador",
                    "La prestadora no tiene un usuario asociado"
                );
                return View(CargarSlider(vm));
            }

            var usuarioPrestador = await _userManager.FindByIdAsync(
                prestador.UserID.ToString()
            );

            if (usuarioPrestador == null)
            {
                ModelState.AddModelError(
                    "CodPrestador",
                    "La usuaria asociada a la prestadora no existe"
                );
                return View(CargarSlider(vm));
            }

            if (UsuarioTieneCuentaActiva(usuarioPrestador))
            {
                ModelState.AddModelError(
                    "CodPrestador",
                    "La usuaria ya tiene una cuenta activa en el sistema"
                );
                return View(CargarSlider(vm));
            }
            //Prestador válido, usuario sin cuenta activa
            return RedirectToAction(
                "ResultadoCuenta",
                new { id = _protectorUtils.EncriptarGuid(prestador.UserID, "CuentaAntigua") }
            );
        }

        [HttpGet]
        public async Task<IActionResult> ResultadoCuenta(string id) 
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");
            var idReal = _protectorUtils.DesencriptarGuid(id, "CuentaAntigua");

            var usuario = await _userManager.FindByIdAsync(idReal.ToString());
            if (usuario == null)
                return RedirectToAction("Index");
            var tienePassword = await _userManager.HasPasswordAsync(usuario);

            if (tienePassword)
            {
                TempData["RespuestaOperacion"] = "Tu cuenta ya se encuentra activa. Inicia sesión.";
                return RedirectToAction("Login");
            }

            var sliders = _contenedorTrabajo.SliderPresentaciones
                                 .GetAll(s => s.Activo)
                                 .OrderBy(s => s.Orden)
                                 .Select(s => new SliderPresentacionesVM
                                 {
                                     ImagenRuta = s.ImagenRuta,
                                     Titulo = s.Titulo,
                                     Descripcion = s.Descripcion,
                                     Orden = s.Orden,
                                     Activo = s.Activo,
                                     SliderPresentacionEncriptado = s.SliderId.ToString()
                                 })
                                 .ToList();

            var vm = new ResultadoCuentaVM
            {
                IdUsuario = _protectorUtils.EncriptarGuid(usuario.Id, "ResetCuenta"),
                Slider = sliders
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResultadoCuenta(ResultadoCuentaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(CargarSlider(model));
            }

            Guid idReal;

            try
            {
                idReal = _protectorUtils.DesencriptarGuid(model.IdUsuario, "ResetCuenta");
            }
            catch
            {
                ModelState.AddModelError("", "Error de seguridad. Intenta nuevamente.");
                return View(CargarSlider(model));
            }

            var usuario = await _userManager.FindByIdAsync(idReal.ToString());
            if (usuario == null)
            {
                ModelState.AddModelError("", "No se pudo reconocer el usuario.");
                return View(CargarSlider(model));
            }

            // Determinar rol por tipo de usuario
            string rolAsignar = usuario.TipoUsuario == "PRESTADOR"
                ? "Prestador"
                : "Cuenta";

            // Actualizar username
            var setUserNameResult = await _userManager.SetUserNameAsync(usuario, model.Correo);
            if (!setUserNameResult.Succeeded)
            {
                AgregarErrores(setUserNameResult);
                return View(CargarSlider(model));
            }

            // Actualizar correo
            var setEmailResult = await _userManager.SetEmailAsync(usuario, model.Correo);
            if (!setEmailResult.Succeeded)
            {
                AgregarErrores(setEmailResult);
                return View(CargarSlider(model));
            }

            // Generar token y resetear contraseña
            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

            var passwordResult = await _userManager.ResetPasswordAsync(
                usuario,
                token,
                model.Password
            );

            if (!passwordResult.Succeeded)
            {
                AgregarErrores(passwordResult);
                return View(CargarSlider(model));
            }


            await _userManager.AddToRoleAsync(usuario, rolAsignar);

            usuario.EmailConfirmed = false;
            usuario.FechaModificacion = DateTime.Now;
            await _userManager.UpdateAsync(usuario);


            TempData["VerificacionCorrecta"] =
                "Cuenta activada correctamente. Ya puedes iniciar sesión.";

            return RedirectToAction(nameof(Index));
        }

        private ValidarCuentaAntiguaVM CargarSlider(ValidarCuentaAntiguaVM vm)
        {
            vm.Slider = _contenedorTrabajo.SliderPresentaciones
                .GetAll(s => s.Activo)
                .OrderBy(s => s.Orden)
                .Select(s => new SliderPresentacionesVM
                {
                    ImagenRuta = s.ImagenRuta,
                    Titulo = s.Titulo,
                    Descripcion = s.Descripcion,
                    Orden = s.Orden,
                    Activo = s.Activo,
                    SliderPresentacionEncriptado = s.SliderId.ToString()
                })
                .ToList();

            return vm;
        }

        private bool UsuarioTieneCuentaActiva(ApplicationUser user)
        {
            return !string.IsNullOrEmpty(user.UserName)
                || !string.IsNullOrEmpty(user.Email)
                || !string.IsNullOrEmpty(user.PasswordHash)
                || !string.IsNullOrEmpty(user.SecurityStamp);
        }

        private void LimpiarCampos(ValidarCuentaAntiguaVM vm)
        {
            vm.NumeroDocumento = null;
            vm.CodPrestador = null;

            ModelState.Remove(nameof(vm.NumeroDocumento));
            ModelState.Remove(nameof(vm.CodPrestador));
        }

        private void AgregarErrores(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private ResultadoCuentaVM CargarSlider(ResultadoCuentaVM model)
        {
            model.Slider = _contenedorTrabajo.SliderPresentaciones
                .GetAll(s => s.Activo)
                .OrderBy(s => s.Orden)
                .Select(s => new SliderPresentacionesVM
                {
                    ImagenRuta = s.ImagenRuta,
                    Titulo = s.Titulo,
                    Descripcion = s.Descripcion,
                    Orden = s.Orden,
                    Activo = s.Activo,
                    SliderPresentacionEncriptado = s.SliderId.ToString()
                })
                .ToList();

            return model;
        }


        private bool DebeSolicitar2FA(ApplicationUser user, bool repromptEnabled, int value, string unit)
        {
            if (!repromptEnabled) return false;

            // Si nunca ha verificado 2FA, pedirlo
            if (user.LastTwoFactorVerifiedUtc is null) return true;

            var last = user.LastTwoFactorVerifiedUtc.Value;

            DateTime next = unit switch
            {
                "weeks" => last.AddDays(7 * value),
                "months" => last.AddMonths(value),
                _ => last.AddDays(7 * value) // fallback
            };

            return DateTime.UtcNow >= next;
        }

    }
}