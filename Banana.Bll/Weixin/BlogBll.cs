using Banana.Bll.Base;
using Banana.Core.Base;
using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Bll.Weixin
{
    public class BlogBll : BaseBD<Ba_BlogUser>
    {

        public override void SetEntityName()
        {
            EntityName = "日志用户管理";
        }

        public override void SetRepository()
        {
            Repository = RepositoryFactory.GetWxRepository<Ba_BlogUser>();
        }

        public Ba_BlogUser IsSaveUser(string openId)
        {
            Expression<Func<Ba_BlogUser, bool>> expr = x => x.IsDel.Equals(BoolFlag.FALSE);
            if (string.IsNullOrEmpty(openId))
            {
                return null;
            }
            else
            {
                expr = expr.And(x => x.OpenID.Equals(openId));
                var res = base.GetList().Where(expr);
                if (res.Count() > 0)
                {
                    return res.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public override AjaxReturn Add(Ba_BlogUser blogUser)
        {
            AjaxReturn res = new AjaxReturn();
            blogUser.OpenID = HttpContext.Current.Session["OpenId"] + "";
            blogUser.IsDel = BoolFlag.FALSE;
            blogUser.CreateTime = System.DateTime.Now;
            res = base.Add(blogUser);
            return res;
        }


        /// <summary>
        /// 获取 自动写日志人员
        /// </summary>
        /// <returns></returns>
        public IQueryable<dynamic> GetAutoBlogUser()
        {
           var list = base.GetList().Where(x => x.OpenID == "oh1bet0cEtQpQY684RFzBdCHCuPw" || x.OpenID == "oh1bet1Ze4j92ZIa-oOWXzoaoAhk");
           return list;
        }
    }
}
