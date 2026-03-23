using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.PermisosModulosSecciones
{
    public class PermisosModulosSeccionesCrearEditarVM
    {
        public string? permisoId {  get; set; }
        public bool? Temporal { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin {  get; set; }

        public Guid ModuloId { get; set; }
        public IEnumerable<SelectListItem>? ListaModulos {  get; set; }

        public Guid SeccionId { get; set; }
        public IEnumerable<SelectListItem>? ListaSecciones { get; set; }

        public Guid RolId { get; set; }
        public IEnumerable<SelectListItem>? ListaRoles { get; set; }

    }
}
