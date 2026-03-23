using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CRMEsar.Models.ViewModels.CrudEntidades.LogsUsuario
{
    public class LogsUsuarioTablaVM
    {
        public string NombreTabla { get; set; }
        public bool exitoso {  get; set; }
        public string respuesta{ get; set; }
        public string fechaCreacion { get; set; }
        public string tipoAccion {  get; set; }
    }
}
