using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades._2MFAConfig
{
    public class _2MFAVM
    {

        public string? Group { get; set; }
        public string? DescriptionValue { get; set; }
        public string? DescriptionString { get; set; }

        [Required(ErrorMessage ="Es requerido este dato")]
        public string ValueString { get; set; }

        [Required(ErrorMessage = "Es requerido este dato")]
        public int ValueInt { get; set; }

        public string IdEncriptadoValue { get; set; }
        public string IdEncriptadoString { get; set; }
    }
}
