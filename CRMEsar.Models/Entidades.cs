using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Entidades
    {
        [Key]
        public Guid EntidadId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "El Nombre de la entidad es requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(50)]
        public string NormalizedName { get; set; }

        // Relación con Estados (uno a muchos)
        public ICollection<Estados>? Estados { get; set; }
    }
}
