using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Timers;

using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;
using Enterprise.Component.MVC;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.UI.Web
{

    public class Global : System.Web.HttpApplication
    {

        #region 变量定义

        /// <summary>
        /// 缓存监控定时器
        /// </summary>
        private static Timer cacheTimer = null;
        /// <summary>
        /// 用户登录失败信息集合
        /// </summary>
        public static List<UserLoginStatistics> UserLoginFailLst;
        /// <summary>
        /// 路由表
        /// </summary>
        private static UrlRouteTable routeTable = new UrlRouteTable();

        #endregion


        void Application_Start(object sender, EventArgs e)
        {

            //加载log4net配置   
            log4net.Config.XmlConfigurator.Configure();

            //加载路由表
            LoadUrlRoute();

            //注册定时执行任务到缓存
            RegisterCacheEntry();

            //初始化
            UserLoginFailLst = new List<UserLoginStatistics>();
        }

        void Application_End(object sender, EventArgs e)
        {

            //关闭路由表
            routeTable.Clear();

            //在应用程序关闭时运行的代码
            if (cacheTimer != null)
            {
                cacheTimer.Enabled = false;
                cacheTimer = null;
            }

            //关闭日志记录器
            log4net.LogManager.Shutdown();
        }

        void Application_Error(object sender, EventArgs e)
        {
            //在出现未处理的错误时运行的代码
            Exception objErr = Server.GetLastError().GetBaseException();
            //写入日志文件
            Debuger.GetInstance().log(this, "程序运行出现错误", objErr);
        }

        void Session_Start(object sender, EventArgs e)
        {
            //加载当前的缓存配置
            MyCacheManager.ReloadCache();
        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

            //移除SESSIONID的值
            if (Utility.SessionIDHash.ContainsKey(Session.SessionID))
            {
                Utility.SessionIDHash.Remove(Session.SessionID);
            }
        }

        #region 路由相关

        /// <summary>
        /// 加载路由表
        /// </summary>
        public static void LoadUrlRoute()
        {
            routeTable.Clear();//先清空一次
            UrlMapPageRoute r = new UrlMapPageRoute("1", "Main", "~/Default.aspx");
            routeTable.AddRoute(r);
            r = new UrlMapPageRoute("2", "C/F/PdfView", "~/Component/FlexPaper/PdfViewer.aspx");
            routeTable.AddRoute(r);
            routeTable.AddRouteCollection(SysModuleService.LoadUrlRoute());
        }

        #endregion

        #region 定时执行相关

        /// <summary>
        /// 利用定时器实现定时执行
        /// </summary>
        private void RegisterCacheEntry()
        {
            if (cacheTimer == null)
            {
                //启动定时器
                cacheTimer = new Timer();
                cacheTimer.Elapsed += new ElapsedEventHandler(cacheTimer_Elapsed);
                int interval = WebKeys.MonitorInterval * 1000;
                if (interval < 10000) interval = 10000;
                cacheTimer.Interval = interval;
                cacheTimer.Enabled = true;
                cacheTimer.AutoReset = true;
            }
        }

        /// <summary>
        /// 定时执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cacheTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                cacheTimer.Enabled = false;
                //加载数据变化表
                IList<SysTablechangeModel> tabchangeList = SysTablechangeService.GetAllTableChangeList();
                //进行有效性检测
                MyCacheManager.CheckAvailability(tabchangeList);

                //2==复位检测
                if (UserLoginFailLst != null && UserLoginFailLst.Count > 0)
                {
                    //只检测自锁定时间开始后5分钟的数据
                    if (UserLoginFailLst.Count(p => p.RecordDate.AddMinutes(5) <= DateTime.Now) > 0)
                    {
                        List<UserLoginStatistics> resetLst = UserLoginFailLst.Where(p => p.RecordDate.AddMinutes(5) <= DateTime.Now).ToList();
                        foreach (UserLoginStatistics m in resetLst)
                        {
                            UserLoginFailLst.Remove(m);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, "执行cacheTimer_Elapsed出错!", ex);
            }
            finally
            {
                cacheTimer.Enabled = true;
            }
        }

        #endregion
    }


    #region 内部类定义

    /// <summary>
    /// 用户登录次数统计类
    /// </summary>
    public class UserLoginStatistics
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int FailCount { get; set; }
        /// <summary>
        /// 记录日期
        /// </summary>
        public DateTime RecordDate { get; set; }
    }

    #endregion

}