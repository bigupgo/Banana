using Banana.Bll.Base;
using Banana.Core.Base;
using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll.Weixin
{
    public class SubscribeBll : BaseBD<Ba_Subscribe>
    {
        public override void SetEntityName()
        {
            this.EntityName = "关注操作";
        }

        public override void SetRepository()
        {
            Repository = RepositoryFactory.GetWxRepository<Ba_Subscribe>();
        }

        /// <summary>
        /// 更新关注者状态
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateSub(Ba_Subscribe entity)
        {
            try
            {
                if (entity != null)
                {
                    var subEntity = this.GetList().Where(x => x.FromUserName.Equals(entity.FromUserName)).FirstOrDefault();
                    if (subEntity == null)
                    {
                        entity.ID = this.GetGUID();
                        Repository.Add(entity, true);
                    }
                    else
                    {
                        subEntity.Status = entity.Status;
                        Repository.Update(subEntity, true);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
        }

        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="pagetion"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Ba_Subscribe> GetList(Pagetion pagetion, string search)
        {
            List<Ba_Subscribe> list = new List<Ba_Subscribe>();
            try
            {
                var totalList = this.GetList();
                pagetion.total = totalList.Count();
                list = totalList.OrderByDescending(x => x.OptionDate).Skip((pagetion.page - 1) * pagetion.rows).Take(pagetion.rows).ToList();
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
          
            return list;
        }
    }
}
