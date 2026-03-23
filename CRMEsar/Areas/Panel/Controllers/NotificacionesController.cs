using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Admin,Prestador")]
    public class NotificacionesController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public NotificacionesController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }


        [HttpGet]
        public async Task<IActionResult> Leer(Guid id)
        {
            var notificacion = _contenedorTrabajo.Notificaciones.Get(id);
            if (notificacion == null) return NotFound();

            notificacion.EstaLeido = true;
            _contenedorTrabajo.Notificaciones.update(notificacion);
            _contenedorTrabajo.Save();

            return RedirectToAction("Index", "Home", new { area = "Panel" });
        }
    }
}
