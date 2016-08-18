using Banana.Core;
using Banana.Core.Base;
using Banana.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Banana.WebUI
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public const string JsonContentType = "application/json";

        protected BaseController()
        {
        }

        protected override void EndExecuteCore(IAsyncResult asyncResult)
        {
            base.EndExecuteCore(asyncResult);
            ContextManager.Dispose();
        }

        public ActionResult ErrorResult(string errorMsg)
        {
            AjaxReturn return3 = new AjaxReturn();
            return3.success = false;
            return3.message =errorMsg;
            AjaxReturn return2 = return3;
            return base.Content(JSON.Serialize(return2), "application/json");
        }

        protected ActionResult GridResult(object list, int total)
        {
            return this.JSONResult(new { total = total, rows = list });
        }

        protected ActionResult JSONResult(object obj)
        {
            if (obj is string)
            {
                return base.Content(obj.ToString());
            }
            return base.Content(JSON.Serialize(obj), "application/json");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            if (!(exception is NullReferenceException) && !(exception is HttpException))
            {
                if (BaseConfig.GetValue("IsErrorLog") == "true")
                {
                    LogHelper.LogError((Environment.NewLine + "异常地址：" + filterContext.HttpContext.Request.Url.LocalPath) + Environment.NewLine + filterContext.Exception.Message, filterContext.Exception);
                }
                try
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
                catch
                {
                }
            }
            base.OnException(filterContext);
        }

        public ActionResult SuccessResult(string okMsg)
        {
            AjaxReturn return3 = new AjaxReturn();
            return3.success = true;
            return3.message = okMsg;
            AjaxReturn return2 = return3;
            return base.Content(JSON.Serialize(return2), "application/json");
        }

        public ActionResult SuccessResult(string okMsg, object data)
        {
            AjaxReturn return3 = new AjaxReturn();
            return3.success = true;
            return3.data = data;
            return3.message = okMsg;
            AjaxReturn return2 = return3;
            return base.Content(JSON.Serialize(return2), "application/json");
        }

        public SessionUser CurrentUser
        {
            get
            {
                return ContextHelper.GetCurrentUser();
            }
        }

        public AjaxReturn ValidFailResult
        {
            get
            {
                AjaxReturn return2 = new AjaxReturn();
                return2.success = false;
                return2.message = "数据有误，验证不通过";
                return return2;
            }
        }

    }
}
