using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.TipoNotificaciones
{
    public class TipoNotificacionesTablaVM
    {
        public string nombre { get; set; }
        public string colorHexadeciaml { get; set; } 
        public string icono { get; set; }
        public string TipoNotificacionIdEncriptado { get; set; }
    }
}
