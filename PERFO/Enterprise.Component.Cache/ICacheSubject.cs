using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 缓存对象接口定义
    /// </summary>
    public interface ICacheSubject
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="observer"></param>
        void Attach(CacheObserver observer);
        
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="observer"></param>
        void Detach(CacheObserver observer);

        /// <summary>
        /// 通知
        /// </summary>
        void Notify();
    }
}
