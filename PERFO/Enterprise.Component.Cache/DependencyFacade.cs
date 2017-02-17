using System.Configuration;
using System.Web.Caching;
using System.Collections.Generic;


namespace Enterprise.Component.Cache
{

    /// <summary>
    /// 获取数据表的缓存依赖关系(外观模式)
    /// </summary>
    public static class DependencyFacade
    {

        #region 系统表相关

        ///// <summary>
        ///// 单位表缓存依赖
        ///// </summary>
        ///// <returns></returns>
        //public static AggregateCacheDependency GetSYS_DEPT_TableDependency()
        //{
        //    return (new SYS_DEPT_Table()).GetDependency();
        //}


        ///// <summary>
        ///// 用户表缓存依赖
        ///// </summary>
        ///// <returns></returns>
        //public static AggregateCacheDependency GetSYS_USER_TableDependency()
        //{
        //    return (new SYS_USER_Table()).GetDependency();
        //}


        ///// <summary>
        ///// IP表缓存依赖
        ///// </summary>
        ///// <returns></returns>
        //public static AggregateCacheDependency GetSYS_IP_TableDependency()
        //{
        //    return (new SYS_IP_Table()).GetDependency();
        //}

        #endregion

    }
}
