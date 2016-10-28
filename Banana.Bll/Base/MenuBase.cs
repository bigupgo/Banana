using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Banana.Core.Base;

namespace Banana.Bll.Base
{
    public class MenuBase : BaseBD<Ba_Model>
    {

        public override void SetEntityName()
        {
            EntityName = "菜单";
        }

        public override void SetRepository()
        {
            Repository = RepositoryFactory.GetBananaRepository<Ba_Model>();
        }

      
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public IQueryable<Ba_Model> GetMenu(int? level, string pid = null)
        {
            Expression<Func<Ba_Model, bool>> expr = x => x.IsDel.Equals("N");  
            if (level != null)
            {
                expr = expr.And(x => x.ModelLevel == level);
            }
            if (!string.IsNullOrEmpty(pid))
            {
                expr = expr.And(x => x.Pid.Equals(pid));
            }
            var data = base.GetList(expr);
            return data;
        }
    }
}
