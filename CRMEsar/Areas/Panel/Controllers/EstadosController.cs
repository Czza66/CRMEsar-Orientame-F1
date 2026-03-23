using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades;
using CRMEsar.Models.ViewModels.CrudEntidades.Estados;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class EstadosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;

        public EstadosController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var estados = _contenedorTrabajo.Estado.GetAll(
                includeProperties: "Entidad"
                ).Select(e => new EstadosTablaVM
                {
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    EntidadNombre = e.Entidad.Nombre,
                    FechaCreacion = e.FechaCreacion,
                    Activo = e.Activo,
                    EstadoIdEncriptado = _protectorUtils.EncriptarGuid(e.EstadoId,"Estado")
                }).ToList();
            return View(estados);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            EstadosAgregarEditarVM estadosVM = new EstadosAgregarEditarVM()
            {
                ListaEntidades = _contenedorTrabajo.Entidad.GetListaEntidades()
            };
            return View(estadosVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EstadosAgregarEditarVM estadosVM) 
        {
            if (ModelState.IsValid)
            {
                var estado = new Estados
                {
                    EstadoId = Guid.NewGuid(),
                    Nombre = estadosVM.Nombre,
                    NormalizedName = estadosVM.Nombre.ToUpperInvariant(),
                    Descripcion = estadosVM.Descripcion,
                    EntidadId = estadosVM.EntidadId,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                };
                _contenedorTrabajo.Estado.Add(estado);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            estadosVM.ListaEntidades = _contenedorTrabajo.Entidad.GetListaEntidades();
            return View(estadosVM);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            // Obtener el estado por su Id, incluyendo la Entidad relacionada
            var IdReal=  _protectorUtils.DesencriptarGuid(id, "Estado");

            var estado = _contenedorTrabajo.Estado.GetFirstOrDefault(
                e => e.EstadoId == IdReal,
                includeproperties: "Entidad"
            );

            if (estado == null)
            {
                return NotFound(); // 404 si no existe
            }

            var viewModel = new EstadosAgregarEditarVM
            {
                EstadoId = _protectorUtils.EncriptarGuid(IdReal,"EstadoEdit"),
                Nombre = estado.Nombre,
                Descripcion = estado.Descripcion,
                EntidadId = estado.EntidadId,
                ListaEntidades = _contenedorTrabajo.Entidad.GetListaEntidades()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EstadosAgregarEditarVM estadoVM)
        {
            if (ModelState.IsValid)
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(estadoVM.EstadoId, "EstadoEdit");

                var estado = new Estados
                {
                    EstadoId = idReal,
                    Nombre = estadoVM.Nombre,
                    Descripcion = estadoVM.Descripcion,
                    NormalizedName = estadoVM.Nombre.ToUpperInvariant()
                };
                _contenedorTrabajo.Estado.Update(estado);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Estado actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            estadoVM.ListaEntidades = _contenedorTrabajo.Entidad.GetListaEntidades();
            return View(estadoVM);
        }
    }
}
