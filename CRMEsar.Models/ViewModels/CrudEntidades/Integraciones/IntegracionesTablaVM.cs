using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Integraciones
{
    public class IntegracionesTablaVM
    {
        public string TipoIntegracion {  get; set; }
        public string Nombre { get; set; }
        public bool Activa {get; set; }
        public string IntegracionIdEncriptado { get; set; }
    }
}
