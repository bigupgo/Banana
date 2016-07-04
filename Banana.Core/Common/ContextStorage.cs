using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Common
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
