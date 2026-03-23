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
    public class TipoNotificacionesRepository : Repository<TipoNotificaciones>, ITipoNotificacionesRepository
    {
        private readonly ApplicationDbContext _db;

        public TipoNotificacionesRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void update( TipoNotificaciones tipoNotificaciones) 
        {
            var objDesdeDB = _db.TipoNotificaciones.FirstOrDefault(t => t.tipoNotificacionId == tipoNotificaciones.tipoNotificacionId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.Nombre = tipoNotificaciones.Nombre;
                objDesdeDB.NormalizedName = tipoNotificaciones.NormalizedName;
                objDesdeDB.ColorHexadecimal = tipoNotificaciones.ColorHexadecimal;
                objDesdeDB.icono = tipoNotificaciones.icono;
            }
        }
    }
}
