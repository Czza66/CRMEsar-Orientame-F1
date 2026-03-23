using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;

namespace CRMEsar.AccesoDatos.Services.Notificaciones
{
    public class NotificacionesService : INotificacionesService
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public NotificacionesService(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }
        public async Task CrearNotificacionAsync(string userId, string titulo, string mensaje, string nombreTabla, string tipoNotificacionNormalizedName)
        {
            var tipo = _contenedorTrabajo.TipoNotificaciones
                .GetFirstOrDefault(t => t.NormalizedName == tipoNotificacionNormalizedName);

            if (tipo == null)
                throw new Exception($"Tipo de notificación '{tipoNotificacionNormalizedName}' no encontrado.");

            var notificacion = new Models.Notificaciones
            {
                NotificacionId = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                Titulo = titulo.ToUpper(),
                Mensaje = mensaje,
                NombreTabla = nombreTabla,
                EstaLeido = false,
                FechaCreacion = DateTime.Now,
                TipoNotificacionId = tipo.tipoNotificacionId
            };

            _contenedorTrabajo.Notificaciones.Add(notificacion);
            _contenedorTrabajo.Save();
        }
    }
}
