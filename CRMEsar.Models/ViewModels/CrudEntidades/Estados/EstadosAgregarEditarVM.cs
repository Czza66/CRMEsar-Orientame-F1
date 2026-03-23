using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Estados
{
    public class EstadosAgregarEditarVM
    {
        public string? EstadoId { get; set; }

        [Required(ErrorMessage = "El Nombre del estado es requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion del estado es requerida")]
        [MaxLength(150)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage ="Es necesario seleccionar una entidad")]

        public bool Activo { get; set; }

        public Guid EntidadId { get; set; }
        public IEnumerable<SelectListItem>? ListaEntidades { get; set; }
    }
}
