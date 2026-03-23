using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Estados
    {
        [Key]
        public Guid EstadoId { get; set; } = Guid.NewGuid();
        public int IdAntiguo { get; set; }

        [Required(ErrorMessage ="El Nombre del estado es requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="La Normalizacion del nombre es requerida")]
        [MaxLength(50)]
        public string NormalizedName { get; set; }

        [Required(ErrorMessage = "La Descripcion del estado es requerida")]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        //Campos Foraneos
        public Guid EntidadId { get; set; }
        public Entidades Entidad { get; set; }

        // Relación con AspNetUser (uno a muchos)
        public ICollection<ApplicationUser>? ApplicationUser { get; set; }

        public ICollection <Modulos>? Modulos { get; set; }
        public ICollection <ModulosSecciones>? ModulosSecciones { get; set; }
        public ICollection<Paises>? Paises { get; set; }

    }
}
