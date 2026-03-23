using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using CRMEsar.Data;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.AccesoDatos.Services.EliminadoLogico
{
    public class EliminadoLogicoService : IEliminadoLogicoService
    {
        private readonly ApplicationDbContext _context;

        public EliminadoLogicoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EliminadoLogico<T>(Guid id, Guid estadoId) where T : class
        {
            var dbSet = _context.Set<T>();
            var entidad = dbSet.Find(id);

            if (entidad == null)
                throw new Exception("Registro no encontrado");

            var property = typeof(T).GetProperty("EstadoId");

            if (property == null)
                throw new Exception("La entidad no tiene la propiedad EstadoId");

            property.SetValue(entidad, estadoId);

            _context.SaveChanges();
        }

    }
}
