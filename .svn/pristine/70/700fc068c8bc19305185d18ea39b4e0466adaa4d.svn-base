using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobService.Common
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        /// <summary>
        /// 写入正确的log
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            loginfo.Info(log);
        }
        /// <summary>
        /// 报错时要写的log
        /// </summary>
        /// <param name="log"></param>
        public static void WriteError(string log)
        {
            logerror.Error(log);
        }
        /// <summary>
        /// 预警时写的log
        /// </summary>
        /// <param name="log"></param>
        public static void WriteWarn(string log)
        {
            logerror.Warn(log);
        }
        /// <summary>
        /// debug时写的log
        /// </summary>
        /// <param name="log"></param>
        public static void WriteDebug(string log)
        {
            logerror.Debug(log);
        }
    }
}
