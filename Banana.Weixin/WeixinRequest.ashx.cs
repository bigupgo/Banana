using Banana.Bll.Base.Weixin;
using Banana.Bll.Weixin;
using Banana.Core.Base;
using Banana.Model.Base.Weixin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Banana.Weixin
{
    /// <summary>
    /// WeixinRequest 的摘要说明
    /// </summary>
    public class WeixinRequest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string PostStr = string.Empty;
            //如果是POST请求则不是微信验证
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    PostStr = Encoding.UTF8.GetString(postBytes);
                }
                //new ErrorLog().WriteLog(PostStr);
                if (!string.IsNullOrEmpty(PostStr))
                {
                    Execute(PostStr);
                }
            }
            else
            {
                //进行认证
                auth();
            }
        }

        /// <summary>
        /// 应答
        /// </summary>
        /// <param name="PostStr"></param>
        private void Execute(string PostStr)
        {
            WeixinApiDispatch dispatch = new WeixinApiDispatch();
            //new ErrorLog().WriteLog(PostStr);//显示发来的xml
            string responseContent = dispatch.Execute(PostStr);
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            //new ErrorLog().WriteLog(responseContent);//显示发出去的xml
            HttpContext.Current.Response.Write(responseContent);
            HttpContext.Current.Response.End();
        }

        private void auth()
        {
            try
            {
                AuthBLL server = new AuthBLL();
                AuthModel entity = new AuthModel
                {
                    echostr = HttpContext.Current.Request.QueryString["echoStr"],
                    signature = HttpContext.Current.Request.QueryString["signature"],
                    timestamp = HttpContext.Current.Request.QueryString["timestamp"],
                    nonce = HttpContext.Current.Request.QueryString["nonce"]
                };

                AjaxReturn result = server.Auth(entity); //微信接入的测试
                if (result.success)
                {
                    HttpContext.Current.Response.Write(result.data.ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("auth方法出错！错误信息:" + ex.Message);
            }
            finally
            {
                HttpContext.Current.Response.End();
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
    }
}