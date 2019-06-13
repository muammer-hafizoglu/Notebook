using Microsoft.EntityFrameworkCore;
using Notebook.Core.EntityRepository.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notebook.Core.EntityRepository.EntityFramework
{
    public class EfEntityRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        private DbContext context;
        public EfEntityRepository(DbContext _context)
        {
            context = _context;
        }
        public void Add(T model)
        {
            context.Entry(model).State = EntityState.Added;
        }

        public void Delete(T model)
        {
            context.Entry(model).State = EntityState.Deleted;
        }

        public IQueryable<T> getAll()
        {
            return context.Set<T>();
        }

        public IQueryable<T> getMany(Expression<Func<T, bool>> exp = null)
        {
            return context.Set<T>().Where(exp);
        }

        public T getOne(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().FirstOrDefault(exp);
        }

        public void Update(T model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        public void Save(bool Disposing = false)
        {
            context.SaveChanges();

            if (Disposing)
            {
                context.Dispose();
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task<T> getOneAsync(Expression<Func<T, bool>> exp)
        {
            return context.Set<T>().FirstOrDefaultAsync(exp);
        }

        public Task<IQueryable<T>> getAllAsync()
        {
            return context.Set<T>() as Task<IQueryable<T>>;
        }

        public Task<IQueryable<T>> getManyAsync(Expression<Func<T, bool>> exp = null)
        {
            return context.Set<T>().Where(exp) as Task<IQueryable<T>>;
        }
    }
}
