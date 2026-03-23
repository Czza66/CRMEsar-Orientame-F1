using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.Models.ViewModels.Login
{
    public class ValidarResetPassword
    {
        [Required(ErrorMessage ="El correo es requerido para hacer la validacion")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="El numero de documento es requerido para hacer la validacion")]
        public string NumeroDocumento { get; set; }

        public List<SliderPresentacionesVM>? Slider { get; set; }
    }
}
