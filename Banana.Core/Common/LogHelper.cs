using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Common
{
    public class LogHelper
    {
        private static readonly ILog log_debug = LogManager.GetLogger("logdebug");
        private static readonly ILog log_err = LogManager.GetLogger("logerror");
        private static readonly ILog log_info = LogManager.GetLogger("loginfo");

        public static void LogDebug(object message)
        {
            if (log_debug.IsDebugEnabled)
            {
                log_debug.Debug(message);
            }
        }

        public static void LogDebug(object message, Exception ex)
        {
            if (log_debug.IsDebugEnabled)
            {
                log_debug.Debug(message, ex);
            }
        }

        public static void LogError(object message)
        {
            if (log_err.IsErrorEnabled)
            {
                log_err.Error(message);
            }
        }

        public static void LogError(string description, Exception ex)
        {
            if (log_err.IsErrorEnabled)
            {
                log_err.Error(description, ex);
            }
        }

        public static void LogInfo(object message)
        {
            if (log_info.IsInfoEnabled)
            {
                log_info.Info(message);
            }
        }

        public static void LogInfo(object message, Exception ex)
        {
            if (log_info.IsInfoEnabled)
            {
                log_info.Info(message, ex);
            }
        }
    }
}
