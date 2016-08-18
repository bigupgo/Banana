using System.Runtime.Remoting.Messaging;

namespace Banana.Core.Base
{
    public class ContextStorage
    {
        public static void Add<T>(string key, T value)
        {
            CallContext.SetData(key, value);
        }

        public static T Get<T>(string key) where T : class
        {
            return (CallContext.GetData(key) as T);
        }

        public static void Remove(string key)
        {
            CallContext.FreeNamedDataSlot(key);
        }

        public static void Set<T>(string key, T value)
        {
            CallContext.SetData(key, value);
        }
    }
}
