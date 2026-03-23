using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.ModulosSecciones;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class ModulosSeccionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;

        private const string NombreEntidadNormalizado = "MODULOSSECIONES";

        public ModulosSeccionesController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils,
            IEliminadoLogicoService eliminadoLogicoService)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
            _eliminadoLogicoService = eliminadoLogicoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var modSecciones = _contenedorTrabajo.ModulosSecciones.GetAll(
                includeProperties: "Estado")
                .Where(e=>e.Estado.Nombre!="Eliminado")
                .Select(e => new ModulosSeccionesTablaVM
                {
                    nombre = e.nombre,
                    descripcion =e.descripcion,
                    orden = e.orden,
                    visible = e.visible,   
                    area = e.area, 
                    controller = e.controller,
                    action = e.action,
                    moduloSeccionIdEncriptado = _protectorUtils.EncriptarGuid(e.seccionId,"moduloSeccion")
                });
            return View(modSecciones);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var listaSecciones = _contenedorTrabajo.ModulosSecciones.GetListaSecciones().ToList();
            // Agregar la opción vacía al inicio de la lista de secciones
            listaSecciones.Insert(0, new SelectListItem
            {
                Text = "-- Sin sección padre --",
                Value = ""
            });
            var viewModelSecciones = new ModulosSeccionesCrearEditarVM
            {
                ListaModulos = _contenedorTrabajo.Modulo.GetListaModulos(),
                ListaSecciones = listaSecciones
            };

            return View(viewModelSecciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ModulosSeccionesCrearEditarVM modCrearVM) 
        {
            if (ModelState.IsValid)
            {
                var seccion = new ModulosSecciones
                {
                    seccionId = Guid.NewGuid(),
                    nombre = modCrearVM.nombre,
                    descripcion = modCrearVM.descripcion,
                    orden = modCrearVM.orden,
                    visible = true,
                    area = modCrearVM.area,
                    controller = modCrearVM.controller,
                    action = modCrearVM.action,
                    EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Activo" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId,
                    ModuloId = modCrearVM.ModuloId,
                    seccionPadreId = modCrearVM.SeccionPadreId
                };
                _contenedorTrabajo.ModulosSecciones.Add(seccion);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        // controlador efitar Get
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var idReal = _protectorUtils.DesencriptarGuid(id, "moduloSeccion");
            var moduloSeccion = _contenedorTrabajo.ModulosSecciones.GetFirstOrDefault(
                e => e.seccionId == idReal,
                includeproperties: "Estado,Modulo"
            );
            if (moduloSeccion == null)
            {
                return NotFound();
            }
            var listaSecciones = _contenedorTrabajo.ModulosSecciones.GetListaSecciones().ToList();
            // Agregar la opción vacía al inicio de la lista de secciones
            listaSecciones.Insert(0, new SelectListItem
            {
                Text = "-- Sin sección padre --",
                Value = ""
            });
            var viewModel = new ModulosSeccionesCrearEditarVM
            {
                seccionId = _protectorUtils.EncriptarGuid(idReal, "moduloSeccion"),
                nombre = moduloSeccion.nombre,
                descripcion = moduloSeccion.descripcion,
                orden = moduloSeccion.orden,
                visible = moduloSeccion.visible,
                area = moduloSeccion.area,
                controller = moduloSeccion.controller,
                action = moduloSeccion.action,
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado),
                ListaModulos = _contenedorTrabajo.Modulo.GetListaModulos(),
                ListaSecciones = listaSecciones
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ModulosSeccionesCrearEditarVM seccionVM)
        {
            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(seccionVM.seccionId, "moduloSeccion");
                var seccion = new ModulosSecciones
                {
                    seccionId = idReal,
                    nombre = seccionVM.nombre,
                    descripcion = seccionVM.descripcion,
                    orden = seccionVM.orden,
                    area = seccionVM.area,
                    controller = seccionVM.controller,
                    action = seccionVM.action,
                    EstadoId = seccionVM.EstadoID,
                    ModuloId = seccionVM.ModuloId,
                    seccionPadreId = seccionVM.SeccionPadreId,
                    visible = true
                };
                _contenedorTrabajo.ModulosSecciones.update(seccion);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Seccion actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            var listaSecciones = _contenedorTrabajo.ModulosSecciones.GetListaSecciones().ToList();
            // Agregar la opción vacía al inicio de la lista de secciones
            listaSecciones.Insert(0, new SelectListItem
            {
                Text = "-- Sin sección padre --",
                Value = ""
            });
            seccionVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
            seccionVM.ListaSecciones = listaSecciones;
            seccionVM.ListaModulos = _contenedorTrabajo.Modulo.GetListaModulos();
            return View(seccionVM);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "moduloSeccion");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<ModulosSecciones>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

    }
}
