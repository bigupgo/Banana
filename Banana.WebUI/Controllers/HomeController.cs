using Banana.Bll;
using Banana.Bll.Base;
using Banana.Core.Base;
using System;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Controllers
{
    public class HomeController : BaseController
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

        /// <summary>
        /// 添加验证用户名是否重复
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsAddHave(string loginName)
        { 
            var service = new UserInfoBll();
            return service.IsAddHaveUser(loginName);
        }

        /// <summary>
        /// 修改验证用户名是否重复
        /// </summary> 
        /// <param name="loginName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public string IsEditHave(string loginName, string Id)
        {
            var service = new UserInfoBll();
            return service.IsEditHaveUser(loginName, Id);
        }

        /// <summary>
        /// 获取第一层菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFirstMenu()
        {
            var server = new MenuBase();
            var data = server.GetMenu(0);
            return JSONResult(data);
        }

        /// <summary>
        /// 获取其他菜单
        /// </summary>
        /// <param name="level"></param>
        /// <param name="pid"></param>
        /// <returns></returns>

        public ActionResult GetOtherMenu(string pid)
        {
            var server = new MenuBase();
            var data = server.GetMenu(null, pid);
            return JSONResult(data);
        }

        /// <summary>
        /// 菜单导航
        /// </summary>
        /// <returns></returns>
        public ActionResult SysMenu()
        {
            return View();
        }


        public ActionResult GetList()
        {
            var server = new MenuBase();
            var result= server.GetList();
            return JSONResult(result);
        }
    }
}
