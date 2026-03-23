using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Modulos
{
    public class ModulosTablaVM
    {
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public string Icono { get; set; }
        public int orden { get; set; }
        public bool visible { get; set; }
        public string ModuloIdEncriptado { get; set; }
    }
}
