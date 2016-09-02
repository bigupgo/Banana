using Banana.Core.Base;
using Banana.Core.Base.Weixin;
using Banana.Model.Base.Weixin;
using Banana.Model.Base.Weixin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Bll.Base.Weixin
{
    public abstract class WxBaseBll
    {
         
        string appId = "";
        string appsecret = "";

        public WxBaseBll()
        {
          
            appId = BaseConfig.GetValue("AppID");
            appsecret = BaseConfig.GetValue("AppSecret");
        }

        /// <summary>
        /// 更新AccessToken
        /// </summary>
        public void UpdateToken()
        {
            string url = WxUrl.Token(appId, appsecret);
            var respose = HttpHelper.CreateGetHttpResponse(url, 8000);
            string resStr = HttpHelper.GetResponseString(respose);
            AccessToken accessToken =   JSON.Deserialize<AccessToken>(resStr);
            BaseConfig.SetWxValue("AccessToken", accessToken.access_token);
            BaseConfig.SetWxValue("TokenGetTime", System.DateTime.Now.ToString());
        }

        /// <summary>
        /// 验证AccessToken是否过期
        /// </summary>
        /// <returns></returns>
        public bool CheckToken()
        {
            bool result = true;
            string getTime = BaseConfig.GetWxValue("TokenGetTime");

            if (string.IsNullOrEmpty(getTime))
            {
                result = false;
            }
            else
            {
                DateTime span = DateTime.Now;
                TimeSpan spdate = span - Convert.ToDateTime(getTime);
                double diffSecond = spdate.TotalSeconds;
                if (diffSecond > 7200)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        public AjaxReturn GetAccessToken()
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                if (!CheckToken())
                {
                    this.UpdateToken();
                }
                result.data = BaseConfig.GetWxValue("AccessToken");

                result.success = true;
                result.message = "获取成功";
            }
            catch(Exception e)
            {
                LogHelper.LogError(e);
                result.success = false;
                result.message = "获取失败";
            }
            return result;
        }

        /// <summary>
        /// 更新JsapiTicket
        /// </summary>
        public void UpdateJsapiTicket()
        {
            string accessToken = this.GetAccessToken().data.ToString();
            string url = WxUrl.JsapiTicket(accessToken);

            var response = HttpHelper.CreateGetHttpResponse(url, 8000);
            var respStr = HttpHelper.GetResponseString(response);

            JsapiTicket jsapiTicket = JSON.Deserialize<JsapiTicket>(respStr);
            if (jsapiTicket != null && jsapiTicket.errcode == 0)
            {
                BaseConfig.SetWxValue("JsapiTicket", jsapiTicket.ticket);
                BaseConfig.SetWxValue("TicketCreateTime", System.DateTime.Now.ToString());
            }

        }

        /// <summary>
        /// 验证JsapiTiket是否过期
        /// </summary>
        /// <returns></returns>
        public bool CheckJsapiTicket()
        {
            bool result = true;
            string getTime = BaseConfig.GetWxValue("TicketCreateTime");

            if (string.IsNullOrEmpty(getTime))
            {
                result = false;
            }
            else
            {
                DateTime span = DateTime.Now;
                TimeSpan spdate = span - Convert.ToDateTime(getTime);
                double diffSecond = spdate.TotalSeconds;
                if (diffSecond > 7200)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取JsapiTicket
        /// </summary>
        /// <returns></returns>
        public AjaxReturn GetJsapiTicket()
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                if (!CheckJsapiTicket())
                {
                    this.UpdateJsapiTicket();
                }
                result.data = BaseConfig.GetWxValue("JsapiTicket");

                result.success = true;
                result.message = "获取成功";
            }
            catch (Exception e)
            {
                LogHelper.LogError(e);
                result.success = false;
                result.message = "获取失败";
            }
            return result;
        }

        /// <summary>
        /// 获取signature签名
        /// </summary>
        /// <param name="noncestr"></param>
        /// <param name="jsapiTicket"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public AjaxReturn GetSignature(string noncestr, string jsapiTicket, string timestamp, string url)
        {
            AjaxReturn result = new AjaxReturn();
            try
            {
                noncestr = "noncestr=" + noncestr;
                jsapiTicket = "jsapiTicket=" + jsapiTicket;
                timestamp = "timestamp=" + timestamp;
                url = "url=" + url;
                string[] tempArry = { noncestr, jsapiTicket, timestamp, url };
                Array.Sort(tempArry);
                string signature = SHA1EnCode(string.Join("&", tempArry));
                result.success = true;
                result.data = signature;
            }
            catch (Exception e)
            {
                result.success = false;
                LogHelper.LogError(e);
            }
            result.SetMessage("获取成功!", "获取失败");
            return result;
        }

        public string SHA1EnCode(string str)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] secArr = sha1.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            return BitConverter.ToString(secArr).Replace("-", "").ToLower();
        }
    }
}
