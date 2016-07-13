using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Banana.Core.Common
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
    }
}
