using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Paises
    {
        [Key]
        public Guid PaisId { get; set; } = Guid.NewGuid();
        public int IdAntiguo { get; set; }
        public string Nombre{ get; set; }
        public string Codigo { get; set; }
        public string NormalizedName{ get; set; }
        public string  codigoInternacional { get; set; }
        public bool PermiteILVE { get; set; }
        public DateTime FechaCreacion {  get; set; }

        //Campos Foraneos
        public Guid EstadoId { get; set; }
        public Estados Estado { get; set; }

        public ICollection<TiposDocumentos>? TiposDocumentos { get; set; }
    }
}
