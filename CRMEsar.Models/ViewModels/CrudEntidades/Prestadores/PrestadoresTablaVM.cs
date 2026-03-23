using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.CrudEntidades.Prestadores
{
    public class PrestadoresTablaVM
    {
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public string Profesion {  get; set; }
        public string? TipoServicio { get; set; }
        public string TipoPrestador { get; set; }
        public string Estado { get; set; }


        public string PrestadorIdEncriptado { get; set; }

    }
}
