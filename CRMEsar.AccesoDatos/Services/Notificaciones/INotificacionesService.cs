using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.AccesoDatos.Services.Notificaciones
{
    public interface INotificacionesService
    {
        Task CrearNotificacionAsync(string userId, string titulo, string mensaje, string nombreTabla, string tipoNotificacionNormalizedName);
    }
}
