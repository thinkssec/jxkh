using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Enterprise.Component.Infrastructure;

namespace Enterprise.Component.Cache
{
    /// <summary>
    /// 文件名:  CacheItemRefreshAction.cs
    /// 功能描述: 自定义缓存刷新操作
    /// 创建人：qw
    /// 创建日期: 2013.1.24
    /// </summary>
    [Serializable]
    public class CacheItemRefreshAction : ICacheItemRefreshAction
    {

        #region 成员对象

        /// <summary>
        /// 对象类型 
        /// </summary>
        private Type attr_t;
        /// <summary>
        /// 自动加载标志
        /// </summary>
        private bool attr_isReload;

        private object[] attr_constuctParms;
        /// <summary>
        /// 构造方法参数集合
        /// </summary>
        public object[] ConstuctParms
        {
            set { this.attr_constuctParms = value; }
            get { return this.attr_constuctParms; }
        }
        private string attr_methodName;
        /// <summary>
        /// 执行的方法名称
        /// </summary>
        public string MethodName
        {
            set { this.attr_methodName = value; }
            get { return this.attr_methodName; }
        }
        private object[] attr_parameters;
        /// <summary>
        /// 方法参数集合
        /// </summary>
        public object[] Parameters
        {
            set { this.attr_parameters = value; }
            get { return this.attr_parameters; }
        }
        
        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法1
        /// </summary>
        /// <param name="attr_T">对象类型</param>
        /// <param name="attr_ConstuctParms">构造参数</param>
        /// <param name="attr_MethodName">方法名称</param>
        /// <param name="attr_Parameters">方法参数</param>
        public CacheItemRefreshAction(Type _T, object[] _ConstuctParms, string _MethodName, object[] _Parameters)
        {
            this.attr_t = _T;
            this.attr_constuctParms = _ConstuctParms;
            this.attr_methodName = _MethodName;
            this.attr_parameters = _Parameters;
            this.attr_isReload = true;
        }

        /// <summary>
        /// 构造方法2
        /// </summary>
        /// <param name="attr_T">对象类型</param>
        /// <param name="attr_IsReload">自动加载标志</param>
        public CacheItemRefreshAction(Type _T, bool _IsReload)
        {
            this.attr_t = _T;
            this.attr_isReload = _IsReload;
        }

        #endregion

        #region 自动加载数据

        /// <summary>
        /// 自动生成对象实例并执行方法
        /// </summary>
        /// <param name="key"></param>
        private void ReloadData(string key)
        {
            try
            {
                //根据类型创建对象
                object dObj = Activator.CreateInstance(attr_t, attr_constuctParms);
                //获取方法的信息
                MethodInfo method = attr_t.GetMethod(attr_methodName);
                //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
                BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
                //调用方法==原来的方法中已经包含了存入缓存的代码，所以直接再调用一下即可。
                if (attr_parameters != null && attr_parameters.Length > 0)
                {
                    method.Invoke(dObj, flag, Type.DefaultBinder, attr_parameters, null);
                }
                else
                {
                    method.Invoke(dObj, flag, Type.DefaultBinder, null, null);
                }
            }
            catch (Exception ex)
            {
                Debuger.GetInstance().log(this, 
                    string.Format("ReloadData出错!KEY={0},Type={1},MethodName={2},Parameters={3}",
                    key, attr_t, attr_methodName, attr_parameters), ex);
            }
        }

        #endregion

        #region ICacheItemRefreshAction 成员
        
        /// <summary>
        /// 自定义刷新操作
        /// </summary>
        /// <param name="removedKey">移除的键</param>
        /// <param name="expiredValue">过期的值</param>
        /// <param name="removalReason">移除理由 Expired：过期被移除 Removed：被手动移除 Scavenged：因为缓存数量已满，则根据缓存等级移除较低级的缓存 Unknown：未知移除，不建议使用</param>
        public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
        {
            //先移除缓存
            CacheHelper.RemoveCache(removedKey);

            //只当过期时才自动加载数据
            if (attr_isReload && removalReason == CacheItemRemovedReason.Expired)
            {
                ReloadData(removedKey);
            }
        }

        #endregion

    }
}
