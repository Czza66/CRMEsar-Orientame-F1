using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.TiposDocumentos
{
    public class TipoDocumentCrearEditarVM
    {
        public string? TiposDocumentosId { get; set; } 

        public int IdAntiguo { get; set; }
        public string TipoDocumento { get; set; }
        public string Abreviatura { get; set; }

        //Campos Foraneos
        public Guid PaisId { get; set; }
        public IEnumerable<SelectListItem>? ListaPaises { get; set; }

        public Guid EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }

    }
}
