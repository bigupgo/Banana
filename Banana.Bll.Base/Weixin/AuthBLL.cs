using Banana.Core.Base;
using Banana.Model.Base.Weixin;
using System;
using System.Security.Cryptography;

namespace Banana.Bll.Base.Weixin
{
    public class AuthBLL
    {
        public AjaxReturn Auth(AuthModel entity)
        {
            try
            {
                AjaxReturn result = new AjaxReturn();
                string token = BaseConfig.GetValue("Token");
                string AppID = BaseConfig.GetValue("AppID");
                string AppSecret = BaseConfig.GetValue("AppSecret");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(AppID) || string.IsNullOrEmpty(AppSecret))
                {
                    LogHelper.LogError("配置项没有配置！");
                }

                entity.Weixintoken = token.ToString();
                entity.AppID = AppID;
                entity.AppSecret = AppSecret;

                string[] tmp = { entity.Weixintoken, entity.timestamp, entity.nonce };
                Array.Sort(tmp);
                string tmpStr = SHA1EnCode(string.Join("", tmp));
                if (tmpStr == entity.signature)
                {
                    result.success = true;
                    result.message = "验证成功！";
                    result.data = entity.echostr;
                }
                else
                {
                    LogHelper.LogError("Auth Fail!");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("验证失败,错误信息:" + ex.Message);
                return new AjaxReturn { success = false, message = "验证失败!" };
            }
        }


        /// <summary>
        /// 进行SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SHA1EnCode(string str)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] secArr = sha1.ComputeHash(System.Text.Encoding.Default.GetBytes(str));
            return BitConverter.ToString(secArr).Replace("-", "").ToLower();
        }
    }

}
