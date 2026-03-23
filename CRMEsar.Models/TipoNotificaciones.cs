using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class TipoNotificaciones
    {
        [Key]
        public Guid tipoNotificacionId { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; }
        public string NormalizedName { get; set; }
        public string ColorHexadecimal { get; set; }
        public string icono { get; set; }
    }
}
