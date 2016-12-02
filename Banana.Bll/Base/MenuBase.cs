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
                mu.Enable = item.Enable;
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
                cm.Enable = child.Enable;
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

        /// <summary>
        /// 获取模块树
        /// </summary>

        public List<Menutree> GetMenuTree()
        {
         

            List<Menutree> treeList = new List<Menutree>();

            if (allMenu == null)
            {
                allMenu = base.GetList(x => x.IsDel.Equals(BoolFlag.FALSE));
            }

            var topMenu = allMenu.Where(x => x.ModelLevel == 0);

            foreach (var item in topMenu)
            {
                Menutree menu = new Menutree();
                menu.text = item.ModelName;
                menu.id = item.ID;
                menu.state = "closed";

                TreeAttribute attr = new TreeAttribute();
                attr.Pid = item.Pid;
                attr.ModelLevel = item.ModelLevel;
                attr.Type = item.Type;
                menu.attributes = attr;
                menu.children = GetTreeChild(item.ID);
                treeList.Add(menu);
            }

            List<Menutree> relist = new List<Menutree>();

            Menutree tree = new Menutree();
            tree.text = "新建根目录";
            tree.id = "0";
            tree.state = "open";
        
            TreeAttribute attrt = new TreeAttribute();
            attrt.Pid = null;
            attrt.ModelLevel = 0;
            attrt.Type = 0;
            tree.attributes = attrt;
            tree.children = treeList;
            relist.Add(tree);
            return relist;
        }


        private List<Menutree> GetTreeChild(string pid)
        {
            var menuList = allMenu.Where(x => x.Pid.Equals(pid) && x.Type == 0);
            List<Menutree> childList = new List<Menutree>();
            if (menuList != null)
            {
                foreach (var item in menuList)
                {
                    Menutree menu = new Menutree();
                    menu.text = item.ModelName;
                    menu.id = item.ID;
                    menu.state = "closed";

                    TreeAttribute attr = new TreeAttribute();
                    attr.Pid = item.Pid;
                    attr.ModelLevel = item.ModelLevel;
                    attr.Type = item.Type;
                    menu.attributes = attr;

                    if (item.Type == 0)
                    {
                        menu.children = GetTreeChild(item.ID);
                    }
                    else
                    {
                        menu.children = null;
                    }
                    childList.Add(menu);
                }
            }

            return childList;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override AjaxReturn Add(Ba_Model model)
        {
            AjaxReturn result = new AjaxReturn();

            if (this.CheckUrl(model.Url))
            {
                result.success = false;
                result.message = "URL已存在!";
            }

            model.ID = GetGUID();
            model.CreateTime = System.DateTime.Now;
            result = base.Add(model);
            return result;
        }

       /// <summary>
       /// 修改菜单
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        public override AjaxReturn Edit(Ba_Model model)
        {
            AjaxReturn result = new AjaxReturn();
            var oldEtity = base.GetByKey(model.ID);

            oldEtity.ModelLevel = model.ModelLevel;
            oldEtity.ModelName = model.ModelName;
            oldEtity.Pid = model.Pid;
            oldEtity.Sort = model.Sort;
            oldEtity.Type = model.Type;
            oldEtity.Url = model.Url;
            oldEtity.IconCls = model.IconCls;
            result = base.Edit(oldEtity);
            return result;
        }

        /// <summary>
        /// 验证 URL是否重复
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool CheckUrl(string url, string Id = null)
        {
            List<Ba_Model> list = new List<Ba_Model>();
            if (!string.IsNullOrEmpty(Id))
            {
                list = base.GetList(x => x.IsDel.Equals(BoolFlag.FALSE) && x.Url.Equals(url) && !x.ID.Equals(Id)).ToList();
            }
            else
            {
                list = base.GetList(x => x.IsDel.Equals(BoolFlag.FALSE) && x.Url.Equals(url)).ToList();
            }
            return list.Count() > 0;
        }

        /// <summary>
        /// 验证组菜单是否有子菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool CheckChild(string Id)
        {
            List<Ba_Model> list = new List<Ba_Model>();
            list = base.GetList(x => x.IsDel.Equals(BoolFlag.FALSE) && x.Pid.Equals(Id)).ToList();
            return list.Count() > 0;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AjaxReturn Delete(string Id,string type)
        {
            AjaxReturn result = new AjaxReturn();
            if (type.Equals("0"))
            {
                if (this.CheckChild(Id))
                {
                    result.success = false;
                    result.message = "菜单下有子菜单无法删除";
                    return result;
                }
            }
           result = base.Delete(Id);
           return result;
        }
    }
}
