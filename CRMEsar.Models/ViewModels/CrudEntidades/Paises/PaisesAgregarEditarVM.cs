using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Paises
{
    public class PaisesAgregarEditarVM
    {

        public string? paisId { get; set; }
        public int IdAntiguo { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string codigoInternacional { get; set; }
        public bool PermiteILVE { get; set; }

        public Guid EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados {  get; set; } 

    }
}
