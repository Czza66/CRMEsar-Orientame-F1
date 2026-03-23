using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Usuarios
{
    public class UsuariosTablaVM
    {
        public string? Sexo { get; set; }
        public string? Ciudad { get; set; }
        public string? TipoUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string NumeroDocumento { get; set; }
        public string correo { get; set; }
        public DateTime FechaModificacion { get; set; }


        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Cargo { get; set; }
        public string? TiposDocumento { get; set; }

        public string UsuarioIdEncriptado { get; set; } 
    }
}
