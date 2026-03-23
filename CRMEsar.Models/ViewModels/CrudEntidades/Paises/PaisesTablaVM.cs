using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Paises
{
    public class PaisesTablaVM
    {
        public int IdAntiguo { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string NormalizedName { get; set; }
        public string codigoInternacional { get; set; }
        public bool PermiteILVE { get; set; }
        public string PaisIdEncriptado { get; set; }    
        public string Estado {  get; set; }
    }
}
