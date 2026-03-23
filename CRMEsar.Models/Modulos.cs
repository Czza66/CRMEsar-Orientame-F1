using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models
{
    public class Modulos
    {
        [Key]
        public Guid moduloId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "El icono del modulo es requerido")]
        public string icono { get; set; }

        [Required(ErrorMessage = "El nombre del modulo es requerido")]
        public string nombre { get; set; }

        public string? descripcionCorta { get; set; }

        [Required(ErrorMessage ="Es requerido darle un orden al modulo")]
        public int orden  { get; set; }

        public bool visible { get; set; } = true;

        //Campos Foraneos
        [ForeignKey("EstadoId")]
        public Guid EstadoId { get; set; }
        public Estados Estado { get; set; }
        //Campos Foraneos
        public ICollection<ModulosSecciones>? ModulosSecciones { get; set; }
        public ICollection<PermisosModulosSecciones>? permisosModulosSecciones { get; set; }
    }
}
