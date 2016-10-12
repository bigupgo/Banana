using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Banana.Core.Base
{
    public interface IRepository<TEntity>
    {
        bool Add(TEntity entity, bool saveChange);

        bool Delete(IEnumerable<TEntity> items, bool saveChange);
        bool Delete(Expression<Func<TEntity, bool>> whereLamb, bool saveChange);
        bool Delete(TEntity entity, bool saveChange);
        TEntity GetByKey(params object[] id);
        TEntity GetByKeys(TEntity entity);
        TEntity GetEntity(Expression<Func<TEntity, bool>> whereLamb);
        string GetKeyProperty(Type entityType);
        List<string> GetKeysProperties(Type entityType);
        IQueryable<TEntity> GetList();
        IQueryable<TEntity> GetList<TO>(Expression<Func<TEntity, bool>> whereLamb, Expression<Func<TEntity, TO>> orderName, bool isASC, Pagetion pagetion);
        int GetTotal();
        int GetTotal(Expression<Func<TEntity, bool>> whereLamb);
        int SaveChange();
        bool Update(TEntity entity, bool saveChange);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> whereLamb);
    }
}
