using Banana.Bll.Base;
using Banana.Core.Base;
using Banana.DBModel;
using Banana.Model.Base.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll.Weixin
{
    public class FoodBll:BaseBLL<Ba_Food>
    {
        public override void SetEntityName()
        {
            this.EntityName = "饭菜";
        }

        public override void SetRepository()
        {
            Repository = RepositoryFactory.GetWxRepository<Ba_Food>();
        }

        /// <summary>
        /// 获取 食物列表
        /// </summary>
        /// <param name="pagetion"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Ba_Food> GetList(Pagetion pagetion, string search)
        {
            List<Ba_Food> list = new List<Ba_Food>();
            var totalData = base.GetList();
            pagetion.total = totalData.Count();
            list = totalData.Skip(pagetion.rows * (pagetion.page - 1)).Take(pagetion.rows).ToList();
            return list;
        }

        /// <summary>
        /// 添加食物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override AjaxReturn Add(Ba_Food entity)
        {
            AjaxReturn result = base.Add(entity);
            return result;
        }

        /// <summary>
        /// 修改食物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override AjaxReturn Edit(Ba_Food entity)
        {
            AjaxReturn result = new AjaxReturn();
            var oldEntity = this.GetByKey(entity.ID);
            oldEntity.FoodName = entity.FoodName;
            oldEntity.Pic = entity.Pic;
            result = base.Edit(entity);
            return result;
        }

        /// <summary>
        /// 随机选择10 个菜
        /// </summary>
        /// <returns></returns>
        public List<Ba_Food> GetRandFood()
        {
            List<Ba_Food> list = new List<Ba_Food>();
            var totalData = base.GetList().ToList();
            Random rd = new Random();
            for (int i = 0; i < 9; i++)
            {
                int num = rd.Next(totalData.Count());
                if (!list.Contains(totalData[num]))
                {
                    list.Add(totalData[num]);
                }
                else
                {
                    i--;
                }
               
            }
            return list;
        }

        public Tuple<int, string, int> GetRandWheel(string foodList, int nowRan, int preNum, bool first)
        {
            List<Ba_Food> list = new List<Ba_Food>();
            if (!string.IsNullOrEmpty(foodList))
            {
                list = JSON.Deserialize<List<Ba_Food>>(foodList);
            }
            int index = 0;
            int num = 0;
            int ran = 0;
            if (first)
            {
                Random ra = new Random();
                num = ra.Next(9);
                ran = num * 40;
                if (num != 0)
                {
                    num = 8 - num;
                }
                index = num;
                preNum = num;
            }
            else
            {
                Random ra = new Random();
              
                num = ra.Next(10);
                ran = num * 40;
                if (preNum > num)
                {
                    index = preNum - num;
                }
                else
                {
                    index = 9 - (num - preNum);
                }
                preNum = index;
            }

          

            string foodName = list[index].FoodName;
            string mess = "今天就去吃" + foodName;
            Tuple<int, string, int> tuple = new Tuple<int, string, int>(ran, mess, preNum);
            return tuple;
        }
    }
}
