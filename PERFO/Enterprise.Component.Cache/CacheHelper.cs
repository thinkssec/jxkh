using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Enterprise.Component.Infrastructure;

namespace Enterprise.Component.Cache
{
    
    /// <summary>
    /// 文件名:  CacheHelper.cs
    /// 功能描述: 缓存操作方法类
    /// 创建人：Caitou
    /// 创建日期: 2012.9
    /// 
    /// 修改人：qw
    /// 修改日期: 2013.1.24
    /// 修改内容：分离自定义刷新动作类，添加自动加载数据功能。
    /// </summary>
    public class CacheHelper
    {

        /* 应用原则：
         * 必须重复访问静态数据或很少改变的数据。
         * 数据访问在创建、访问或传输方法代价高昂。
         * 数据必须总是可以访问，即使当源服务器不能访问。
         */

        //2种建立CacheManager的方式
        //ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
        /// <summary>
        /// 缓存对象
        /// </summary>
        private static ICacheManager cache = CacheFactory.GetCacheManager();
        /// <summary>
        /// 缓存项名称集合
        /// </summary>
        private static IList<string> cacheKeys = new List<string>();

        /// <summary>
        /// 获取缓存项名称集合对象
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetCacheKeys()
        {
            return cacheKeys;
        }

        #region 简化操作区

        /// <summary>
        /// 添加缓存操作
        /// </summary>
        /// <param name="_T">类类型</param>
        /// <param name="_LoadMode">自动加载</param>
        /// <param name="_ConstuctParms">构造方法参数</param>
        /// <param name="_MethodName">调用方法名称</param>
        /// <param name="_MethodParameters">调用方法参数</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Add(Type _T, bool _LoadMode, object[] _ConstuctParms, string _MethodName,
            object[] _MethodParameters, string key, object value)
        {
            if (WebKeys.EnableCaching)
            {
                CacheItemRefreshAction refreshAction = new CacheItemRefreshAction(_T, _LoadMode);
                refreshAction.ConstuctParms = _ConstuctParms;
                refreshAction.MethodName = _MethodName;
                refreshAction.Parameters = _MethodParameters;
                CacheHelper.Add(key, value, refreshAction);
            }
        }

        #endregion

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Add(string key, object value)
        {
            if (WebKeys.EnableCaching)
            {
                cache.Add(key, value);
                if (!cacheKeys.Contains(key))
                    cacheKeys.Add(key);
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="myRefreshAction">自定义刷新动作</param>
        public static void Add(string key, object value, CacheItemRefreshAction myRefreshAction) 
        {
            if (WebKeys.EnableCaching)
            {
                //自定义刷新方式,如果过期将自动重新加载,过期时间为设定的时间（分钟）
                cache.Add(key, value,
                    CacheItemPriority.Normal, myRefreshAction,
                    new AbsoluteTime(TimeSpan.FromMinutes(WebKeys.CacheTimeout)));
                if (!cacheKeys.Contains(key))
                    cacheKeys.Add(key);
            }
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return (WebKeys.EnableCaching && Contains(key)) ? cache.GetData(key) : null;
        }

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key">键</param>
        public static void RemoveCache(string key)
        {
            cache.Remove(key);
            if (!cacheKeys.Contains(key))
                cacheKeys.Remove(key);
        }

        /// <summary>
        /// 移除与classKey相关的所有缓存对象
        /// </summary>
        /// <param name="cacheKey">键</param>
        public static void RemoveCacheForClassKey(string cacheKey)
        {
            List<string> keys = cacheKeys.Where(p => p.Contains(cacheKey)).ToList();
            foreach (string key in keys)
            {
                RemoveCache(key);
            }
        }

        /// <summary>
        /// 移除与cacheKeys数组相关的所有缓存对象
        /// </summary>
        /// <param name="cacheKeys">键数组</param>
        public static void RemoveCacheForClassKeys(string[] cacheKeys)
        {
            foreach (var ck in cacheKeys)
            {
                RemoveCacheForClassKey(ck);
            }
        }

        /// <summary>
        ///  判断是否存在缓存对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return cache.Contains(key);
        }

        /// <summary>
        /// 获取总的缓存数
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return cache.Count;
        }

        /// <summary>
        /// 清空缓存对象
        /// </summary>
        public static void Refresh()
        {
            cache.Flush();
            cacheKeys.Clear();
        }

    }
}
