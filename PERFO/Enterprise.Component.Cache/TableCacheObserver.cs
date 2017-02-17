using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 数据表缓存观察者
    /// </summary>
    public class TableCacheObserver : CacheObserver
    {

        /// <summary>
        /// 数据表缓存观察者-构造方法
        /// </summary>
        /// <param name="observerName"></param>
        /// <param name="subject"></param>
        public TableCacheObserver(string observerName, ICacheSubject subject)
            : base(observerName, subject)
        {
        }

        /// <summary>
        /// 更新操作(目前用于清除缓存)
        /// </summary>
        public override void Update()
        {
            //删除指定的缓存项
            //HttpRuntime.Cache.Remove(observerName);
            //删除observerName相同的所有缓存对象
            CacheHelper.RemoveCacheForClassKey(observerName);
        }
    }
}
