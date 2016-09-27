using Banana.Bll.Weixin;
using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Areas.Weixin.Controllers
{
    public class SubscribeController : BaseController
    {
        //
        // GET: /Weixin/Subscribe/
        SubscribeBll server = new SubscribeBll();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(Pagetion pagetion, string search)
        {
            var data = server.GetList(pagetion, search);

            return GridResult(data, pagetion.total);
        }

    }
}
