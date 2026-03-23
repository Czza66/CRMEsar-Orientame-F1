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
    public class NotificacionesRepository : Repository<Notificaciones>, INotificacionesRepository
    {
        private readonly ApplicationDbContext _db;
        public NotificacionesRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void update(Notificaciones notificaciones)
        {
            var objDesdeBD = _db.Notificaciones.FirstOrDefault(n => n.NotificacionId == notificaciones.NotificacionId);
            if (objDesdeBD != null) 
            {
                objDesdeBD.NotificacionId = notificaciones.NotificacionId;
                objDesdeBD.EstaLeido = notificaciones.EstaLeido;
            }
        }
    }
}
