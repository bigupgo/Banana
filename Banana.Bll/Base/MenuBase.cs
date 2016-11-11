using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Banana.Core.Base;
using Banana.Model.Base;

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
            Expression<Func<Ba_Model, bool>> expr = x => x.IsDel.Equals(BoolFlag.FALSE);
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
        public IQueryable<Ba_Model> allMenu = null;
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<MenuMV> GetList()
        {
            List<MenuMV> list = new List<MenuMV>();
            var data = base.GetList(x => x.IsDel.Equals(BoolFlag.FALSE));
            allMenu = data;
            var topMenu = data.Where(x => x.ModelLevel == 0).OrderBy(x => x.Sort);
            foreach (var item in topMenu)
            {
                MenuMV mu = new MenuMV();
                mu.ID = item.ID;
                mu.ModelName = item.ModelName;
                mu.ModelLevel = item.ModelLevel;
                mu.iconCls = item.IconCls;
                mu.Pid = item.Pid;
                mu.Type = item.Type;
                mu.Url = item.Url;
                mu.State = item.State;
                mu.Sort = item.Sort;
                mu.CreateTime = item.CreateTime;
                mu.children = GetChildMenu(item.ID);
                list.Add(mu);
            }

            return list;
        }


        private List<MenuMV> GetChildMenu(string pid)
        {
            var childData = allMenu.Where(x => x.Pid.Equals(pid));
            List<MenuMV> childMenu = new List<MenuMV>();
            foreach (var child in childData)
            {
                MenuMV cm = new MenuMV();
                cm.ID = child.ID;
                cm.ModelName = child.ModelName;
                cm.ModelLevel = child.ModelLevel;
                cm.iconCls = child.IconCls;
                cm.Pid = child.Pid;
                cm.Type = child.Type;
                cm.Url = child.Url;
                cm.State = child.State;
                cm.Sort = child.Sort;
                cm.CreateTime = child.CreateTime;
                if (child.Type == 0)
                {
                    cm.children = GetChildMenu(cm.ID);
                }
                else
                {
                    cm.children = null;
                }
                childMenu.Add(cm);
            }
           
            return childMenu;

        }

    }
}

