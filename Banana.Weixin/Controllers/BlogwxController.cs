using Banana.Bll.Weixin;
using Banana.CheckInBll;
using Banana.CheckInBll.com.longruan.android;
using Banana.Core.Base;
using Banana.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.Weixin.Controllers
{
    public class BlogwxController : Controller
    {
        //
        // GET: /Blogwx/
        CheckInHandlerBll server = null;
        public BlogwxController()
        {

            server = new CheckInHandlerBll();
        }

        List<Project> prolist = null;
        public ActionResult Index()
        {
            string openId = Request.QueryString["openId"];

            if ((Session["BlogName"] + "") != "" && (Session["BlogPassword"] + "") != "")
            {
                ViewBag.Login = "display:none;";
            }
            else
            {
                BlogBll blogBll = new BlogBll();
                var blogUser = blogBll.IsSaveUser(openId);

                string blogName = "";
                string blogPassword = "";
                if (blogUser != null)
                {
                    blogName = blogUser.BlogName;
                    blogPassword = blogUser.BlogPassword;
                }
                Session["OpenId"] = openId;
                if (!string.IsNullOrEmpty(blogName) && !string.IsNullOrEmpty(blogPassword))
                {
                    Session["BlogName"] = blogName;
                    Session["BlogPassword"] = blogPassword;
                    ViewBag.Login = "display:none;";
                }
                else
                {
                    ViewBag.Login = "display:block;";
                }
            }
          
            return View();
        }

        public ActionResult BlogUserAdd(Ba_BlogUser blogUser)
        {
            BlogBll blogBll = new BlogBll();
            var res = blogBll.Add(blogUser);
            return Content(JSON.Serialize(res), "application/json");
        }

        public void GetProjects()
        {
            prolist = server.GetProjects().ToList();
        }

        public ActionResult SearchProject(string search)
        {
            if(prolist==null){
                 prolist = server.GetProjects().ToList();
            }
            if (string.IsNullOrEmpty(search))
            {
                return null;
            }
            var data = prolist.Where(x => x.projectID.Contains(search) || x.projectName.Contains(search));
            if (data.Count() > 10)
            {
                data = data.Take(10);
            }
            return Content(JSON.Serialize(data));
        }

        public ActionResult Add(NewBlog blog)
        {
            return Content(JSON.Serialize(server.AddBlog(blog)));
        }

        public ActionResult GetNewBlog()
        {
            return Content(JSON.Serialize(server.GetNewBlog()), "application/json");
        }
    }
}
