using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Cargos;
using CRMEsar.Models.ViewModels.CrudEntidades.Paises;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class CargosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;

        private const string NombreEntidadNormalizado = "ESTADOSREGISTROS";

        public CargosController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils,
            IEliminadoLogicoService eliminadoLogicoService)
        {
            _contenedorTrabajo  = contenedorTrabajo;
            _protectorUtils = protectorUtils;
            _eliminadoLogicoService = eliminadoLogicoService;
        }

        public IActionResult Index()
        {
            var cargo = _contenedorTrabajo.Cargos.GetAll(
                includeProperties: "Estado"
                ).Where(p => p.Estado.NormalizedName != "ELIMINADO")
                .Select(p => new CargosTablaVM
                {
                    Cargo = p.Cargo,
                    Abreviatura = p.Abreviatura,
                    Estado = p.Estado.Nombre,
                    CargoIdEncriptado = _protectorUtils.EncriptarGuid(p.CargoId, "Cargo")
                });
            return View(cargo);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CargosAgregarEditarVM cargosVM)
        {
            if (ModelState.IsValid)
            {
                var cargo = new Cargos
                {
                    CargoId = Guid.NewGuid(),
                    Cargo = cargosVM.Cargo,
                    Abreviatura = cargosVM.Abreviatura,
                    IdAntiguo = 0,
                    EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Activo" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId
                };
                _contenedorTrabajo.Cargos.Add(cargo);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Cargo creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "Cargo");

            var cargo = _contenedorTrabajo.Cargos.GetFirstOrDefault(
                P => P.CargoId == IdReal,
                includeproperties: "Estado"
                );

            if (cargo == null)
            {
                return NotFound();

            }

            var ViewModel = new CargosAgregarEditarVM
            {
                CargoId = _protectorUtils.EncriptarGuid(IdReal, "cargoEdit"),
                Cargo = cargo.Cargo,
                Abreviatura = cargo.Abreviatura,
                EstadoId = cargo.EstadoId,
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado)
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CargosAgregarEditarVM cargosVM)
        {

            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(cargosVM.CargoId, "cargoEdit");

                var cargo = new Cargos
                {
                    CargoId = idReal,
                    Cargo = cargosVM.Cargo,
                    Abreviatura = cargosVM.Abreviatura,
                    EstadoId = cargosVM.EstadoId
                };
                _contenedorTrabajo.Cargos.update(cargo);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Cargo actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            cargosVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
            return View(cargosVM);
        }

        [HttpPost]
        public IActionResult Delete(String id) 
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "Cargo");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<Cargos>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

    }
}
