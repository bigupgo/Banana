using Banana.Bll.Weixin;
using Banana.Core.Base;
using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Areas.Weixin.Controllers
{
    public class FoodController : BaseController
    {
        //
        // GET: /Weixin/Food/
        FoodBll server = new FoodBll();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult GetList(Pagetion pagetion, string search)
        {
            var list = server.GetList(pagetion, search);
            return GridResult(list, pagetion.total);
        }

        [HttpPost]
        public ActionResult Add(Ba_Food food)
        {
            AjaxReturn result = server.Add(food);
            return JSONResult(result);
        }

        public ActionResult Edit(Ba_Food food)
        {
            AjaxReturn result = server.Edit(food);
            return JSONResult(result);
        }


        [HttpPost]
        public ActionResult Delete(string ids)
        {
            AjaxReturn result = server.DeleteByIDs(ids);

            return JSONResult(result);
        }

        /// <summary>
        /// 保存饭菜图片
        /// </summary>
        /// <param name="foodId"></param>
        /// <returns></returns>
        [HttpPost]
        public string SavePic(string foodId)
        {
            string url = "";
            HttpPostedFileBase file = base.Request.Files[0];
            url = server.SavePic(foodId, file);
            return url;
        }


    }
}
