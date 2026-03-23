using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.ModulosSecciones;
using CRMEsar.Models.ViewModels.CrudEntidades.Paises;
using CRMEsar.Models.ViewModels.CrudEntidades.TiposDocumentos;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class TiposDocumentosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;

        private const string NombreEntidadNormalizado = "ESTADOSREGISTROS";

        public TiposDocumentosController(IContenedorTrabajo contenedorTrabajo,
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
            var tipDocument = _contenedorTrabajo.TiposDocumentos.GetAll(
                includeProperties: "Estado"
                ).Where(p => p.Estado.NormalizedName != "ELIMINADO" &&
                p.Pais.Estado.NormalizedName != "ELIMINADO")
                .Select(p => new TiposDocumentosTablaVM
                {
                    TipoDocumento = p.TipoDocumento,
                    Abreviatura = p.Abreviatura,
                    Pais = p.Pais.Nombre,
                    Estado = p.Estado.Nombre,
                    TipoDocumentoIdEncriptado = _protectorUtils.EncriptarGuid(p.TiposDocumentosId, "tipDocument")
                });
            return View(tipDocument);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var ListaPaises = _contenedorTrabajo.Paises.GetListaPaises().ToList();
            // Agregar la opción vacía al inicio de la lista de secciones
            ListaPaises.Insert(0, new SelectListItem
            {
                Text = "-- Selecciona un Pais --",
                Value = ""
            });
            var viewModelTiposDocuments = new TipoDocumentCrearEditarVM()
            {
                ListaPaises = ListaPaises
            };

            return View(viewModelTiposDocuments);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoDocumentCrearEditarVM TipDocVM) 
        {
            if (ModelState.IsValid) 
            {
                var tipDoc = new TiposDocumentos
                {
                    TiposDocumentosId = Guid.NewGuid(),
                    TipoDocumento = TipDocVM.TipoDocumento,
                    Abreviatura = TipDocVM.Abreviatura,
                    IdAntiguo = 0,
                    EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Activo" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId,
                    PaisId = _contenedorTrabajo.Paises.GetFirstOrDefault(e => e.Estado.Nombre == "Activo" && e.Estado.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Estado").PaisId
                };
                _contenedorTrabajo.TiposDocumentos.Add(tipDoc);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            var idReal = _protectorUtils.DesencriptarGuid(id, "tipDocument");

            var TipDocument = _contenedorTrabajo.TiposDocumentos.GetFirstOrDefault(
                t => t.TiposDocumentosId == idReal,
                includeproperties: "Estado"
                );

            if (TipDocument == null)
            {
                return NotFound();
            }

            var viewModel = new TipoDocumentCrearEditarVM
            {
                TiposDocumentosId = _protectorUtils.EncriptarGuid(idReal, "TipDocumentEdit"),
                TipoDocumento = TipDocument.TipoDocumento,
                Abreviatura = TipDocument.Abreviatura,
                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado),
                ListaPaises = _contenedorTrabajo.Paises.GetListaPaises()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TipoDocumentCrearEditarVM tipDocVM) 
        {
            if (ModelState.IsValid) 
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(tipDocVM.TiposDocumentosId, "TipDocumentEdit");

                var tipDocument = new TiposDocumentos
                {
                    TiposDocumentosId = idReal,
                    TipoDocumento = tipDocVM.TipoDocumento,
                    Abreviatura = tipDocVM.Abreviatura,
                    EstadoId = tipDocVM.EstadoId,
                    PaisId = tipDocVM.PaisId
                };
                _contenedorTrabajo.TiposDocumentos.update(tipDocument);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Tipo de documento actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            tipDocVM.ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);
            tipDocVM.ListaPaises = _contenedorTrabajo.Paises.GetListaPaises();
            return View(tipDocVM);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "tipDocument");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<TiposDocumentos>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }

    }
}
