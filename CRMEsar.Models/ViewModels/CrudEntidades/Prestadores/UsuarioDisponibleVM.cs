using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Prestadores
{
    public class UsuarioDisponibleVM
    {
        public string UserId { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Ciudad { get; set; }
        public string? Pais { get; set; }
    }
}
