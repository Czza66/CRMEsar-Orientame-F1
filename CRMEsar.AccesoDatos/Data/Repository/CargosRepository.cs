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
    public class CargosRepository : Repository<Cargos>, ICargosRepository
    {
        private readonly ApplicationDbContext _db;

        public CargosRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListaCargos()
        {
            return _db.Cargos
                 .Where(c => c.Estado.NormalizedName == "ACTIVO")
                 .Select(c => new SelectListItem
                 {
                     Text = c.Cargo,
                     Value = c.CargoId.ToString()
                 });
        }

        public void update(Cargos cargos)
        {
            var objdesdeDB = _db.Cargos.FirstOrDefault(c => c.CargoId == cargos.CargoId);
            if (objdesdeDB != null)
            {
                objdesdeDB.Cargo = cargos.Cargo;
                objdesdeDB.Abreviatura = cargos.Abreviatura;
                objdesdeDB.EstadoId = cargos.EstadoId;      
            }
        }
    }
}
