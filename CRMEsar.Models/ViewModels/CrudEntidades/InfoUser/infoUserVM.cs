using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.LogsUsuario;
using CRMEsar.Models.ViewModels.CrudEntidades.NotificacionesUsuario;

namespace CRMEsar.Models.ViewModels.CrudEntidades.InfoUser
{
    public class infoUserVM
    {
        public ApplicationUser usuario { get; set; }
        public IEnumerable<NotificacionesTablaVM> notificaciones { get; set; }
        public IEnumerable<LogsUsuarioTablaVM> logs { get; set; }
    }
}
