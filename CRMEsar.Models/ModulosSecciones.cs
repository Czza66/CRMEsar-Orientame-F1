using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class ModulosSecciones
    {
        [Key]
        public Guid seccionId { get; set; } = Guid.NewGuid();
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public bool visible { get; set; }
        public string area { get; set; }
        public string controller { get; set; }
        public string action { get; set; }

        //Relacion Autoreferenciada
        public Guid? seccionPadreId { get; set; }
        public ModulosSecciones? seccionPadre { get; set; }

        //Campos Foraneos
        public Guid EstadoId { get; set; }
        public Estados Estado { get; set; }

        //Campos Foraneos
        public Guid ModuloId { get; set; }
        public Modulos Modulo { get; set; }

        public ICollection<PermisosModulosSecciones>? permisosModulosSecciones { get; set; }
    }
}
