using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CRMEsar.Models
{
    public class PermisosModulosSecciones
    {
        [Key]
        public Guid PermisoId { get; set; } = Guid.NewGuid();
        public bool Temporal { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        //Campos foraneos
        public Guid RolId { get; set; }
        public ApplicationRole applicationRole { get; set; }

        public Guid ModuloId { get; set; }
        public Modulos Modulo { get; set; }

        public Guid SeccionId { get; set; }
        public ModulosSecciones Seccion { get; set; }

    }
}
