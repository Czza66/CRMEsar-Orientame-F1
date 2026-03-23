using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.AtencionesRda;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class AtencionesRdaController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AtencionesRdaController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils,
            IEliminadoLogicoService eliminadoLogicoService,
            UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
            _eliminadoLogicoService = eliminadoLogicoService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AtencionesRdaIndexVM vm)
        {
            // Si llega nulo por binding, asegura instancias
            vm ??= new AtencionesRdaIndexVM();
            vm.Filtros ??= new AtencionesRdaFiltrosVM();

            // OJO: Esto debe ser IQueryable idealmente.
            // Si tu repo devuelve IEnumerable, funcionará pero NO será óptimo con 1M.
            var q = _contenedorTrabajo.AtencionesRda.GetAll();

            if (User.IsInRole("Prestador"))
            {
                var codigo = await GetCodigoPrestadorAsync();
                if (!codigo.HasValue) return Forbid();

                q = q.Where(x => x.CodigoPrestador == codigo.Value);
            }

            // 1) Años (DateOnly.Year)
            var anios = q.Select(x => x.FechaAtencion.Value.Year)
                         .Distinct()
                         .OrderByDescending(x => x)
                         .ToList();

            var anioActual = DateTime.Now.Year;

            // Año por defecto: si existe el actual úsalo, si no usa el más reciente disponible
            var anioDefault = anios.Contains(anioActual)
                ? anioActual
                : anios.FirstOrDefault();

            if (vm.Filtros.AnioAtencion == null && anioDefault != 0)
                vm.Filtros.AnioAtencion = anioDefault;

            vm.Filtros.ListaAnios = anios
                .Select(a => new SelectListItem { Value = a.ToString(), Text = a.ToString() })
                .ToList();

            // 2) Distinct de strings (limpios y ordenados)
            vm.Filtros.ListaDonantes = DistinctToSelectList(q.Select(x => x.Donante));
            vm.Filtros.ListaTiposPrestador = DistinctToSelectList(q.Select(x => x.tipoPrestador));
            vm.Filtros.ListaPaises = DistinctToSelectList(q.Select(x => x.Pais));
            vm.Filtros.ListaGruposEtareos = DistinctToSelectList(q.Select(x => x.GrupoEtareo));
            vm.Filtros.ListaTiposConsulta = DistinctToSelectList(q.Select(x => x.TipoConsulta));

            // --- APLICAR FILTROS PARA CARGAR ITEMS ---
            if (vm.Filtros.AnioAtencion.HasValue)
                q = q.Where(x => x.FechaAtencion.Value.Year == vm.Filtros.AnioAtencion.Value);

            if (!string.IsNullOrWhiteSpace(vm.Filtros.Donante))
                q = q.Where(x => x.Donante == vm.Filtros.Donante);

            if (!string.IsNullOrWhiteSpace(vm.Filtros.TipoPrestador))
                q = q.Where(x => x.tipoPrestador == vm.Filtros.TipoPrestador);

            if (!string.IsNullOrWhiteSpace(vm.Filtros.Pais))
                q = q.Where(x => x.Pais == vm.Filtros.Pais);

            if (!string.IsNullOrWhiteSpace(vm.Filtros.GrupoEtareo))
                q = q.Where(x => x.GrupoEtareo == vm.Filtros.GrupoEtareo);

            if (!string.IsNullOrWhiteSpace(vm.Filtros.TipoConsulta))
                q = q.Where(x => x.TipoConsulta == vm.Filtros.TipoConsulta);

            // --- Proyección liviana (solo lo que muestra la tabla) ---
            vm.Items = q
                .OrderByDescending(x => x.FechaAtencion)
                .ThenByDescending(x => x.AtencionID)
                .Take(100)
                .Select(x => new AteencionesRdaTablaVM
                {
                    FechaAtencion = x.FechaAtencion,
                    Donante = x.Donante,
                    Pais = x.Pais,
                    NombrePrestador = x.NombrePrestador,
                    TipoPrestador = x.tipoPrestador,
                    GrupoEtareo = x.GrupoEtareo,
                    TipoConsulta = x.TipoConsulta,
                    IdentidadDisociada = x.IdentidadDisociada,
                    ILVE = x.ILVE,
                    Telemedicina = x.Telemedicina
                })
                .ToList();

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> GetData(
                int draw,
                int start,
                int length,
                int? anioAtencion,
                string? donante,
                string? tipoPrestador,
                string? pais,
                string? grupoEtareo,
                string? tipoConsulta)
        {
            // Base query
            var q = _contenedorTrabajo.AtencionesRda.GetAll();

            if (User.IsInRole("Prestador"))
            {
                var codigo = await GetCodigoPrestadorAsync();
                if (!codigo.HasValue) return Forbid();

                q = q.Where(x => x.CodigoPrestador == codigo.Value);
            }

            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            // Filtros maestros
            if (anioAtencion.HasValue)
                q = q.Where(x => x.FechaAtencion.Value.Year == anioAtencion.Value);

            if (!string.IsNullOrWhiteSpace(donante))
                q = q.Where(x => x.Donante == donante);

            if (!string.IsNullOrWhiteSpace(tipoPrestador))
                q = q.Where(x => x.tipoPrestador == tipoPrestador);

            if (!string.IsNullOrWhiteSpace(pais))
                q = q.Where(x => x.Pais == pais);

            if (!string.IsNullOrWhiteSpace(grupoEtareo))
                q = q.Where(x => x.GrupoEtareo == grupoEtareo);

            if (!string.IsNullOrWhiteSpace(tipoConsulta))
                q = q.Where(x => x.TipoConsulta == tipoConsulta);

            var recordsTotal = q.Count();

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                searchValue = searchValue.Trim();

                q = q.Where(x =>
                    (x.NombrePrestador != null && x.NombrePrestador.Contains(searchValue)) ||
                    (x.Donante != null && x.Donante.Contains(searchValue)) ||
                    (x.Pais != null && x.Pais.Contains(searchValue)) ||
                    (x.TipoConsulta != null && x.TipoConsulta.Contains(searchValue)) ||
                    (x.GrupoEtareo != null && x.GrupoEtareo.Contains(searchValue))
                );
            }

            var recordsFiltered = q.Count();

            // Orden dinámico según DataTables
            var orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var orderDir = Request.Form["order[0][dir]"].FirstOrDefault(); // asc/desc

            // Mapeo: índice de columna -> propiedad
            q = orderColumnIndex switch
            {
                "0" => (orderDir == "asc") ? q.OrderBy(x => x.FechaAtencion) : q.OrderByDescending(x => x.FechaAtencion),
                "2" => (orderDir == "asc") ? q.OrderBy(x => x.Donante) : q.OrderByDescending(x => x.Donante),
                "3" => (orderDir == "asc") ? q.OrderBy(x => x.Pais) : q.OrderByDescending(x => x.Pais),
                "4" => (orderDir == "asc") ? q.OrderBy(x => x.NombrePrestador) : q.OrderByDescending(x => x.NombrePrestador),
                _ => q.OrderByDescending(x => x.FechaAtencion)
                      .ThenByDescending(x => x.AtencionID)
            };

            // Paging
            var data = q.Skip(start).Take(length)
                .Select(x => new
                {
                    fechaAtencion = x.FechaAtencion.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    donante = x.Donante,
                    pais = x.Pais,
                    nombrePrestador = x.NombrePrestador,
                    tipoPrestador = x.tipoPrestador,
                    grupoEtareo = x.GrupoEtareo,
                    tipoConsulta = x.TipoConsulta,
                    identidadDisociada = x.IdentidadDisociada,
                    ilve = x.ILVE == null ? "-" : (x.ILVE.Value ? "Sí" : "No"),
                    telemedicina = x.Telemedicina == null ? "-" : (x.Telemedicina.Value ? "Sí" : "No"),
                    acciones = "<a class='btn btn-sm btn-outline-primary' href='javascript:void(0)'>Ver</a>"
                })
                .ToList();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            });
        }



        private static List<SelectListItem> DistinctToSelectList(IQueryable<string> source)
        {
            return source
                .Where(s => s != null && s != "")
                .Select(s => s.Trim())
                .Distinct()
                .OrderBy(s => s)
                .Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
        }

        private async Task<int?> GetCodigoPrestadorAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var prestador = _contenedorTrabajo.Prestadores.GetFirstOrDefault(p => p.UserID == user.Id).Codigo;
                return prestador;

        }
    }
}

