using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class AspNetUserLogs
    {
        [Key]
        public Guid LogId { get; set; } = Guid.NewGuid();
        public string TipoAccion { get; set; }
        public string NombreTabla { get; set; }
        public string RecordId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public bool Exitoso { get; set; }
        public string Respuesta { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Llaves Foraneas 
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
