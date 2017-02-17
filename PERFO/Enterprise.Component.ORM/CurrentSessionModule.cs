using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate.Context;
using NHibernate;

namespace Enterprise.Component.ORM
{

    /// <summary>
    /// 文件名:  CurrentSessionModule.cs
    /// 功能描述: 执行HttpApplication对象的BeginRequest和EndRequest方法。
    /// 创建人：qw
    /// 创建日期: 2013.1.21
    /// </summary>
    public class CurrentSessionModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
            context.EndRequest += new EventHandler(Application_EndRequest);
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// BeginRequest执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_BeginRequest(object sender, EventArgs e)
        {
            //foreach (string key in ORMApplication.SessionFactoryMap.Keys)
            //{
            //    ManagedWebSessionContext.Bind(HttpContext.Current,
            //        ORMApplication.SessionFactoryMap[key].OpenSession());
            //}
        }

        /// <summary>
        /// EndRequest执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_EndRequest(object sender, EventArgs e)
        {
            //foreach (string key in ORMApplication.SessionFactoryMap.Keys)
            //{
            //    ISession session = ManagedWebSessionContext.Unbind(HttpContext.Current,
            //        ORMApplication.SessionFactoryMap[key]);

            //    if (session.Transaction.IsActive)
            //    {
            //        session.Transaction.Rollback();
            //    }

            //    if (session != null)
            //    {
            //        session.Close();
            //    }
            //}
        }
    }
}
