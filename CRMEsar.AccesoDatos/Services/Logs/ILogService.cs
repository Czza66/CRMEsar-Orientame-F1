using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.AccesoDatos.Services.Logs
{
    public interface ILogService
    {
        Task RegistrarAsync<T>(
            string userId,
            string tipoAccion,
            T entidad,
            bool exitoso,
            string? respuesta = null
            ) where T : class;
    }
}
