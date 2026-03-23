using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IModuloRepository : IRepository<Modulos>
    {
        void update(Modulos modulos);

        IEnumerable<SelectListItem> GetListaModulos();

        
    }
}
