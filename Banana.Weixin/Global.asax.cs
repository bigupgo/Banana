using Banana.CheckInBll;
using Banana.Core.Base;
using FluentScheduler;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Banana.Weixin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            #region 初始化日志
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            #endregion

            #region 初始化BaseConfig
            BaseConfig.Init();
            #endregion

            #region 每天 8:30 检测工作日志
            try
            {
                CheckInHandlerBll server = new CheckInHandlerBll();
                Registry task = new Registry();
                task.Schedule(() => server.AutoWriteBlog()).ToRunNow().AndEvery(1).Days().At(8, 30);//任务每天定时执行  
                JobManager.Initialize(task);  
            }
            catch
            {
            }
 

            #endregion

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}