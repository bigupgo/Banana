using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace Banana.Core.Base
{
    public class RepositoryFactory
    {
        public IRepository<TEntity> GetRepository<TContext, TEntity>()
            where TContext : ObjectContext, new()
            where TEntity : EntityObject
        {
            return new BaseRepository<TContext, TEntity>();
        }
    }
}
