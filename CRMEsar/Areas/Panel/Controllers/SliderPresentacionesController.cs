using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.AccesoDatos.Services.EliminadoLogico;
using CRMEsar.Models;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;
using CRMEsar.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class SliderPresentacionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly ProtectorUtils _protectorUtils;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEliminadoLogicoService _eliminadoLogicoService;

        public SliderPresentacionesController(IContenedorTrabajo contenedorTrabajo,
            ProtectorUtils protectorUtils,
            IWebHostEnvironment webHostEnvironment,
            IEliminadoLogicoService eliminadoLogicoService)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _protectorUtils = protectorUtils;
            _webHostEnvironment = webHostEnvironment;
            _eliminadoLogicoService = eliminadoLogicoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sliders = _contenedorTrabajo.SliderPresentaciones.GetAll(
                ).Select(s => new SliderPresentacionesVM
                {
                    ImagenRuta = s.ImagenRuta,
                    Titulo = s.Titulo,
                    Descripcion = s.Descripcion,
                    Orden = s.Orden,
                    Activo =  s.Activo,
                    SliderPresentacionEncriptado = _protectorUtils.EncriptarGuid(s.SliderId, "slider")
                });
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            var viewModulSlidersPresentaciones = new SliderPresentacionesCrearEditarVM();
            return View(viewModulSlidersPresentaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SliderPresentacionesCrearEditarVM sliderVM)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos == null || archivos.Count == 0)
                {
                    ModelState.AddModelError("", "Debe subir una imagen para el slider.");
                    return View(sliderVM);
                }

                string rutaPrincipalArchivoSlider = _webHostEnvironment.WebRootPath;
                var subidas = Path.Combine(rutaPrincipalArchivoSlider, @"img\sliders");
                
                var extension = Path.GetExtension(archivos[0].FileName);
                string nombreArchivoSlider = Guid.NewGuid().ToString();

                string rutaFinal = Path.Combine(subidas, nombreArchivoSlider);

                using (var fileStreams = new FileStream(Path.Combine(subidas,nombreArchivoSlider + extension), FileMode.Create)) 
                {
                    archivos[0].CopyTo(fileStreams);
                }

                    var slider = new SliderPresentaciones
                    {
                        SliderId = Guid.NewGuid(),
                        Titulo = sliderVM.Titulo,
                        Descripcion = sliderVM.Descripcion,
                        ImagenRuta = @"\img\sliders\"+nombreArchivoSlider+extension,
                        TextoAlternativo = sliderVM.TextoAlternativo,
                        UrlDestino = sliderVM.UrlDestino,
                        Orden = sliderVM.Orden,
                        Activo = true,
                        FechaCreacion = DateTime.Now,
                    };

                _contenedorTrabajo.SliderPresentaciones.Add(slider);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Slider creado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            var idReal = _protectorUtils.DesencriptarGuid(id, "slider");
            var slider = _contenedorTrabajo.SliderPresentaciones.GetFirstOrDefault(
                s => s.SliderId == idReal
                );
            if (slider == null)
            {
                return NotFound();  
            }

            var ViewModel = new SliderPresentacionesCrearEditarVM
            {
                sliderId = _protectorUtils.EncriptarGuid(idReal, "slider"),
                ImagenRuta = slider.ImagenRuta,
                Titulo = slider.Titulo,
                Descripcion = slider.Descripcion,
                TextoAlternativo = slider.TextoAlternativo,
                UrlDestino = slider.UrlDestino,
                Activo = slider.Activo
            };

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SliderPresentacionesCrearEditarVM SliderVm) 
        {
            if (ModelState.IsValid) 
            {
                Guid idReal = _protectorUtils.DesencriptarGuid(SliderVm.sliderId, "slider");
                var slider = new SliderPresentaciones
                {
                    SliderId = idReal,
                    Titulo = SliderVm.Titulo,
                    Descripcion=SliderVm.Descripcion,
                    TextoAlternativo=SliderVm.TextoAlternativo,
                    UrlDestino=SliderVm.UrlDestino, 
                    Orden=SliderVm.Orden,
                    Activo=SliderVm.Activo,
                    ImagenRuta = SliderVm.ImagenRuta
                };
                _contenedorTrabajo.SliderPresentaciones.update(slider);
                _contenedorTrabajo.Save();
                TempData["RespuestaOperacion"] = "Slider actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(SliderVm);
        }

    }
}
