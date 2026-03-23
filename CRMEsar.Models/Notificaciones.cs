using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Notificaciones
    {
        [Key]
        public Guid NotificacionId { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string NombreTabla { get; set; }
        public bool EstaLeido { get; set; }
        public DateTime FechaCreacion { get; set; }

        //Llaves foraneas
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid TipoNotificacionId { get; set; }

        [ForeignKey("TipoNotificacionId")]
        public TipoNotificaciones TipoNotificaciones { get; set; }
    }
}
