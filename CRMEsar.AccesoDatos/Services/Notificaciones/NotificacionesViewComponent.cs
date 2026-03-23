using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMEsar.AccesoDatos.Services.Notificaciones
{
    public class NotificacionesViewComponent : ViewComponent
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificacionesViewComponent(IContenedorTrabajo contenedorTrabajo,
            UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo  = contenedorTrabajo;
            _userManager = userManager; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var todas = await _contenedorTrabajo.Notificaciones.GetAllAsync(
                n => n.UserId == user.Id && !n.EstaLeido,
                includeProperties: "TipoNotificaciones");

            var modelo = new NotificacioneVM
            {
                TotalNotificaciones = todas.Count(),
                notificaciones = todas.OrderByDescending(n => n.FechaCreacion).ToList()
            };

            return View(modelo);
        }
    }
}
