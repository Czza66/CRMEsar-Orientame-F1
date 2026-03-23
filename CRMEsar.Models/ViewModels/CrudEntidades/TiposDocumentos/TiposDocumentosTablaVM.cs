using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.TiposDocumentos
{
    public class TiposDocumentosTablaVM
    {
        public string TipoDocumento { get; set; }
        public string Abreviatura { get; set; }

        //Campos Foraneos
        public string Pais { get; set; }
        public string Estado { get; set; }

        public string TipoDocumentoIdEncriptado { get; set; }
    }
}
