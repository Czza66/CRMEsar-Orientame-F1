using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.CrudEntidades.SliderPresentaciones;

namespace CRMEsar.Models.ViewModels.Login
{
    public class ResultadoCuentaVM
    {
        [Required(ErrorMessage = "Ocurrio un error al reconocer el usuario, intentalo de nuevo")]
        public string IdUsuario { get; set; }  

        [Required(ErrorMessage ="El correo es requerido")]
        [EmailAddress(ErrorMessage ="Debes ingresar un correo valido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
            [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Debe contener mayúscula, minúscula, número y carácter especial")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        public List<SliderPresentacionesVM>? Slider { get; set; }

    }
}
