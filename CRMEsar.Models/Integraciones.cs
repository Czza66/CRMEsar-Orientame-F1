using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Integraciones
    {
        [Key]
        public Guid IntegracionId { get; set; } = Guid.NewGuid();
        public string TipoIntegracion { get; set; }
        public string Nombre { get; set; }
        public string EndpointURL { get; set; }
        public string metodoHttp { get; set; }
        public string JsonSchema { get; set; }
        public bool Activa { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
