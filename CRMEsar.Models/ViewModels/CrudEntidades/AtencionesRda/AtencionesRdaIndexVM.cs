using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.AtencionesRda
{
    public class AtencionesRdaIndexVM
    {
        public AtencionesRdaFiltrosVM Filtros { get; set; } = new();
        public List<AteencionesRdaTablaVM> Items { get; set; } = new();
    }
}
