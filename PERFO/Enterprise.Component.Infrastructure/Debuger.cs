using System;
using System.Data;
using System.Configuration;
using log4net;
using log4net.Appender;

namespace Enterprise.Component.Infrastructure
{
    /// <summary>
    /// 文件名:  Debuger.cs
    /// 功能描述: 日志记录器，以log4jnet为引擎
    /// 创建人：qw
    /// 创建日期: 2013.1.21
    /// </summary>
    public class Debuger
    {
        //Debuger
        private static Debuger _instance;
        //logger
        private ILog logger = LogManager.GetLogger(typeof(Debuger));
        //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType 
        //日志记录器，是利用反射获取当前类的type,记录在日志中，便于定位日志发生的所在
        //private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //同步
        private static object _synRoot = new Object();

        public Debuger()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 得到唯一实例
        /// </summary>
        /// <returns></returns>
        public static Debuger GetInstance()
        {
            if (_instance == null)
            {
                lock (_synRoot)
                {
                    _instance = new Debuger();
                }
            }
            return _instance;
        }

        /// <summary>
        /// 清除对象
        /// </summary>
        /// <param name="obj"></param>
        public static void Reset(object obj)
        {
            _instance = null;
        }

        ///日志记录器1
        ///日志等级：Warn
        public void log(String s)
        {
            lock (_synRoot)
            {
                logger.Warn("Message=<<" + s + ">>");
            }
        }

        ///日志记录器2
        ///日志等级：Warn
        public void log(Object obj, String s)
        {
            lock (_synRoot)
            {
                logger.Warn("Type Name=<<" + obj.GetType() + ">>  Message=<<" + s + ">>");
            }
        }

        ///日志记录器3
        ///日志等级：Error
        public void log(Object obj, String s, Exception ex)
        {
            lock (_synRoot)
            {
                logger.Error("Type Name=<<" + obj.GetType() + ">>  Message=<<" + s + ">>", ex);
            }
        }

    }

}
