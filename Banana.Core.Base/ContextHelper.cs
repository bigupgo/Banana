using System;
using System.Security.Cryptography;
using System.Web;

namespace Banana.Core.Base
{
    public class ContextHelper
    {
        private const string default_skin = "bootstrap";
        private const string key_user = "CURRENT_USER";


        //带添加
        public static SessionUser GetCurrentUser()
        {
            return SessionManager.Get<SessionUser>(SessionManager.UserKey);
        }


        public static string GetLoginName()
        {
            SessionUser currentUser = GetCurrentUser();
            return ((currentUser == null) ? "" : currentUser.LoginName);
        }

        public static string GetSkin()
        {
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["skin"];
            if (cookie == null)
            {
                return "bootstrap";
            }
            string str = cookie.Value;
            if (string.IsNullOrEmpty(str))
            {
                return "bootstrap";
            }
            if (string.IsNullOrEmpty(str))
            {
                str = BaseConfig.GetValue("DefaultSkin");
                if (string.IsNullOrEmpty(str))
                {
                    str = "default";
                }
            }
            return str;
        }

        public static string GetUserID()
        {
            SessionUser currentUser = GetCurrentUser();
            return ((currentUser == null) ? "" : currentUser.Id);
        }

        public static string GetUserName()
        {
            SessionUser currentUser = GetCurrentUser();
            return ((currentUser == null) ? "" : currentUser.LoginName);
        }


        public static bool IsSuperAdmin()
        {
            SessionUser currentUser = GetCurrentUser();
            return ((currentUser != null) && currentUser.IsSuperAdmin);
        }


        public static void RemoveCurrentUser()
        {
            HttpContext.Current.Session.Remove("CURRENT_USER");
        }

        public static void SetSession(SessionUser user)
        {
            HttpContext.Current.Session["CURRENT_USER"] = user;
        }

        public static bool IsLogin()
        {
            return (GetCurrentUser() != null);
        }


        /// 生成随机字母字符串(数字字母混和)
        public static string GetNoncestr(int codeCount)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 时间戳转换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
