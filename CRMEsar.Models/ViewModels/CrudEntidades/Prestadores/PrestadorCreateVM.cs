using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Prestadores
{
    public class PrestadorCreateVM
    {
        public string UserIdSeleccionado { get; set; }

        public List<UsuarioDisponibleVM>? UsuariosDisponibles { get; set; }

        // Campos Prestador
        [Display(Name = "Profesión")]
        public string Profesion { get; set; }

        [Display(Name = "Recomendada por")]
        public string? RecomendadoPor { get; set; }
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [StringLength(70)]
        public string Celular { get; set; }
        public bool Orientador { get; set; }
        public bool PermiteILVE { get; set; }
        [Display(Name = "Tipo de servicio")]
        public string TipoServicio { get; set; }
        public string TipoPrestador { get; set; }
        public IEnumerable<SelectListItem>? ListaTipoPrestador { get; set; }
    }
}
