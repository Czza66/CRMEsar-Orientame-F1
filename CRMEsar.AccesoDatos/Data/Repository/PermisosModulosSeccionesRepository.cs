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
    public class PermisosModulosSeccionesRepository : Repository<PermisosModulosSecciones>, IPermisosModulosSeccionesRepository
    {
        private readonly ApplicationDbContext _db;

        public PermisosModulosSeccionesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(PermisosModulosSecciones permisosModulosSecciones)
        {
            var objDesdeDB = _db.PermisosModulosSecciones.FirstOrDefault(e => e.PermisoId == permisosModulosSecciones.PermisoId);
            if (objDesdeDB != null)
            {
                objDesdeDB.Temporal = permisosModulosSecciones.Temporal;
                objDesdeDB.FechaInicio = permisosModulosSecciones.FechaInicio;
                objDesdeDB.FechaFin = permisosModulosSecciones.FechaFin;
                objDesdeDB.ModuloId = permisosModulosSecciones.ModuloId;
                objDesdeDB.RolId = permisosModulosSecciones.RolId;
                objDesdeDB.SeccionId = permisosModulosSecciones.SeccionId;
            }
        }


        public bool ExistePermiso(string roleId, Guid moduloId, Guid seccionId)
        {
            return _db.PermisosModulosSecciones.Any(p =>
                p.RolId.ToString() == roleId &&
                p.ModuloId == moduloId &&
                p.SeccionId == seccionId);
        }
    }
}
