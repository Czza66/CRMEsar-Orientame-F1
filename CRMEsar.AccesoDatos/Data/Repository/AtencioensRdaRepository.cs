using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class AtencioensRdaRepository : Repository<AtencionesRDA>, IAtencionesRdaRepository
    {
        private readonly ApplicationDbContext _db;

        public AtencioensRdaRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

    }
}
