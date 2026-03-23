using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Integraciones
{
    public class IntegracionesCrearEditarVM
    {
        public string? integracionId {  get; set; }
        public bool activa {  get; set; }
        public string tipoIntegracion { get; set; }
        public string nombre { get; set; }
        public string EndPointURL { get; set; }
        public string metodoHTTP { get; set; }
        public string? JsonSchema { get; set; }
    }
}
