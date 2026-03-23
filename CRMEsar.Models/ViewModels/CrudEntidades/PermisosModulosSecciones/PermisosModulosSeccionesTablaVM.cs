using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.PermisosModulosSecciones
{
    public class PermisosModulosSeccionesTablaVM
    {
        public string Modulo { get; set; }
        public string Seccion { get; set; }
        public bool Temporal { get; set; }
        public string Rol { get; set; }
        public string PermisomoduloSeccionIdEncriptado { get; set; }
    }
}
