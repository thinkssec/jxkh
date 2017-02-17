using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Enterprise.Component.Cache
{

    /// <summary>
    /// 缓存清除-事件委托
    /// </summary>
    /// <param name="e"></param>
    public delegate void ClearEventHandler(CacheEventArgs e);

    /// <summary>
    /// 委托传递的事件参数
    /// </summary>
    public class CacheEventArgs : EventArgs
    {

        /// <summary>
        /// 实例参数
        /// </summary>
        private string _CacheClassKey;

        /// <summary>
        /// 构造方法
        /// </summary>
        public CacheEventArgs(string cacheClassKey)
        {
            this._CacheClassKey = cacheClassKey;
        }

    }
}
