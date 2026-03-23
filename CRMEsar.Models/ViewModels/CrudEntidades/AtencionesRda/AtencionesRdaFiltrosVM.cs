using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.AtencionesRda
{
    public class AtencionesRdaFiltrosVM
    {
        public int? AnioAtencion { get; set; }
        public string? Donante { get; set; }
        public string? TipoPrestador { get; set; }
        public string? Pais { get; set; }
        public string? GrupoEtareo { get; set; }
        public string? TipoConsulta { get; set; }

        // Paginación (más adelante si la quieres por cursor)
        public string? Cursor { get; set; }

        // Combos
        public List<SelectListItem> ListaAnios { get; set; } = new();
        public List<SelectListItem> ListaDonantes { get; set; } = new();
        public List<SelectListItem> ListaTiposPrestador { get; set; } = new();
        public List<SelectListItem> ListaPaises { get; set; } = new();
        public List<SelectListItem> ListaGruposEtareos { get; set; } = new();
        public List<SelectListItem> ListaTiposConsulta { get; set; } = new();
    }
}
