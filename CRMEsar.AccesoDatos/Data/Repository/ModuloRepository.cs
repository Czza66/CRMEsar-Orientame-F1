using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    internal class ModuloRepository : Repository<Modulos>, IModuloRepository
    {
        private readonly ApplicationDbContext _db;

        public ModuloRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListaModulos()
        {
            return _db.Modulos
                .Select(i => new SelectListItem
                {
                    Text = i.nombre,
                    Value = i.moduloId.ToString()
                });
        }


        public void update(Modulos modulos)
        {
            var objDesdeDB = _db.Modulos.FirstOrDefault(e => e.moduloId == modulos.moduloId);
            if (objDesdeDB != null)
            {
                objDesdeDB.nombre = modulos.nombre;
                objDesdeDB.descripcionCorta = modulos.descripcionCorta;
                objDesdeDB.orden = modulos.orden;
                objDesdeDB.visible = modulos.visible;
                objDesdeDB.EstadoId = modulos.EstadoId;
                objDesdeDB.icono = modulos.icono;
            }
        }
    }
}
