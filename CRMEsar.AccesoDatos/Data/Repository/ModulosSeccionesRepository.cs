using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using CRMEsar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class ModulosSeccionesRepository : Repository<ModulosSecciones>,IModulosSeccionesRepository
    {
        private readonly ApplicationDbContext _db;

        public ModulosSeccionesRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListaSecciones()
        {
            return _db.ModulosSecciones
                .Select(i => new SelectListItem
                {
                    Text = i.nombre,
                    Value = i.seccionId.ToString()
                });
        }
        public IEnumerable<ModulosSecciones> GetListaSeccionesPorModulo(Guid moduloId)
        {
            return _db.ModulosSecciones
                .Include(ms => ms.Estado)  
                .Where(ms => ms.ModuloId == moduloId)
                .Where(ms => ms.Estado.Nombre != "Eliminado")
                .ToList();
        }

        public void update(ModulosSecciones modulosSecciones) 
        {
            var objDesdeDB = _db.ModulosSecciones.FirstOrDefault(e => e.seccionId == modulosSecciones.seccionId);
            if (objDesdeDB != null) 
            {
                objDesdeDB.nombre = modulosSecciones.nombre;
                objDesdeDB.descripcion = modulosSecciones.descripcion;
                objDesdeDB.orden = modulosSecciones.orden;
                objDesdeDB.visible = modulosSecciones.visible;
                objDesdeDB.area = modulosSecciones.area;
                objDesdeDB.controller = modulosSecciones.controller;
                objDesdeDB.action = modulosSecciones.action;
                objDesdeDB.EstadoId = modulosSecciones.EstadoId;
            }
        }

    }
}
