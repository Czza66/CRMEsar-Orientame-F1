using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.MenuVM;

namespace CRMEsar.AccesoDatos.Services.Menu
{
    public class ModuloConSeccionesVM
    {
        public string NombreModulo { get; set; }
        public string Icono { get; set; }
        public List<menuVM> Secciones { get; set; } = new();
    }
}
