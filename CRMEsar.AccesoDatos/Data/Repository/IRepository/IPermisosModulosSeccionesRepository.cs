using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IPermisosModulosSeccionesRepository : IRepository<PermisosModulosSecciones>
    {
        void update(PermisosModulosSecciones permisosModulosSecciones);

        bool ExistePermiso(string roleId, Guid moduloId, Guid seccionId);

    }
}
