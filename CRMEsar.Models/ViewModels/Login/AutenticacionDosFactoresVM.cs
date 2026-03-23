using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.Models.ViewModels.Login
{
    public class AutenticacionDosFactoresVM
    {
        //Acceso (Login)
        [Required(ErrorMessage ="El código del autenticador es requerido")]
        [Display(Name = "Código del Autenticador")]
        public string code { get; set; }

        //Para el registro

        public string Token { get; set; }

        //Para codigoQR
        public string? urlCodigoQR { get; set; }

        public List<SliderPresentacionesVM>? Slider { get; set; }
    }
}
