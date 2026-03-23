using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models.ViewModels.MenuVM;

namespace CRMEsar.AccesoDatos.Services.Menu
{
    public interface IMenuService
    {
        Task<List<ModuloConSeccionesVM>> ObtenerMenuJerarquicoPorRolAsync(Guid guid);
    }
}
