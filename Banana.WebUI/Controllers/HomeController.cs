using Banana.Bll;
using Banana.Core;
using Banana.Core.Common;
using Banana.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (!ContextHelper.IsLogin()) { return RedirectToAction("Login"); }
            return View();
        }

        /// <summary>
        /// Home页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 进入登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie userCookie = Request.Cookies["userCookie"];
            if (userCookie != null)
            {
                ViewBag.UserName = userCookie.Values["userName"];
                ViewBag.Password = userCookie.Values["password"];
            }
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPass"></param>
        /// <param name="remenberPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string userName, string userPass, bool remenberPwd)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                ContextHelper.RemoveCurrentUser();
              
                var service = new UserInfoBll();
                result = service.Login(userName, userPass);
                if (remenberPwd)
                {
                    HttpCookie cookie = new HttpCookie("userCookie");
                    cookie.Values.Add("userName", userName);
                    cookie.Values.Add("password", userPass);
                    cookie.Expires = System.DateTime.Now.AddDays(365);
                    Response.AppendCookie(cookie);
                }
                else
                {
                    HttpCookie userCookie = Request.Cookies["userCookie"];
                    if (userCookie != null)
                    {
                        userCookie.Expires = System.DateTime.Now.AddDays(-365);//清除客户端cookie
                        Response.AppendCookie(userCookie);
                    }
                }
                SessionUser user = result.data as SessionUser;
                if (result.success && user != null)
                {
                    ContextHelper.SetSession(user);
                    result.data = null;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message.ToString();
            }
            return Content(JSON.Serialize(result));
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            ContextHelper.RemoveCurrentUser();
            AjaxReturn result = new AjaxReturn() { success = true, message = "成功退出" };
            return Content(JSON.Serialize(result));
        }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDetail()
        {
            var service = new UserInfoBll();
            var data = service.GetDetail();
            string fileName = data.UserIcon;
            if (!string.IsNullOrEmpty(fileName) && !fileName.Contains("/"))
            {
                data.UserIcon = BaseConfig.GetValue("UserImgUrl") + fileName;
            }
            return Content(JSON.Serialize(data));
        }

        public string IsAddHave(string loginName)
        {
            var service = new UserInfoBll();
            return service.IsAddHaveUser(loginName);
        }

        public string IsEditHave(string loginName, string Id)
        {
            var service = new UserInfoBll();
            return service.IsEditHaveUser(loginName, Id);
        }
    }
}
