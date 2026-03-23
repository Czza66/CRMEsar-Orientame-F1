using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Estados
{
    public class EstadosTablaVM
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string EntidadNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public string EstadoIdEncriptado { get; set; }
    }
}
