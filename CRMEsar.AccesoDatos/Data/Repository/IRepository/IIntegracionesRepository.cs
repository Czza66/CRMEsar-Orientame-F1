using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface IIntegracionesRepository : IRepository<Integraciones>
    {
        void update(Integraciones integraciones);
    }
}
