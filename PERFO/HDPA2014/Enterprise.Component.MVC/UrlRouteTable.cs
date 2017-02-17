using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Enterprise.Component.MVC
{
    /// <summary>
    /// URL路由表控制类
    /// </summary>
    public class UrlRouteTable
    {
        /// <summary>
        /// 路由配置集合
        /// </summary>
        public RouteCollection Routes { get; private set; }
        /// <summary>
        /// 路由配置信息集合
        /// </summary>
        private static Hashtable UrlRouteMap = Hashtable.Synchronized(new Hashtable());  

        public UrlRouteTable()
        {
            this.Routes = System.Web.Routing.RouteTable.Routes;
            Clear();
        }

        /// <summary>
        /// 添加路由集合 3参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRouteCollection(IList<UrlMapPageRoute> routes)
        {
            foreach (var r in routes)
            {
                AddRoute(r);
            }
        }

        /// <summary>
        /// 添加路由 3参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRoute(UrlMapPageRoute r)
        {
            if (!UrlRouteMap.ContainsKey(r.RouteName))
            {
                lock (UrlRouteMap.SyncRoot)
                {
                    UrlRouteMap.Add(r.RouteName, r);
                    this.Routes.MapPageRoute(r.RouteName, r.RouteUrl, r.PhysicalFile);
                }
            }
        }

        /// <summary>
        /// 添加路由 4参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRoute4(UrlMapPageRoute r)
        {
            if (!UrlRouteMap.ContainsKey(r.RouteName))
            {
                lock (UrlRouteMap.SyncRoot)
                {
                    UrlRouteMap.Add(r.RouteName, r);
                    this.Routes.MapPageRoute(r.RouteName, r.RouteUrl, r.PhysicalFile, r.CheckPhysicalUrlAccess);
                }
            }
        }

        /// <summary>
        /// 添加路由 5参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRoute5(UrlMapPageRoute r)
        {
            if (!UrlRouteMap.ContainsKey(r.RouteName))
            {
                lock (UrlRouteMap.SyncRoot)
                {
                    UrlRouteMap.Add(r.RouteName, r);
                    this.Routes.MapPageRoute(r.RouteName, r.RouteUrl, r.PhysicalFile, r.CheckPhysicalUrlAccess, r.Defaults);
                }
            }
            
        }

        /// <summary>
        /// 添加路由 6参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRoute6(UrlMapPageRoute r)
        {
            if (!UrlRouteMap.ContainsKey(r.RouteName))
            {
                lock (UrlRouteMap.SyncRoot)
                {
                    UrlRouteMap.Add(r.RouteName, r);
                    this.Routes.MapPageRoute(r.RouteName, r.RouteUrl, r.PhysicalFile, r.CheckPhysicalUrlAccess, r.Defaults, r.Constraints);
                }
            }
            
        }

        /// <summary>
        /// 添加路由 7参数
        /// </summary>
        /// <param name="r"></param>
        public void AddRoute7(UrlMapPageRoute r)
        {
            if (!UrlRouteMap.ContainsKey(r.RouteName))
            {
                lock (UrlRouteMap.SyncRoot)
                {
                    UrlRouteMap.Add(r.RouteName, r);
                    this.Routes.MapPageRoute(r.RouteName, r.RouteUrl, r.PhysicalFile, r.CheckPhysicalUrlAccess, r.Defaults, r.Constraints, r.DataTokens);
                }
            }
        }

        /// <summary>
        /// 清空所有路由
        /// </summary>
        public void Clear()
        {
            UrlRouteMap.Clear();
            this.Routes.Clear();
        }

    }

    /// <summary>
    /// URL路由映射类
    /// </summary>
    public class UrlMapPageRoute
    {

        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 路由URL
        /// </summary>
        public string RouteUrl { get; set; }
        /// <summary>
        /// 物理文件
        /// </summary>
        public string PhysicalFile { get; set; }
        /// <summary>
        /// 检测物理URL的可访问性
        /// </summary>
        public bool CheckPhysicalUrlAccess { get; set; }
        /// <summary>
        /// 缺省名称集合
        /// </summary>
        public RouteValueDictionary Defaults { get; set; }
        /// <summary>
        /// 约束条件集合
        /// </summary>
        public RouteValueDictionary Constraints { get; set; }
        /// <summary>
        /// 数据令牌集合
        /// </summary>
        public RouteValueDictionary DataTokens { get; set; }

        //
        // 摘要:
        //     提供用于定义 Web 窗体应用程序的路由的方法。
        //
        // 参数:
        //   routeName:
        //     路由的名称。
        //
        //   routeUrl:
        //     路由的 URL 模式。
        //
        //   physicalFile:
        //     路由的物理 URL。
        //
        // 返回结果:
        //     将添加到路由集合的路由。
        public UrlMapPageRoute(string routeName, string routeUrl, string physicalFile)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
        }
        //
        // 摘要:
        //     提供用于定义 Web 窗体应用程序的路由的方法。
        //
        // 参数:
        //   routeName:
        //     路由的名称。
        //
        //   routeUrl:
        //     路由的 URL 模式。
        //
        //   physicalFile:
        //     路由的物理 URL。
        //
        //   checkPhysicalUrlAccess:
        //     一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        //     属性。
        //
        // 返回结果:
        //     将添加到路由集合的路由。
        public UrlMapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
            this.CheckPhysicalUrlAccess = checkPhysicalUrlAccess;
        }
        //
        // 摘要:
        //     提供用于定义 Web 窗体应用程序的路由的方法。
        //
        // 参数:
        //   routeName:
        //     路由的名称。
        //
        //   routeUrl:
        //     路由的 URL 模式。
        //
        //   physicalFile:
        //     路由的物理 URL。
        //
        //   checkPhysicalUrlAccess:
        //     一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        //     属性。
        //
        //   defaults:
        //     路由参数的默认值。
        //
        // 返回结果:
        //     将添加到路由集合的路由。
        public UrlMapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
            this.CheckPhysicalUrlAccess = checkPhysicalUrlAccess;
            this.Defaults = defaults;
        }
        //
        // 摘要:
        //     提供用于定义 Web 窗体应用程序的路由的方法。
        //
        // 参数:
        //   routeName:
        //     路由的名称。
        //
        //   routeUrl:
        //     路由的 URL 模式。
        //
        //   physicalFile:
        //     路由的物理 URL。
        //
        //   checkPhysicalUrlAccess:
        //     一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        //     属性。
        //
        //   defaults:
        //     路由的默认值。
        //
        //   constraints:
        //     一些约束，URL 请求必须满足这些约束才能作为此路由处理。
        //
        // 返回结果:
        //     将添加到路由集合的路由。
        public UrlMapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
            this.CheckPhysicalUrlAccess = checkPhysicalUrlAccess;
            this.Defaults = defaults;
            this.Constraints = constraints;
        }
        //
        // 摘要:
        //     提供用于定义 Web 窗体应用程序的路由的方法。
        //
        // 参数:
        //   routeName:
        //     路由的名称。
        //
        //   routeUrl:
        //     路由的 URL 模式。
        //
        //   physicalFile:
        //     路由的物理 URL。
        //
        //   checkPhysicalUrlAccess:
        //     一个值，该值指示 ASP.NET 是否应验证用户是否有权访问物理 URL（始终会检查路由 URL）。此参数设置 System.Web.Routing.PageRouteHandler.CheckPhysicalUrlAccess
        //     属性。
        //
        //   defaults:
        //     路由参数的默认值。
        //
        //   constraints:
        //     一些约束，URL 请求必须满足这些约束才能作为此路由处理。
        //
        //   dataTokens:
        //     与路由关联的值，但这些值不用于确定路由是否匹配 URL 模式。
        //
        // 返回结果:
        //     将添加到路由集合的路由。
        //
        // 异常:
        //   System.ArgumentNullException:
        //     routeUrl 参数为 null。
        public UrlMapPageRoute(string routeName, string routeUrl, string physicalFile, bool checkPhysicalUrlAccess, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens)
        {
            this.RouteName = routeName;
            this.RouteUrl = routeUrl;
            this.PhysicalFile = physicalFile;
            this.CheckPhysicalUrlAccess = checkPhysicalUrlAccess;
            this.Defaults = defaults;
            this.Constraints = constraints;
            this.DataTokens = dataTokens;
        }


    }

}
