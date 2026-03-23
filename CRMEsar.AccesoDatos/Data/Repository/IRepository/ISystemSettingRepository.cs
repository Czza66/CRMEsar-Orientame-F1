using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.Models;

namespace CRMEsar.AccesoDatos.Data.Repository.IRepository
{
    public interface ISystemSettingRepository : IRepository<SystemSetting>
    {
        void update(SystemSetting systemSetting);
    }
}
