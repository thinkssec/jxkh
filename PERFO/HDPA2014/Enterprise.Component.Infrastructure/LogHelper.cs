using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 日志操作方法类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logger"></param>
        public static void WriteLog(string logger)
        {
            //调用Debuger日志记录器
            Debuger.GetInstance().log(logger);
            //Logger.Write(logger, "General");
        }
    }
}
