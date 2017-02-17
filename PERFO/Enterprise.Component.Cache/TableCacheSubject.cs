using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 数据表缓存项实体
    /// </summary>
    public class TableCacheSubject : ICacheSubject
    {

        #region 观察者

        /// <summary>
        /// 观察者集合
        /// </summary>
        private List<CacheObserver> observers = new List<CacheObserver>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="observer"></param>
        public void Attach(CacheObserver observer)
        {
            observers.Add(observer);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="observer"></param>
        public void Detach(CacheObserver observer)
        {
            if (observers.Count > 0)
            {
                observers.Remove(observer);
            }
        }

        /// <summary>
        /// 触发通知
        /// </summary>
        public void Notify()
        {
            foreach (CacheObserver observer in observers)
            {
                observer.Update();
            }
        }

        #endregion


        #region 缓存项自身内容

        /// <summary>
        /// 构造方法
        /// </summary>
        public TableCacheSubject(TableChangeModel _Table)
        {
            this.Table = _Table;
        }

        /// <summary>
        /// 数据表
        /// </summary>
        public TableChangeModel Table { get; set; }

        /// <summary>
        /// 检测当前状态
        /// </summary>
        /// <param name="_Table"></param>
        /// <returns></returns>
        public bool CheckState(TableChangeModel _Table)
        {

            if (Table.CHANGEID != _Table.CHANGEID)
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
