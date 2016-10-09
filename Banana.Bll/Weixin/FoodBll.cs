using Banana.Bll.Base;
using Banana.Core.Base;
using Banana.Core.Common;
using Banana.DBModel;
using Banana.Model.Base.Weixin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            try
            {
                var totalData = base.GetList();
                if (!string.IsNullOrEmpty(search))
                {
                    totalData = totalData.Where(x => x.FoodName.Contains(search));
                }
                pagetion.total = totalData.Count();
                list = totalData.OrderByDescending(x => x.CreateDate).Skip(pagetion.rows * (pagetion.page - 1)).Take(pagetion.rows).ToList();
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }
           
          
            return list;
        }

        /// <summary>
        /// 添加食物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override AjaxReturn Add(Ba_Food entity)
        {
            AjaxReturn result = new AjaxReturn();
            if (IsAddHas(entity.FoodName))
            {
                result.success = false;
                result.message = "菜名已添加!";
            }
            else
            {
                entity.CreateDate = System.DateTime.Now;
                result = base.Add(entity);
            }
           
            return result;
        }

        /// <summary>
        /// 修改食物
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AjaxReturn Edit(Ba_Food entity)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                if (IsEditHas(entity.FoodName, entity.ID))
                {
                    result.success = false;
                    result.message = "菜名已添加!";
                }
                else
                {
                    var oldEntity = this.GetByKey(entity.ID);
                    oldEntity.FoodName = entity.FoodName;
                    oldEntity.Pic = entity.Pic;
                    result.success = Repository.Update(oldEntity, true);
                }
            }
            catch (Exception e)
            {
                LogHelper.LogError(e.Message);
            }

            return result;
        }

        public AjaxReturn DeleteByIDs(string ids)
        {
            AjaxReturn res = new AjaxReturn();
            string[] arrayId = ids.Split(',');
            foreach (string id in arrayId)
            {
                Repository.Delete(x => x.ID.Equals(id),false);
            }
            res.success = Repository.SaveChange()>0;
            res.SetMessage("操作成功", "操作失败");
            return res;
        }

        /// <summary>
        /// 保存饭菜图片
        /// </summary>
        /// <param name="foodId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string SavePic(string foodId, HttpPostedFileBase file)
        {
            HttpServerUtility Server = HttpContext.Current.Server;
            if (string.IsNullOrEmpty(foodId) || file == null)
            {
                throw new ArgumentNullException();
            }
            if (foodId == "0")
            {
                foodId = GetGUID();
            }
            string basePath = BaseConfig.GetValue("FoodPicUrl");
            string fileName = foodId + Path.GetExtension(file.FileName);
            string url = basePath + fileName;
            string filePath = Server.MapPath("~" + url);
            UploadHelper.UploadFile(filePath, file);
            return url + "," + fileName;
        }

        /// <summary>
        /// 随机选择菜
        /// </summary>
        /// <returns></returns>
        public List<Ba_Food> GetRandFood(int total)
        {
            List<Ba_Food> list = new List<Ba_Food>();
            var totalData = base.GetList().ToList();
            Random rd = new Random();
            for (int i = 0; i < total; i++)
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
            Random ra = new Random();
            num = ra.Next(9);
            if (first)
            {
              
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
            string mess = "今天就去吃【" + foodName+"】";
            Tuple<int, string, int> tuple = new Tuple<int, string, int>(ran, mess, preNum);
            return tuple;
        }


        private bool IsAddHas(string foodName)
        {
            bool flag = false;
            var list = this.GetList().Where(x => x.FoodName.Equals(foodName));
            if (list != null && list.Count() > 0)
            {
                flag = true;
            }

            return flag;
        }

        private bool IsEditHas(string foodName,string ID)
        {
            bool flag = false;
            var list = this.GetList().Where(x => x.FoodName.Equals(foodName) && !x.ID.Equals(ID));
            if (list != null && list.Count() > 0)
            {
                flag = true;
            }

            return flag;
        }
    }
}
