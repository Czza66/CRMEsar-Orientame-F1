using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Paises;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class PaisesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;

        private const string NombreEntidadNormalizado = "ESTADOSREGISTROS";

        public PaisesController(IContenedorTrabajo contenedorTrabajo,
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
            var paises = _contenedorTrabajo.Paises.GetAll(
                includeProperties: "Estado"
                ).Where(p=>p.Estado.NormalizedName != "ELIMINADO")
                .Select(p => new PaisesTablaVM
                {
                    Nombre = p.Nombre,
                    Codigo = p.Codigo,
                    PermiteILVE = p.PermiteILVE,
                    Estado = p.Estado.Nombre,
                    PaisIdEncriptado = _protectorUtils.EncriptarGuid(p.PaisId,"Pais")
                });
            return View(paises);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PaisesAgregarEditarVM paisVM) 
        {
            if (ModelState.IsValid) 
            {
                var pais = new Paises
                {
                    PaisId = Guid.NewGuid(),
                    Nombre = paisVM.Nombre,
                    NormalizedName = paisVM.Nombre.ToUpper(),
                    Codigo = paisVM.Codigo,
                    codigoInternacional = paisVM.codigoInternacional,
                    PermiteILVE = paisVM.PermiteILVE,
                    IdAntiguo = 0,
                    FechaCreacion = DateTime.Now,
                    EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Activo" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId
                };
                _contenedorTrabajo.Paises.Add(pais);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();  
        }

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "Pais");

            var pais = _contenedorTrabajo.Paises.GetFirstOrDefault(
                P=>P.PaisId==IdReal,
                includeproperties: "Estado"
                );

            if (pais==null)
            {
                return NotFound();
                
            }

            var ViewModel = new PaisesAgregarEditarVM
            {
                paisId = _protectorUtils.EncriptarGuid(IdReal, "PaisEdit"),
                Nombre = pais.Nombre,
                codigoInternacional = pais.codigoInternacional,
                Codigo = pais.Codigo,
                PermiteILVE = pais.PermiteILVE,
                EstadoId = pais.EstadoId,
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado)
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaisesAgregarEditarVM paisesVM) 
        {

            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(paisesVM.paisId, "PaisEdit");

                var pais = new Paises
                {
                    PaisId = idReal,
                    Nombre = paisesVM.Nombre,
                    NormalizedName = paisesVM.Nombre.ToUpper(),
                    codigoInternacional = paisesVM.codigoInternacional,
                    Codigo = paisesVM.Codigo,
                    PermiteILVE = paisesVM.PermiteILVE,
                    EstadoId = paisesVM.EstadoId
                };
                _contenedorTrabajo.Paises.update(pais);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Pais actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            paisesVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
            return View(paisesVM);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "Pais");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<Paises>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

    }
}
