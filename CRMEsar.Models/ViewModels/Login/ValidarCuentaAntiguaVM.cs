using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.Models.ViewModels.Login
{
    public class ValidarCuentaAntiguaVM
    {

        public string? NumeroDocumento { get; set; }

        public int? CodPrestador { get; set; }
        public List<SliderPresentacionesVM>? Slider { get; set; }
    }
}
