using Notebook.Core.EntityRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notebook.Business.Managers.Abstract
{
    public interface IManager<T> where T : class, IEntity
    {
        IQueryable<T> getAll();
        IQueryable<T> Table();
        List<T> getAll(int Skip, int Take);
        Task<IQueryable<T>> getAllAsync();
        IQueryable<T> getMany(Expression<Func<T, bool>> exp = null);
        Task<IQueryable<T>> getManyAsync(Expression<Func<T, bool>> exp = null);
        T getOne(Expression<Func<T, bool>> exp);
        Task<T> getOneAsync(Expression<Func<T, bool>> exp);
        IOrderedQueryable<T> getOneQuery(Expression<Func<T, bool>> exp);
        void Add(T model);
        void Update(T model);
        void Update(T databaseModel, T objectModel);
        void Delete(T model);
        void Save(bool Disposing = false);
        string CreateGuid(int length = 8);
    }
}
