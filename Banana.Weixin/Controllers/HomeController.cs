using Banana.Bll.Weixin;
using Banana.Core.Base;
using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.Weixin.Controllers
{
    public class HomeController : WxBaseController
    {
        //
        // GET: /Home/
        FoodBll server = new FoodBll();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Food()
        {
            return View();
        }

        public ActionResult GetFood(int total)
        {
            var list = server.GetRandFood(total);
            return JSONResult(list);
        }

        public ActionResult GetEat(string foodlist,int nowRan,int preNum,bool first)
        {
            var data = server.GetRandWheel(foodlist, nowRan, preNum, first);
            return JSONResult(data);
        }

        public ActionResult Grid()
        {
            return View();
        }

    }
}
