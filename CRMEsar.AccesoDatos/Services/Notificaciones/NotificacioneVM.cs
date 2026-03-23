using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.AccesoDatos.Services.Notificaciones
{
    public class NotificacioneVM
    {
        public string notificacionId { get; set; }
        public int TotalNotificaciones { get; set; }
        public List<Models.Notificaciones> notificaciones { get; set; }  
    }
}
