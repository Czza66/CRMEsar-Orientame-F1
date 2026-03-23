using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CRMEsar.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CRMEsar.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbset;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbset = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T Get(Guid id)
        {
            return dbset.Find(id);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //Se crea una consulta IQueryable a paritr del DbSet del contexto
            IQueryable<T> query = dbset;

            // Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // Se incluyen propiedades de navegacion si se proporcionan
            if (includeProperties != null)
            {
                //se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query); 
            }

            return query; 
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
        {
            //Se crea una consulta IQueryable a paritr del DbSet del contexto
            IQueryable<T> query = dbset;

            // Se aplica el filtro si se proporciona
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Se incluyen propiedades de navegacion si se proporcionan
            if (includeproperties != null)
            {
                //se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeproperty in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeproperty);
                }
            }

            return query.FirstOrDefault();

        }

        public void Remove(Guid id)
        {
            T entityToRemove = dbset.Find(id);
            dbset.Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>>? filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    string? includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }


    }
}
