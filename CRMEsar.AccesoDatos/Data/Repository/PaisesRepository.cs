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
    public class PaisesRepository : Repository<Paises>, IPaisesRepository
    {
        private readonly ApplicationDbContext _db;

        public PaisesRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaPaises() 
        {
            return _db.Paises
                .Where(T => T.Estado.NormalizedName == "ACTIVO")
                .Select(p => new SelectListItem
                {
                    Text = p.Nombre,
                    Value = p.PaisId.ToString()
                });
        }

        public void update(Paises paises)
        {
            var objdesdeDB = _db.Paises.FirstOrDefault(p => p.PaisId == paises.PaisId);
            if (objdesdeDB != null) 
            { 
                objdesdeDB.Nombre = paises.Nombre;
                objdesdeDB.NormalizedName = paises.NormalizedName;
                objdesdeDB.PermiteILVE = paises.PermiteILVE;
                objdesdeDB.EstadoId = paises.EstadoId;
                objdesdeDB.Codigo = paises.Codigo;
                objdesdeDB.codigoInternacional = paises.codigoInternacional;
            }
        }
    }
}
