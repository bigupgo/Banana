using System;

namespace Banana.Core.Base
{
    public class ContextManager
    {
        private const string contextKeys = "ContextKeys";
        private const string suffix = "_TContext2014";

        private static void addContextKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key 不能为 空");
            }
            string str = ContextStorage.Get<string>("ContextKeys");
            if (string.IsNullOrEmpty(str))
            {
                str = key;
            }
            else
            {
                str = str + "," + key;
            }
            ContextStorage.Set<string>("ContextKeys", str);
        }

        public static void Dispose()
        {
            string[] strArray = getContextKeys();
            if ((strArray != null) && (strArray.Length > 0))
            {
                string[] strArray2 = strArray;
                for (int i = 0; i < strArray2.Length; i = (int)(i + 1))
                {
                    string key = strArray2[i];
                    IDisposable disposable = ContextStorage.Get<IDisposable>(key);
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public static void Dispose(Type contextType)
        {
            if (contextType == null)
            {
                throw new ArgumentNullException("对象不能为空");
            }
            IDisposable disposable = ContextStorage.Get<IDisposable>(contextType.ToString() + "_TContext2014");
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        private static string[] getContextKeys()
        {
            string str = ContextStorage.Get<string>("ContextKeys");
            if (!string.IsNullOrEmpty(str))
            {
                return str.Split((char[])new char[] { ',' });
            }
            return null;
        }

        public static TContext Instance<TContext>() where TContext : class, IDisposable, new()
        {
            string key = typeof(TContext).ToString() + "_TContext2014";
            TContext local = ContextStorage.Get<TContext>(key);
            if (local == null)
            {
                TContext local2 = Activator.CreateInstance<TContext>();
                ContextStorage.Add<TContext>(key, local2);
                addContextKey(key);
                return local2;
            }
            return local;
        }
    }
}
