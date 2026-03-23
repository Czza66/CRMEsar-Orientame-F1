using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IEntidadRepository : IRepository<Entidades>
    {
        void update(Entidades entidad);

        IEnumerable<SelectListItem> GetListaEntidades();

    }
}
