using System.Globalization;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.Panel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(SignInManager<ApplicationUser> signInManager,
            IContenedorTrabajo contenedorTrabajo,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _contenedorTrabajo = contenedorTrabajo;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Prestador")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction(nameof(AdminPanel));

            if (User.IsInRole("Prestador"))
                return RedirectToAction(nameof(PrestadorPanel));

            // fallback por seguridad
            return RedirectToAction("AccessDenied", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            var usuariosActivos = _userManager.Users
                .Where(u => u.Estado != null 
                && u.Estado.Nombre == "Activo"
                && u.NormalizedEmail != null)
                .Count();

            var usuarios2MFA = _userManager.Users
               .Where(u => u.Estado != null
               && u.Estado.Nombre == "Activo"
               && u.NormalizedEmail != null
               && u.TwoFactorEnabled == true)
               .Count();

            var prestadoresActivos = _contenedorTrabajo.Prestadores
                .GetAll(p => p.Estado != null
                && p.Estado.Nombre == "Perteneciente"
                && p.User.Estado != null 
                && p.User.Estado.Nombre =="Activo"
                && p.User.NormalizedEmail != null)

                .Count();

            var UsuariosSesionActivaHoy = _contenedorTrabajo.Log
                .GetAll(l => l.TipoAccion == "INICIOSESION"
                && l.Respuesta == "Inicio de sesion correcto"
                && l.NombreTabla == "ApplicationUser"
                && l.FechaCreacion >= hoy && l.FechaCreacion < manana)
                .Count();

            var vm = new AdminPanelVM
            {
                UsuariosActivos = usuariosActivos,
                UsuariosCon2FA = usuarios2MFA,
                PrestadoresActivos = prestadoresActivos,
                IniciosSesionHoy = UsuariosSesionActivaHoy
            };

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult LoginsUltimos12Meses()
        {
            var hoy = DateTime.Today;
            var inicioMesActual = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioRango = inicioMesActual.AddMonths(-11);
            var finRango = inicioMesActual.AddMonths(1);

            var q = _contenedorTrabajo.Log.GetAll(l =>
                l.TipoAccion == "INICIOSESION" &&
                l.Respuesta == "Inicio de sesion correcto" &&
                l.NombreTabla == "ApplicationUser" &&
                l.FechaCreacion >= inicioRango &&
                l.FechaCreacion < finRango
            );

            // Total de inicios por mes (si quieres únicos por usuario, dime el campo UserId en Log)
            var dataDb = q.GroupBy(x => new { x.FechaCreacion.Year, x.FechaCreacion.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToList();

            var labels = new List<string>();
            var values = new List<int>();

            for (int i = 0; i < 12; i++)
            {
                var m = inicioRango.AddMonths(i);
                labels.Add($"{m:MMM yyyy}".ToLower()); // ej: "ene 2026"
                var row = dataDb.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month);
                values.Add(row?.Count ?? 0);
            }

            return Json(new { labels, values });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UsuariosActivosConVsSinPassword()
        {
            // Base query: usuarios activos y con email normalizado (tu regla)
            var q = _userManager.Users.Where(u =>
                u.Estado.Nombre == "Activo"
            );

            var conPassword = q.Count(u => u.PasswordHash != null);
            var sinPassword = q.Count(u => u.PasswordHash == null);

            return Json(new
            {
                labels = new[] { "Usuarioa Activos (migrados)", "Usuarios Inactivos (cuenta antigua)" },
                values = new[] { conPassword, sinPassword }
            });
        }



        [HttpGet]
        [Authorize(Roles = "Prestador")]
        public async Task<IActionResult> PrestadorPanel()
        {
            var userOn = await _userManager.GetUserAsync(User);

            if (userOn == null)
                return Unauthorized();

            var vm = new PrestadorPanelVM
            {
                Nombre = $"{userOn.Nombre1} {userOn.Apellido1}"
            };

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Prestador")]
        public IActionResult AtencionesUltimos12Meses()
        {
            var userId = _userManager.GetUserId(User);

            var prestador = _contenedorTrabajo.Prestadores
                .GetFirstOrDefault(p => p.UserID.ToString() == userId); // <- ajusta: p.UserId o p.User.Id

            if (prestador == null)
                return Forbid();

            var codigoPrestador = prestador.Codigo;

            var q = _contenedorTrabajo.AtencionesRda.GetAll()
                .Where(x => x.CodigoPrestador == codigoPrestador && x.FechaAtencion.HasValue);

            var dataAgrupada = q
                .GroupBy(x => new { x.FechaAtencion.Value.Year, x.FechaAtencion.Value.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Count() })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            if (!dataAgrupada.Any())
                return Json(new { labels = Array.Empty<string>(), values = Array.Empty<int>() });

            var hoy = DateTime.Today;
            bool existeMesActual = dataAgrupada.Any(x => x.Year == hoy.Year && x.Month == hoy.Month);

            var mesBase = existeMesActual
                ? new DateTime(hoy.Year, hoy.Month, 1)
                : new DateTime(dataAgrupada.First().Year, dataAgrupada.First().Month, 1);

            var mesesObjetivo = Enumerable.Range(0, 12)
                .Select(i => mesBase.AddMonths(-i))
                .OrderBy(m => m)
                .ToList();

            var cultura = new CultureInfo("es-CO");
            var labels = mesesObjetivo.Select(m => m.ToString("MMM yyyy", cultura)).ToArray();

            var values = mesesObjetivo.Select(m =>
                dataAgrupada
                    .Where(x => x.Year == m.Year && x.Month == m.Month)
                    .Select(x => x.Total)
                    .FirstOrDefault()
            ).ToArray();

            return Json(new { labels, values });
        }

        [HttpGet]
        [Authorize(Roles = "Prestador")]
        public IActionResult TiposServicioBarChart()
        {
            var userId = _userManager.GetUserId(User);

            var prestador = _contenedorTrabajo.Prestadores
                .GetFirstOrDefault(p => p.UserID.ToString() == userId);

            if (prestador == null)
                return Forbid();

            var codigoPrestador = prestador.Codigo;

            var q = _contenedorTrabajo.AtencionesRda.GetAll()
                .Where(x => x.CodigoPrestador == codigoPrestador);

            // ✅ Agrupar por tipo (limpia nulos / vacíos)
            var dataDb = q
                .Where(x => x.TipoConsulta != null && x.TipoConsulta != "")
                .GroupBy(x => x.TipoConsulta.Trim())
                .Select(g => new { Tipo = g.Key, Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .Take(10) // Top 10 (cámbialo si quieres)
                .ToList();

            var labels = dataDb.Select(x => x.Tipo).ToArray();
            var values = dataDb.Select(x => x.Total).ToArray();

            return Json(new { labels, values });
        }

        [HttpGet]
        [Authorize(Roles = "Prestador")]
        public IActionResult AnticonceptivosAgrupadoUltimos12Meses()
        {
            var userId = _userManager.GetUserId(User);

            var prestador = _contenedorTrabajo.Prestadores
                .GetFirstOrDefault(p => p.UserID.ToString() == userId); // ajusta si tu relación es diferente

            if (prestador == null) return Forbid();

            var codigoPrestador = prestador.Codigo;

            var q = _contenedorTrabajo.AtencionesRda.GetAll()
                .Where(x => x.CodigoPrestador == codigoPrestador
                            && x.FechaAtencion.HasValue);


            // Trae agrupación por mes/método (solo lo necesario)
            var dataAgrupada = q
                .Where(x => x.MetodoAnticonceptivo != null && x.MetodoAnticonceptivo.Trim() != "")
                .GroupBy(x => new
                {
                    Year = x.FechaAtencion.Value.Year,
                    Month = x.FechaAtencion.Value.Month,
                    Metodo = x.MetodoAnticonceptivo.Trim()
                })
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Metodo, Total = g.Count() })
                .ToList();

            if (!dataAgrupada.Any())
                return Json(new { labels = Array.Empty<string>(), datasets = Array.Empty<object>() });

            // ✅ Mes base: mes actual si existe, si no el último con data
            var hoy = DateTime.Today;
            bool existeMesActual = dataAgrupada.Any(x => x.Year == hoy.Year && x.Month == hoy.Month);

            var ultimoConData = dataAgrupada
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .First();

            var mesBase = existeMesActual
                ? new DateTime(hoy.Year, hoy.Month, 1)
                : new DateTime(ultimoConData.Year, ultimoConData.Month, 1);

            var meses = Enumerable.Range(0, 12)
                .Select(i => mesBase.AddMonths(-i))
                .OrderBy(m => m)
                .ToList();

            var cultura = new CultureInfo("es-CO");
            var labels = meses.Select(m => m.ToString("MMM yyyy", cultura)).ToArray();

            // ✅ Top 5 métodos del periodo (por total)
            var topMetodos = dataAgrupada
                .GroupBy(x => x.Metodo)
                .Select(g => new { Metodo = g.Key, Total = g.Sum(z => z.Total) })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .Select(x => x.Metodo)
                .ToList();

            // Paleta simple (Chart.js)
                var bg = new[]
                {
                    "rgba(78,115,223,0.25)",
                    "rgba(28,200,138,0.25)",
                    "rgba(246,194,62,0.25)",
                    "rgba(231,74,59,0.25)",
                    "rgba(54,185,204,0.25)",
                    "rgba(133,135,150,0.25)"
                };
                        var br = new[]
                        {
                    "rgba(78,115,223,1)",
                    "rgba(28,200,138,1)",
                    "rgba(246,194,62,1)",
                    "rgba(231,74,59,1)",
                    "rgba(54,185,204,1)",
                    "rgba(133,135,150,1)"
                };

            // ✅ datasets por método (grouped bars)
            var datasets = new List<object>();

            for (int i = 0; i < topMetodos.Count; i++)
            {
                var metodo = topMetodos[i];

                var values = meses.Select(m =>
                    dataAgrupada
                        .Where(x => x.Year == m.Year && x.Month == m.Month && x.Metodo == metodo)
                        .Select(x => x.Total)
                        .FirstOrDefault()
                ).ToArray();

                datasets.Add(new
                {
                    label = metodo,
                    data = values,
                    backgroundColor = bg[i % bg.Length],
                    borderColor = br[i % br.Length],
                    borderWidth = 1
                });
            }

            // ✅ "Otros"
            var otrosValues = meses.Select(m =>
                dataAgrupada
                    .Where(x => x.Year == m.Year && x.Month == m.Month && !topMetodos.Contains(x.Metodo))
                    .Sum(x => x.Total)
            ).ToArray();

            if (otrosValues.Sum() > 0)
            {
                datasets.Add(new
                {
                    label = "Otros",
                    data = otrosValues,
                    backgroundColor = "rgba(133,135,150,0.25)",
                    borderColor = "rgba(133,135,150,1)",
                    borderWidth = 1
                });
            }

            return Json(new { labels, datasets });
        }

        [Authorize(Roles = "Admin,Prestador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> cerrarSesion() 
        {
            await _signInManager.SignOutAsync(); //Eliminar Cookie de applicacion
            TempData["CerrarSesion"] = "Que vuelvas pronto!";
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

    }
}
