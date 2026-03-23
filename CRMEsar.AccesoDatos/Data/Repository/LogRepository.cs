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
    public class LogRepository : Repository<AspNetUserLogs>, ILogRepository
    {
        private readonly ApplicationDbContext _db;

        public LogRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
    }
}
