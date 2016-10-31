using Banana.CheckInBll;
using Banana.CheckInBll.com.longruan.android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI.Areas.Blog.Controllers
{
    public class BlogController : BaseController
    {
        //
        // GET: /Blog/Blog/
        CheckInHandlerBll server = new CheckInHandlerBll();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetList()
        { 
            var data = server.GetBlogList();
            return JSONResult(data);
        }
    
        public ActionResult Add(NewBlog blog)
        {
            return JSONResult(server.AddBlog(blog));
        }

        public ActionResult Edit(NewBlog blog)
        {
            return JSONResult(server.UpdateBlog(blog));
        }

        public ActionResult Delete(int blogID)
        {
            return JSONResult(server.DeleteBlog(blogID));
        }

        public ActionResult GetProjects()
        {
            return JSONResult(server.GetProjects());
        }
        
    }
}
