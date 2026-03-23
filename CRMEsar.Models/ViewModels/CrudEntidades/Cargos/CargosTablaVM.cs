using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Cargos
{
    public class CargosTablaVM
    {
        public string Cargo { get; set; }
        public string Abreviatura { get; set; }
        public string Estado { get; set; }

        public string CargoIdEncriptado { get; set; }
    }
}
