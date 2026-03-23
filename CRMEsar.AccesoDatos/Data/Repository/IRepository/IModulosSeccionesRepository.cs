using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IModulosSeccionesRepository : IRepository<ModulosSecciones>
    {
        void update(ModulosSecciones modulosSecciones);

        IEnumerable<SelectListItem> GetListaSecciones();
        IEnumerable<ModulosSecciones> GetListaSeccionesPorModulo(Guid moduloId);
    }
}
