using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.TipoNotificaciones
{
    public class TipoNotificacionCrearEditarVM
    {
        public string? tipoNotificacionId {  get; set; }
        public string nombre {  get; set; } 
        public string colorHexadecimal { get; set; }
        public string icono { get; set; } 

    }
}
