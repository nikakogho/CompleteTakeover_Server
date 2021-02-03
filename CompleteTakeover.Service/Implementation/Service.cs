using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CompleteTakeover.Service
{
    using Repository;

    internal abstract class Service<TEntity, TKeyType, TRepository> : IService<TEntity, TKeyType, TRepository> 
        where TEntity : class 
        where TRepository : IRepository<TEntity, TKeyType>
    {
        protected IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract TRepository Repository { get; }

        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Find(predicate);
        }

        public virtual TEntity Get(TKeyType id)
        {
            return Repository.Get(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
        }

        public virtual void Remove(TKeyType id)
        {
            Repository.Remove(Get(id));
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Repository.RemoveRange(entities);
        }

        public virtual void RemoveRange(IEnumerable<TKeyType> ids)
        {
            foreach (var id in ids) Remove(id);
        }
    }
}
