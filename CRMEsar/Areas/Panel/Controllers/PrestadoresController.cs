using System.CodeDom;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.Prestadores;
using CRMEsar.Services.Prestadores;
using CRMEsar.Services.Usuarios;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class PrestadoresController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;
        private readonly IUsuarioServices _usuarioServices;
        private readonly IPrestadorCodigoService _prestadorCodigoService;

        private const string NombreEntidadNormalizado = "ESTADOSPRESTADORES";

        public PrestadoresController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils,
            IEliminadoLogicoService eliminadoLogicoService,
            IUsuarioServices usuarioServices,
            IPrestadorCodigoService prestadorCodigoService)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
            _eliminadoLogicoService = eliminadoLogicoService;
            _usuarioServices = usuarioServices;  
            _prestadorCodigoService = prestadorCodigoService;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            var prestadores = _contenedorTrabajo.Prestadores.GetAll(
                includeProperties: "Estado,User"
                ).Where(p => p.Estado.NormalizedName != "ELIMINADO")
                .Select(p => new PrestadoresTablaVM
                {
                    NombreCompleto = p.User.NombreCompleto,
                    Correo = p.User.Email,
                    Profesion = p.Profesion,
                    TipoServicio = p.TipoServicio,
                    TipoPrestador = p.TipoPrestador,
                    Estado = p.Estado.Nombre,

                    PrestadorIdEncriptado = _protectorUtils.EncriptarGuid(p.PrestadorId,"PrestadorId")
                });

            return View(prestadores);
        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var model = new PrestadorCreateVM
            {
                UsuariosDisponibles = await _usuarioServices.UsuariosDisponiblesPrestadores(),
                ListaTipoPrestador = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Seleccione un tipo" },
                    new SelectListItem { Value = "Presencial", Text = "Presencial" },
                    new SelectListItem { Value = "Virtual", Text = "Virtual" },
                    new SelectListItem { Value = "Semi Presencial", Text = "Semi Presencial" }
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestadorCreateVM model) 
        {
            if (string.IsNullOrEmpty(model.UserIdSeleccionado))
            {
                ModelState.AddModelError(nameof(model.UserIdSeleccionado),
                    "Debe seleccionar un usuario para crear la prestadora.");
            }
            if (!ModelState.IsValid) 
            {
                model.UsuariosDisponibles = await _usuarioServices.UsuariosDisponiblesPrestadores();

                model.ListaTipoPrestador = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Seleccione un tipo" },
                    new SelectListItem { Value = "Presencial", Text = "Presencial" },
                    new SelectListItem { Value = "Virtual", Text = "Virtual" },
                    new SelectListItem { Value = "Semi Presencial", Text = "Semi Presencial" }
                };

                return View(model);
            }

            var IdReal = _protectorUtils.DesencriptarGuid(model.UserIdSeleccionado, "PrestadorId");
            var existePrestador = _contenedorTrabajo.Prestadores
                                    .GetAll()
                                    .Any(p => p.UserID == IdReal);

            if (existePrestador)
            {
                ModelState.AddModelError("",
                    "El usuario seleccionado ya tiene una prestadora asociada.");

                model.UsuariosDisponibles =
                    await _usuarioServices.UsuariosDisponiblesPrestadores();

                model.ListaTipoPrestador = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Seleccione un tipo" },
                    new SelectListItem { Value = "Presencial", Text = "Presencial" },
                    new SelectListItem { Value = "Virtual", Text = "Virtual" },
                    new SelectListItem { Value = "Semi Presencial", Text = "Semi Presencial" }
                };

                return View(model);
            }

            int codigoPrestador = _prestadorCodigoService.ObtenerCodigoDisponible();

            var prestador = new Prestadores
            {
                PrestadorId = Guid.NewGuid(),
                UserID = IdReal,
                TipoPrestador = model.TipoPrestador,
                TipoServicio = model.TipoServicio,
                Profesion = model.Profesion,
                RecomendadoPor = model.RecomendadoPor,
                Telefono = model.Telefono,
                Celular = model.Celular,
                Orientador = model.Orientador,
                PermiteILVE = model.PermiteILVE,
                Codigo = codigoPrestador,
                EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Perteneciente" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId
            };
            _contenedorTrabajo.Prestadores.Add(prestador);
            _contenedorTrabajo.Save();
            TempData["RespuestaOperacion"] = "Prestadora creada correctamente";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "PrestadorId");

            var prestador = _contenedorTrabajo.Prestadores.GetFirstOrDefault(
                p => p.PrestadorId == IdReal,
                includeproperties: "Estado,User.Pais"
                );
            if (prestador == null) 
            {
                return NotFound();
            }

            var VM = new PrestadoresEditarVM
            {
                PrestadorId = _protectorUtils.EncriptarGuid(IdReal, "PrestadorEdit"),
                Profesion = prestador.Profesion,
                RecomendadoPor = prestador.RecomendadoPor,
                Telefono = prestador.Telefono,
                Celular = prestador.Celular,
                Orientador = prestador.Orientador,
                PermiteILVE = prestador.PermiteILVE,
                TipoPrestador = prestador.TipoPrestador,
                EstadoId = prestador.EstadoId,
                ListaTipoPrestador = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Presencial", Text = "Presencial" },
                    new SelectListItem { Value = "Virtual", Text = "Virtual" },
                    new SelectListItem { Value = "Semi Presencial", Text = "Semi Presencial" }
                },
                NombreCompleto = prestador.User.NombreCompleto,
                Correo = prestador.User.Email,
                Pais = prestador.User.Pais.Nombre,
                Ciudad = prestador.User.Ciudad,

                ListaEstados = _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado)
            };
            return View(VM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PrestadoresEditarVM VM)
        {
            Guid idReal = _protectorUtils.DesencriptarGuid(VM.PrestadorId, "PrestadorEdit");

            if (!ModelState.IsValid)
            {
                VM.ListaEstados =
                    _contenedorTrabajo.Estado.GetListaEstados(NombreEntidadNormalizado);

                VM.ListaTipoPrestador = new List<SelectListItem>
        {
            new SelectListItem { Value = "Presencial", Text = "Presencial" },
            new SelectListItem { Value = "Virtual", Text = "Virtual" },
            new SelectListItem { Value = "Semi Presencial", Text = "Semi Presencial" }
        };

                return View(VM); 
            }

            var prestadorBD = _contenedorTrabajo.Prestadores
                .GetFirstOrDefault(p => p.PrestadorId == idReal);

            if (prestadorBD == null)
            {
                return NotFound();
            }

            prestadorBD.Profesion = VM.Profesion;
            prestadorBD.RecomendadoPor = VM.RecomendadoPor;
            prestadorBD.Telefono = VM.Telefono;
            prestadorBD.Celular = VM.Celular;
            prestadorBD.Orientador = VM.Orientador;
            prestadorBD.PermiteILVE = VM.PermiteILVE;
            prestadorBD.TipoPrestador = VM.TipoPrestador;
            prestadorBD.EstadoId = VM.EstadoId;

            _contenedorTrabajo.Prestadores.Update(prestadorBD);
            _contenedorTrabajo.Save();
            TempData["RespuestaOperacion"] =
                "Prestadora actualizada correctamente";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var IdReal = _protectorUtils.DesencriptarGuid(id, "PrestadorId");
            var EstadoId = _contenedorTrabajo.Estado.GetFirstOrDefault(e => e.Nombre == "Eliminado" && e.Entidad.NormalizedName == NombreEntidadNormalizado, includeproperties: "Entidad").EstadoId;

            _eliminadoLogicoService.EliminadoLogico<Prestadores>(IdReal, EstadoId);
            TempData["RespuestaOperacion"] = "Registro eliminado correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}
