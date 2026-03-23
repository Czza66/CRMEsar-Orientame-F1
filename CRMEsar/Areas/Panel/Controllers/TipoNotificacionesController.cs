using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.TipoNotificaciones;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class TipoNotificacionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;

        public TipoNotificacionesController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tipoNotificacion = _contenedorTrabajo.TipoNotificaciones.GetAll(
                ).Select(t => new TipoNotificacionesTablaVM
                {
                    nombre = t.Nombre,
                    colorHexadeciaml = t.ColorHexadecimal,
                    icono = t.icono,
                    TipoNotificacionIdEncriptado = _protectorUtils.EncriptarGuid(t.tipoNotificacionId,"TipoNotificacion")
                });

            return View(tipoNotificacion);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoNotificacionCrearEditarVM TipoVM) 
        {
            if (ModelState.IsValid) 
            {
                var tipo = new TipoNotificaciones
                {
                    tipoNotificacionId = Guid.NewGuid(),
                    Nombre = TipoVM.nombre,
                    NormalizedName = TipoVM.nombre.ToUpper(),
                    ColorHexadecimal = TipoVM.colorHexadecimal,
                    icono=TipoVM.icono
                };
                _contenedorTrabajo.TipoNotificaciones.Add(tipo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var idReal = _protectorUtils.DesencriptarGuid(id, "TipoNotificacion");
            var tipoNotifi = _contenedorTrabajo.TipoNotificaciones.GetFirstOrDefault(
                t => t.tipoNotificacionId == idReal
                );
            if (tipoNotifi == null)
            {
                return NotFound();
            }
            var viewModel = new TipoNotificacionCrearEditarVM
            {
                tipoNotificacionId = _protectorUtils.EncriptarGuid(idReal, "TipoNotificacion"),
                nombre = tipoNotifi.Nombre,
                colorHexadecimal = tipoNotifi.ColorHexadecimal,
                icono = tipoNotifi.icono

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TipoNotificacionCrearEditarVM tipoNotifi)
        {
            if (ModelState.IsValid) 
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(tipoNotifi.tipoNotificacionId, "TipoNotificacion");
                var tipoNotificacion = new TipoNotificaciones
                {
                   tipoNotificacionId = idReal,
                   Nombre = tipoNotifi.nombre,
                   NormalizedName = tipoNotifi.nombre.ToUpper(),
                   ColorHexadecimal = tipoNotifi.colorHexadecimal,
                   icono=tipoNotifi.icono
                };
                _contenedorTrabajo.TipoNotificaciones.update(tipoNotificacion);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Tipo notificacion actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(tipoNotifi);
        }
    }
}
