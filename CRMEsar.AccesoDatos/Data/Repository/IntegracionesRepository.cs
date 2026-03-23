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
    public class IntegracionesRepository : Repository<Integraciones>, IIntegracionesRepository
    {
        private readonly ApplicationDbContext _db;

        public IntegracionesRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void update(Integraciones integraciones) 
        {
            var objDesdeDB = _db.Integraciones.FirstOrDefault(e => e.IntegracionId == integraciones.IntegracionId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.TipoIntegracion = integraciones.TipoIntegracion;
                objDesdeDB.Nombre = integraciones.Nombre;
                objDesdeDB.EndpointURL = integraciones.EndpointURL;
                objDesdeDB.metodoHttp = integraciones.metodoHttp;
                objDesdeDB.JsonSchema = integraciones.JsonSchema;
                objDesdeDB.Activa = integraciones.Activa;
            }
        }
    }
}
