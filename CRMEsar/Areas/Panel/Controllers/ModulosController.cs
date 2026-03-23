using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Estados;
using CRMEsar.Models.ViewModels.CrudEntidades.Modulos;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class ModulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;

        private const string NombreEntidadNormalizado = "MODULOS";
        public ModulosController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var modulos = _contenedorTrabajo.Modulo.GetAll(
                includeProperties: "Estado"
                ).Select(e => new ModulosTablaVM
                {
                    Nombre = e.nombre,
                    DescripcionCorta = e.descripcionCorta,
                    Icono = e.icono,
                    visible = e.visible,
                    orden =e.orden,
                    ModuloIdEncriptado = _protectorUtils.EncriptarGuid(e.moduloId,"Modulo")
                });

            return View(modulos);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ModulosAgregarEditarVM modulosVM)
        {
            if (ModelState.IsValid)
            {
                var modulo = new Modulos
                {
                    moduloId = Guid.NewGuid(),
                    nombre = modulosVM.Nombre,
                    icono = modulosVM.icono,
                    descripcionCorta = modulosVM.DescripcionCorta,
                    orden = modulosVM.orden,
                    EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e=>e.Nombre=="Activo" && e.Entidad.NormalizedName==NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId
                };
                _contenedorTrabajo.Modulo.Add(modulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            // Obtener el estado por su Id, incluyendo la Entidad relacionada
            var IdReal = _protectorUtils.DesencriptarGuid(id, "Modulo");

            var modulo = _contenedorTrabajo.Modulo.GetFirstOrDefault(
                e => e.moduloId == IdReal,
                includeproperties: "Estado"
            );

            if (modulo == null)
            {
                return NotFound(); // 404 si no existe
            }

            var viewModel = new ModulosAgregarEditarVM
            {
                moduloId = _protectorUtils.EncriptarGuid(IdReal, "ModuloEdit"),
                Nombre = modulo.nombre,
                DescripcionCorta = modulo.descripcionCorta,
                icono = modulo.icono,
                orden = modulo.orden,
                visible = modulo.visible,
                EstadoId = modulo.EstadoId,
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado)

            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ModulosAgregarEditarVM moduloVM)
        {
            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(moduloVM.moduloId, "ModuloEdit");

                var modulo = new Modulos
                {
                    moduloId = idReal,
                    icono = moduloVM.icono,
                    nombre = moduloVM.Nombre,
                    descripcionCorta = moduloVM.DescripcionCorta,
                    orden = moduloVM.orden,
                    visible = moduloVM.visible,
                    EstadoId = moduloVM.EstadoId,
                };
                _contenedorTrabajo.Modulo.update(modulo);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Modulo actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            moduloVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
            return View(moduloVM);
        }
    }
}
