using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class SystemSettingRepository : Repository<SystemSetting>, ISystemSettingRepository
    {
        private readonly ApplicationDbContext _db;

        public SystemSettingRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void update(SystemSetting systemSetting)
        {
            var objDesdeDB = _db.systemSettings.FirstOrDefault(s => s.SettingId == systemSetting.SettingId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.Key = systemSetting.Key;
                objDesdeDB.Group = systemSetting.Group;
                objDesdeDB.Description = systemSetting.Description;
                objDesdeDB.DataType = systemSetting.DataType;
                objDesdeDB.ValueString = systemSetting.ValueString;
                objDesdeDB.ValueInt = systemSetting.ValueInt;
                objDesdeDB.ValueDecimal = systemSetting.ValueDecimal;
                objDesdeDB.ValueBit = systemSetting.ValueBit;
                objDesdeDB.ValueDateTimeUtc = systemSetting.ValueDateTimeUtc;
                objDesdeDB.ValueJson = systemSetting.ValueJson;
                objDesdeDB.IsActive = systemSetting.IsActive;
                objDesdeDB.UpdatedAtUtc = systemSetting.UpdatedAtUtc;
                objDesdeDB.UpdatedBy = systemSetting.UpdatedBy;
            }
        }
    }
}
