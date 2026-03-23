using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Models;
using Microsoft.AspNetCore.Http;

namespace CRMEsar.AccesoDatos.Services.Logs
{
    public class LogService : ILogService
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IHttpContextAccessor _httpContext;

        public LogService(IContenedorTrabajo contenedorTrabajo,
            IHttpContextAccessor httpContext)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _httpContext = httpContext;
        }

        public async Task RegistrarAsync<T>(string userId, string tipoAccion, T entidad, bool exitoso, string? respuesta = null)
            where T : class
        {
            var ip = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var userAgent = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString();

            var nombreTabla = typeof(T).Name;
            var recordIdProp = entidad?.GetType().GetProperty("Id");
            var recordId = recordIdProp?.GetValue(entidad)?.ToString();

            var log = new AspNetUserLogs
            {
                LogId = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                NombreTabla = nombreTabla,
                RecordId = recordId,
                IpAddress = ip,
                UserAgent = userAgent,
                Exitoso = exitoso,
                TipoAccion = tipoAccion,
                Respuesta = respuesta,
                FechaCreacion = DateTime.Now
            };
            _contenedorTrabajo.Log.Add(log);
            _contenedorTrabajo.Save();
        }
    }
}
