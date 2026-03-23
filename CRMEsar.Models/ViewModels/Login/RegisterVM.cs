using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.Login
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "El Correo del usuario es requerido")]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(50, ErrorMessage = "El {0} debe estar entre al menos {2} caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion de la contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseña y confirmacion de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ComfirmPassword { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio")]
        [Display(Name = "Primer Nombre")]
        public string Nombre1 { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string? Nombre2 { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [Display(Name = "Primer Apellido")]
        public string Apellido1 { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string? Apellido2 { get; set; }

        [Required(ErrorMessage = "El numero de documento es obligatorio")]
        [Display(Name = "Numero de Documento")]
        public string NumeroDocumento { get; set; }

    }
}
