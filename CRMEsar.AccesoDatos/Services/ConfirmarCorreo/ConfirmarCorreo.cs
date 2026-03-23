using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Text.Json;

namespace CRMEsar.Utilidades.ConfirmarCorreo
{
    public class ConfirmarCorreo : IConfirmarCorreo
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IHttpClientFactory _httpClientFactory;

        public ConfirmarCorreo(IContenedorTrabajo contenedorTrabajo,
            IHttpClientFactory httpClientFactory)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _httpClientFactory = httpClientFactory;
        }

        public async Task EnviarConfirmacionCorreoAsync(string userId, string correo, string urlConfirmacion) 
        {
            var integracion = await _contenedorTrabajo.Integraciones.GetFirstOrDefaultAsync(
                i => i.Activa &&
                i.TipoIntegracion == "POWERAUTOMATE" &&
                i.Nombre == "CONFIRMARCORREO");

            if (integracion == null) 
            {
                throw new InvalidOperationException("Integración CONFIRMARCORREO no encontrada o inactiva.");
            }

            var jsonPayLoad = new
            {
                UserID = userId,
                Correo = correo,
                URL = urlConfirmacion
            };

            var cliente = _httpClientFactory.CreateClient();
            var contenido = new StringContent(JsonSerializer.Serialize(jsonPayLoad), Encoding.UTF8, "application/json");

            var respuesta = await cliente.PostAsync(integracion.EndpointURL, contenido);

            if (!respuesta.IsSuccessStatusCode)
            {
                var mensaje = await respuesta.Content.ReadAsStringAsync();
                throw new ApplicationException($"Error al enviar correo de confirmación: {mensaje}");
            }
        }
    }
}
