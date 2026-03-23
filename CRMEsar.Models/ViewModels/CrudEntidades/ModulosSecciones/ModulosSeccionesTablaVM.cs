using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.ModulosSecciones
{
    public class ModulosSeccionesTablaVM
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public bool visible { get; set; }
        public string area { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string moduloSeccionIdEncriptado { get; set; }
    }
}
