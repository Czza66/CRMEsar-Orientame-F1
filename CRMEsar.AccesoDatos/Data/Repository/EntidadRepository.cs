using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class EntidadRepository : Repository<Entidades> , IEntidadRepository
    {
        private readonly ApplicationDbContext _db;
        public EntidadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaEntidades()
        {
            return _db.Entidad
                .OrderBy(e => e.Nombre)
                .Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.EntidadId.ToString()
                });
        }

        public void update(Entidades entidad)
        {
            var objDesdeBD = _db.Entidad.FirstOrDefault(e => e.EntidadId == entidad.EntidadId);
            if (objDesdeBD != null)
            {
                objDesdeBD.Nombre = entidad.Nombre;
                objDesdeBD.NormalizedName = entidad.NormalizedName;
            }
        }
    }
}
