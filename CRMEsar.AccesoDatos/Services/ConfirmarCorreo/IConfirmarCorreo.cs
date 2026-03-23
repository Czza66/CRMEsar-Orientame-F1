using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Utilidades.ConfirmarCorreo
{
    public interface IConfirmarCorreo
    {
        Task EnviarConfirmacionCorreoAsync(string userId, string correo, string urlConfirmacion);
    }
}
