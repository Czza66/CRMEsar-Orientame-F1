using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.NotificacionesUsuario
{
    public class NotificacionesTablaVM
    {
        public string titulo { get; set; }
        public string  mensaje { get; set; }
        public bool estaleido { get; set; }
        public string fecha { get; set; }
    }
}
