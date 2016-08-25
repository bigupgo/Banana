using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Core.Base.Weixin
{
    public static class WeixinCommon
    {
        //获得虚拟路径
        public static string FormatPath(string path)
        {
            //获得主机+端口 
            string Authority = "http://" + HttpContext.Current.Request.Url.Authority;
            string ApplicationPath = HttpContext.Current.Request.ApplicationPath.Substring(HttpContext.Current.Request.ApplicationPath.Length - 1) == "/" ? HttpContext.Current.Request.ApplicationPath : HttpContext.Current.Request.ApplicationPath + "/";
            string result = Authority + ApplicationPath + path;
            return result;
        }

        /// <summary> 
        /// 微信的CreateTime是当前与1970-01-01 00:00:00之间的秒数
        /// </summary> 
        /// <param name=“dt”></param>
        /// <returns></returns> 
        public static long DateTimeToInt(this DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));

            //intResult = (time- startTime).TotalMilliseconds; 
            long t = (dt.Ticks - startTime.Ticks) / 10000000;
            //现在是10位，除10000调整为13位
            return t;
        }
    }
}
