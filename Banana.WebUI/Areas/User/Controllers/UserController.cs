using Banana.Bll;
using Banana.Core.Base;
using Banana.Model;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Areas.User.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/User/
        UserInfoBll server = new UserInfoBll();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetList(Pagetion pagetion, string search)
        {
            TestBll bb = new TestBll();
            bb.GetlistTest();
            List<UserInfo> list = new List<UserInfo>();
            list = server.GetList(pagetion, search);
            return GridResult(list, pagetion.total);
        }

        [HttpPost]
        public ActionResult Add(UserInfo userInfo)
        {
            AjaxReturn result = server.Add(userInfo);
            return JSONResult(result);
        }

        public ActionResult Edit(UserInfo userInfo)
        {
            AjaxReturn result = server.Edit(userInfo);
            return JSONResult(result);
        }

        [HttpPost]
        public ActionResult Delete(string ids)
        {
            AjaxReturn result = server.Deletes(ids);
           
            return JSONResult(result);
        }

        [HttpPost]
        public string SaveUserImg(string userId)
        {
            string url = "";
            HttpPostedFileBase file = base.Request.Files[0];
            url = server.SaveUserIcon(userId, file);
            return url;
        }

        public void Export(string search)
        {
            server.ExportExcel(search);
        }

    }
}
