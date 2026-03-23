using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Integraciones;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class IntegracionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;

        public IntegracionesController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var integracion = _contenedorTrabajo.Integraciones.GetAll(
                ).Select(i => new IntegracionesTablaVM
                {
                    TipoIntegracion = i.TipoIntegracion,
                    Nombre = i.Nombre,
                    Activa = i.Activa,
                    IntegracionIdEncriptado = _protectorUtils.EncriptarGuid(i.IntegracionId, "integracion")
                });
            return View(integracion);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModulIntegraciones = new IntegracionesCrearEditarVM();
            return View(viewModulIntegraciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IntegracionesCrearEditarVM integracionVM)
        {
            if (ModelState.IsValid)
            {
                var integraciones = new Integraciones
                {
                    IntegracionId = Guid.NewGuid(),
                    TipoIntegracion = integracionVM.tipoIntegracion,
                    Nombre = integracionVM.nombre,
                    EndpointURL = integracionVM.EndPointURL,
                    metodoHttp = integracionVM.metodoHTTP,
                    JsonSchema = integracionVM.JsonSchema,
                    Activa = true,
                    fechaCreacion = DateTime.Now,
                };
                _contenedorTrabajo.Integraciones.Add(integraciones);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            var idReal = _protectorUtils.DesencriptarGuid(id, "integracion");
            var integracion = _contenedorTrabajo.Integraciones.GetFirstOrDefault(
                i => i.IntegracionId == idReal
                );
            if (integracion == null) 
            {
                return NotFound();
            }
            var viewModel = new IntegracionesCrearEditarVM
            {
                integracionId = _protectorUtils.EncriptarGuid(idReal, "Integracion"),
                tipoIntegracion = integracion.TipoIntegracion,
                nombre = integracion.Nombre,
                EndPointURL = integracion.EndpointURL,
                metodoHTTP = integracion.metodoHttp,
                JsonSchema = integracion.JsonSchema,
                activa = integracion.Activa,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(IntegracionesCrearEditarVM integracionVM) 
        {
            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(integracionVM.integracionId, "Integracion");
                var integracion = new Integraciones
                {
                    IntegracionId = idReal,
                    EndpointURL = integracionVM.EndPointURL,
                    metodoHttp = integracionVM.metodoHTTP,
                    JsonSchema = integracionVM.JsonSchema,
                    Activa = integracionVM.activa,
                    Nombre = integracionVM.nombre,
                    TipoIntegracion = integracionVM.tipoIntegracion
                };
                _contenedorTrabajo.Integraciones.update(integracion);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Integracion actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(integracionVM);
        }
    }
}
