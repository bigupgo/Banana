using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Model.Base.Weixin.Enum
{
    public class WxUrl
    {
        //获取token GET 有效期7200秒
        public static string Token(string appId, string appsecret)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appsecret);
        }

        //获取微信服务器IP地址 GET
        public static string IP(string accessToken)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}", accessToken);
        }

        //获得jsapi_ticket（有效期7200秒) GET
        public static string JsapiTicket(string accessToken)
        {
            return string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken);
        }
    }
}
