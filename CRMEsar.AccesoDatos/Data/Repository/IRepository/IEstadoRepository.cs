using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IEstadoRepository :IRepository<Estados>
    {
        void Update(Estados estado);

        IEnumerable<SelectListItem> GetListaEstados(string EntidadNormalizada);
    }
}
