using Notebook.Business.Managers.Abstract;
using Notebook.Core.EntityRepository;
using Notebook.Core.EntityRepository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notebook.Business.Managers.Concrete
{
    public abstract class Manager<T> : IManager<T> where T : class, IEntity
    {
        private IEntityRepository<T> serviceDal;
        public Manager(IEntityRepository<T> _serviceDal)
        {
            serviceDal = _serviceDal;
        }
        public virtual void Add(T model)
        {
            model.ID = model.ID ?? CreateGuid();

            serviceDal.Add(model);
            Save();
        }

        public string CreateGuid(int length = 8)
        {
            string _guid = Guid.NewGuid().ToString().Substring(0, length);

            while (serviceDal.getAll().Any(a=>a.ID == _guid))
            {
                _guid = Guid.NewGuid().ToString().Substring(0, length);
            }

            return _guid.ToLower();
        }

        public virtual void Delete(T model)
        {
            serviceDal.Delete(model);
            Save();
        }

        public virtual IQueryable<T> getAll()
        {
            return serviceDal.getAll();
        }

        public virtual List<T> getAll(int Skip, int Take)
        {
            return serviceDal.getAll().Skip(Skip).Take(Take).ToList();
        }

        public Task<IQueryable<T>> getAllAsync()
        {
            return serviceDal.getAllAsync();
        }

        public IQueryable<T> getMany(Expression<Func<T, bool>> exp = null)
        {
            return serviceDal.getMany(exp);
        }

        public Task<IQueryable<T>> getManyAsync(Expression<Func<T, bool>> exp = null)
        {
            return serviceDal.getManyAsync(exp);
        }

        public T getOne(Expression<Func<T, bool>> exp)
        {
            return serviceDal.getOne(exp);
        }

        public Task<T> getOneAsync(Expression<Func<T, bool>> exp)
        {
            return serviceDal.getOneAsync(exp);
        }

        public IOrderedQueryable<T> getOneQuery(Expression<Func<T, bool>> exp)
        {
            return serviceDal.getOne(exp) as IOrderedQueryable<T>;
        }

        public void Save(bool Disposing = false)
        {
            serviceDal.Save(Disposing);
        }

        public IQueryable<T> Table()
        {
            return serviceDal.getAll();
        }

        public virtual void Update(T model)
        {
            serviceDal.Update(model);
            Save();
        }

        public virtual void Update(T databaseModel, T objectModel)
        {
            serviceDal.Update(databaseModel);
            Save();
        }
    }
}
