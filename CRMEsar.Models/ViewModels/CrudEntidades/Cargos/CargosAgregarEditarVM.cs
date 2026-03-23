using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Cargos
{
    public class CargosAgregarEditarVM
    {
        public string? CargoId { get; set; }

        public int IdAntiguo { get; set; }
        public string Cargo { get; set; }
        public string Abreviatura { get; set; }

        public Guid EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }

    }
}
