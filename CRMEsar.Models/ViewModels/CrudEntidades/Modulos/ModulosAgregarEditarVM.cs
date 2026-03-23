using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Modulos
{
    public class ModulosAgregarEditarVM
    {
        public string? moduloId { get; set; }

        [Required(ErrorMessage = "El icono del modulo es requerido")]
        public string icono { get; set; }

        [Required(ErrorMessage = "El Nombre del modulo es requerida")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Descripcion del modulo es requerida")]
        public string DescripcionCorta { get; set; }

        [Required(ErrorMessage ="El orden es requerido")]
        public int orden { get; set; }

        public bool visible { get; set; }

        public Guid EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }
    }
}
