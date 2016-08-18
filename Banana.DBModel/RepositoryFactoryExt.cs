using Banana.Core.Base;
using System.Data.Objects.DataClasses;

namespace Banana.DBModel
{
    /// <summary>
    /// Repository 工厂
    /// </summary>
    public static class RepositoryFactoryExt
    {
        /// <summary>
        /// 获取 业务库（business） 仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repositoryFactory"></param>
        /// <returns></returns>
        public static IRepository<TEntity> GetBusinessRepository<TEntity>(this RepositoryFactory repositoryFactory)
            where TEntity : EntityObject
        {
            return repositoryFactory.GetRepository<BananaContext, TEntity>();
        }
    }
}
