using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.Models.ViewModels.Panel
{
    public class AdminPanelVM
    {
        public int UsuariosActivos { get; set; }
        public int IniciosSesionHoy { get; set; }
        public int UsuariosCon2FA { get; set; }
        public int PrestadoresActivos { get; set; }
    }
}
