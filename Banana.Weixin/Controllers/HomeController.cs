using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.Weixin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            BaseConfig.SetValue("AccessToken", "123qw123");
            return View();
        }

    }
}
