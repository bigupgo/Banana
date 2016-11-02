using Banana.CheckInBll;
using Banana.CheckInBll.com.longruan.android;
using Banana.Core.Base;
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
        CheckInHandlerBll server = new CheckInHandlerBll();
        List<Project> prolist = null;
        public ActionResult Index()
        {
            return View();
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
