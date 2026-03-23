using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.Models.ViewModels.Login
{
    public class AccesoVM
    {
        [Required(ErrorMessage = "El Correo del usuario es requerido")]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recuérdame")]
        public bool RememberMe { get; set; }

        public List<SliderPresentacionesVM>? Slider { get; set; }
    }
}