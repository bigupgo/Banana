﻿using Banana.Bll.Weixin;
using Banana.CheckInBll.com.longruan.android;
using Banana.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.CheckInBll
{
    public class CheckInHandlerBll
    {
        private MyService server = null;
        private User m_CurrentUser = null;
        private string loginName = "cuijiaqi";
        private string password = "112358cjq";
        private static Project[] Projects;
        private static DateTime LastGetProjectsTime;

        /// <summary>
        /// 龙软checkin系统的服务对象
        /// </summary>
        public CheckInHandlerBll()
        {
            server = new MyService();
        }

        /// <summary>
        /// 获取最新10条日志
        /// </summary>
        /// <returns></returns>
        public Blog[] GetBlogList()
        {
            if (this.CurrentUser == null)
            {
                CurrentUser = Login();
            }
            Blog[] projects = server.queryBlog(new BlogQuery() { employeeID = CurrentUser.employeeID, startNum = "0", endNum = "10" });
            return projects;
        }

        /// <summary>
        /// 获取最新日志
        /// </summary>
        /// <returns></returns>
        public Blog GetNewBlog()
        {
            if (this.CurrentUser == null)
            {
                CurrentUser = Login();
            }
            if (CurrentUser == null)
            {
                return null;
            }
            Blog[] projects = server.queryBlog(new BlogQuery() { employeeID = CurrentUser.employeeID, startNum = "0", endNum = "1" });
            return projects == null ? new Blog() : projects.FirstOrDefault();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public AjaxReturn AddBlog(NewBlog blog)
        {
            AjaxReturn ar = new AjaxReturn() { success = false };
            try
            {
                if (this.CurrentUser == null)
                {
                   CurrentUser = Login();
                }
                blog.employeeID = this.CurrentUser.employeeID;
                blog.blogType = "0";
                //blog.blogDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ar.success = server.insertBlog(blog);
                ar.SetMessage("添加日志成功！", "添加日志失败！");
                return ar;
            }
            catch
            {
                ar.success = true;
                ar.message = "服务不稳定，请查看是否添加到列表中。";
            }

            return ar;
        }

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public AjaxReturn UpdateBlog(NewBlog blog)
        {
            AjaxReturn ar = new AjaxReturn() { success = false };

            try
            {
                ar.success = server.updateBlog(blog);
                ar.SetMessage("更新日志成功！", "更新日志失败！");
                return ar;
            }
            catch
            {
                ar.success = true;
                ar.message = "服务不稳定，请查看列表中是否修改。";
            }

            return ar;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public AjaxReturn DeleteBlog(int blogID)
        {
            AjaxReturn ar = new AjaxReturn() { success = false };

            try
            {
                ar.success = server.deleteBlog(blogID);
                ar.SetMessage("删除日志成功！", "删除日志失败！");
                return ar;
            }
            catch
            {
                ar.success = true;
                ar.message = "服务不稳定，请查看是否从列表中删除。";
            }

            return ar;
        }


        void s_getAllProjectsCompleted(object sender, getAllProjectsCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Projects = e.Result;
                    LastGetProjectsTime = DateTime.Now;
                }
            }
            catch
            {
            }
            server.getAllProjectsCompleted -= s_getAllProjectsCompleted;
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public Project[] GetProjects()
        {
            int max = 3;
            while (max > 0)
            {
                try
                {
                    if (Projects != null)
                    {
                        if (LastGetProjectsTime.AddMinutes(30) < DateTime.Now)//30分钟内不重复读取
                        {
                            server.getAllProjectsCompleted += s_getAllProjectsCompleted;
                            server.getAllProjectsAsync(1);
                        }
                    }
                    else
                    {
                        Projects = server.getAllProjects();
                        LastGetProjectsTime = DateTime.Now;
                    }
                    break;
                }
                catch
                {
                    max--;
                }
            }

            return Projects;
        }

        public User Login()
        {
            loginName = HttpContext.Current.Session["BlogName"] + "";
            password = HttpContext.Current.Session["BlogPassword"] + "";
            if (string.IsNullOrEmpty(this.loginName) || string.IsNullOrEmpty(this.password))
            {
                return null;
            }

            User user = server.login(this.loginName, Banana.Core.Common.MD5Encoder.Compute(this.password));
            return user;
        }

        public User CurrentUser
        {
            get
            {
                if (this.m_CurrentUser == null)
                {
                     this.Login();
                }
                return this.m_CurrentUser;
            }
            private set
            {
                this.m_CurrentUser = value;
            }
        }

        public void AutoWriteBlog()
        {
            BlogBll blogBll = new BlogBll();
            var userList = blogBll.GetAutoBlogUser();
            foreach (var blogUser in userList)
            {
                try
                {
                    User user = server.login(blogUser.BlogName, Banana.Core.Common.MD5Encoder.Compute(blogUser.BlogPassword));
                    Blog[] projects = server.queryBlog(new BlogQuery() { employeeID = user.employeeID, startNum = "0", endNum = "1" });
                    var newBlog = projects.FirstOrDefault();
                   
                    if (newBlog != null)
                    {
                        int num = GetWeekDay();
                        if (num != 0)
                        {
                            if (Convert.ToDateTime(newBlog.blogDate).Day < (System.DateTime.Now.Day - num))
                            {
                                NewBlog blog = new NewBlog();
                                blog.employeeID = user.employeeID;
                                blog.beginTime = "08:30";
                                blog.endTime = "18:00";
                                blog.blogType = "0";
                                blog.blogDate = DateTime.Now.AddDays(-num).ToString("yyyy-MM-dd HH:mm:ss");
                                blog.projectID = newBlog.projectID;
                                blog.projectName = newBlog.projectName;
                                blog.blogContent = newBlog.blogContent;
                                server.insertBlog(blog);
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    LogHelper.LogError(e.Message);
                }
            }
        }

        public int GetWeekDay()
        {
            int num = 0;
            string weekstr = DateTime.Now.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": num = 3; break;
                case "Tuesday": num = 1; break;
                case "Wednesday": num = 1; break;
                case "Thursday": num = 1; break;
                case "Friday": num = 1; break;
                case "Saturday": num = 0; break;
                case "Sunday": num = 0; break;
            }

            return num;
        }
    }
}
