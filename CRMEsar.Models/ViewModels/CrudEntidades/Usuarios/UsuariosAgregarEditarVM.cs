using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Usuarios
{
    public class UsuariosAgregarEditarVM
    {
        public string? UsuarioId { get; set; }
        public string? Sexo { get; set; }
        public string? Ciudad { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Nombre1 { get; set; }
        public string? Nombre2 { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string NumeroDocumento { get; set; }
        public string correo { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public Guid? PaisId { get; set; }
        public IEnumerable<SelectListItem>? ListaPaises { get; set; }

        public Guid? TipoDocumentoID { get; set; }
        public IEnumerable<SelectListItem>? ListaTiposDocumentos { get; set; }

        public Guid? CargoID { get; set; }
        public IEnumerable<SelectListItem>? ListaCargos { get; set; }

        public Guid? EstadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEstados { get; set; }
    }
}
