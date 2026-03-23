using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.ModulosSecciones
{
    public class ModulosSeccionesCrearEditarVM
    {
        public string? seccionId { get; set; }

        [Required(ErrorMessage ="El nombre de la seccion es requerida")]
        public string nombre { get; set; }

        [Required(ErrorMessage ="La descripcion de la seccion es requerida")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "El orden de la seccion es requerida")]
        public int orden { get; set; }

        public bool? visible { get; set; }

        [Required(ErrorMessage = "La area de la seccion es requerida")]
        public string area { get; set; }

        [Required(ErrorMessage = "El controlador de la seccion es requerida")]
        public string controller { get; set; }

        [Required(ErrorMessage = "La accion de la seccion es requerida")]
        public string action { get; set; }

        public Guid EstadoID { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }

        public Guid ModuloId { get; set; }
        public IEnumerable<SelectListItem>? ListaModulos { get; set; }

        public Guid? SeccionPadreId { get; set; }
        public IEnumerable<SelectListItem>? ListaSecciones { get; set; }
    }
}
