using System;
using System.Web;

namespace Banana.Core.Base
{
    public class SessionManager
    {
        public static string UserKey = "CURRENT_USER";

        public static void Add<T>(string key, T value)
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext should not be null,may it's not a http request!");
            }
            HttpContext.Current.Session.Add(key, value);
        }

        public static T Get<T>(string key) where T : class
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext should not be null,may it's not a http request!");
            }
            return (HttpContext.Current.Session[key]) as T;
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext should not be null,may it's not a http request!");
            }
            HttpContext.Current.Session.Remove(key);
        }
    }
}
