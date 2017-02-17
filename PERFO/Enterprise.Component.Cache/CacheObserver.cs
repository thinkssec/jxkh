using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 缓存观察者
    /// </summary>
    public abstract class CacheObserver
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        protected string observerName;
        /// <summary>
        /// 缓存项实体
        /// </summary>
        protected ICacheSubject subject;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="observerName">观察者名称</param>
        /// <param name="subject">关联对象</param>
        public CacheObserver(string observerName, ICacheSubject subject)
        {
            this.observerName = observerName;
            this.subject = subject;
        }

        /// <summary>
        /// 更新操作(目前用于清除缓存)
        /// </summary>
        public abstract void Update();
    }
}
