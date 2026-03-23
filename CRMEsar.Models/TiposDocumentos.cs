using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class TiposDocumentos
    {
        [Key]
        public Guid TiposDocumentosId { get; set; } = Guid.NewGuid();

        public int IdAntiguo { get; set; }
        public string TipoDocumento { get; set; }
        public string Abreviatura { get; set; }

        //Campos Foraneos
        public Guid PaisId { get; set; }

        [ForeignKey(nameof(PaisId))]
        public Paises Pais { get; set; }

        public Guid EstadoId { get; set; }

        [ForeignKey(nameof(EstadoId))]
        public Estados Estado { get; set; }

    }
}
