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
    public class PrestadoresRepository : Repository<Prestadores>, IPrestadoresRepository
    {
        private readonly ApplicationDbContext _db;

        public PrestadoresRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Prestadores prestadores)
        {
            var objDesdeDB = _db.Prestadores.FirstOrDefault(p => p.PrestadorId == prestadores.PrestadorId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.Profesion = prestadores.Profesion;
                objDesdeDB.Codigo = prestadores.Codigo;
                objDesdeDB.RecomendadoPor = prestadores.RecomendadoPor;
                objDesdeDB.Telefono = prestadores.Telefono;
                objDesdeDB.Celular = prestadores.Celular;
                objDesdeDB.Orientador = prestadores.Orientador;
                objDesdeDB.PermiteILVE = prestadores.PermiteILVE;
                objDesdeDB.TipoServicio = prestadores.TipoServicio;
                objDesdeDB.TipoPrestador = prestadores.TipoPrestador;
                objDesdeDB.EstadoId = prestadores.EstadoId;
            }
        }
    }
}
