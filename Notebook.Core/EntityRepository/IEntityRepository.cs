using Notebook.Core.EntityRepository.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notebook.Core.EntityRepository
{
    public interface IEntityRepository<T> : IDisposable where T : class, IEntity
    {
        IQueryable<T> getAll();
        Task<IQueryable<T>> getAllAsync();
        T getOne(Expression<Func<T, bool>> exp);
        Task<T> getOneAsync(Expression<Func<T, bool>> exp);
        IQueryable<T> getMany(Expression<Func<T, bool>> exp = null);
        Task<IQueryable<T>> getManyAsync(Expression<Func<T, bool>> exp = null);
        void Add(T model);
        void Update(T model);
        void Delete(T model);
        void Save(bool Disposing = false);
    }
}
