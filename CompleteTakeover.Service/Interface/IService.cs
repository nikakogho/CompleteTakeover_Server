using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    public interface IService<TEntity, TKeyType, TRepository> 
        where TEntity : class 
        where TRepository : IRepository<TEntity, TKeyType>
    {
        TRepository Repository { get; }
        TEntity Get(TKeyType id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void Remove(TKeyType id);
        void RemoveRange(IEnumerable<TEntity> entities);
        void RemoveRange(IEnumerable<TKeyType> ids);
    }
}
