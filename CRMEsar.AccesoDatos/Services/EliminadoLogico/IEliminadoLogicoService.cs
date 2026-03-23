using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEsar.AccesoDatos.Services.EliminadoLogico
{
    public interface IEliminadoLogicoService
    {
        void EliminadoLogico<T>(Guid id, Guid estadoId) where T : class;
    }
}
