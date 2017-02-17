using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Hosting;
using NHibernate.Cfg;
using NHibernate;

namespace Enterprise.Component.ORM
{
    /// <summary>
    /// 文件名:  ORMApplication.cs
    /// 功能描述: ORM应用类，实现多种数据库的配置加载，可产生对应的数据连接对象。
    /// 创建人：qw
    /// 创建日期: 2013.1.21
    /// </summary>
    public class ORMApplication : HttpApplication
    {
        /// <summary>
        /// NHibernate配置信息集合
        /// </summary>
        private static IDictionary<string, Configuration> _ConfigurationMap;
        /// <summary>
        /// 数据库对象信息集合
        /// </summary>
        private static IDictionary<string, ISessionFactory> _SessionFactoryMap;

        /// <summary>
        /// 初始化
        /// </summary>
        static ORMApplication()
        {
            LoadConfiguration();
        }

        /// <summary>
        /// 获取与命名空间对应数据库连接对象ISession
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static ISession GetCurrentSession(System.Type theType)
        {
            //按分层数量最多最优先处理的规则---进行排序
            IList<string> keys = _SessionFactoryMap.Keys.OrderByDescending(p => p.Split('.').Length).ToList();
            for (int i = keys.Count - 1; i >= 0 ; i--)
            {
                if (theType.ToString().StartsWith(keys[i]))
                {
                    return _SessionFactoryMap[keys[i]].OpenSession();
                }
            }
            return null;
        }


        /// <summary>
        /// 重新加载NHibernate的数据库配置信息
        /// </summary>
        public static void LoadConfiguration()
        {
            _ConfigurationMap = new Dictionary<string, Configuration>();
            _SessionFactoryMap = new Dictionary<string, ISessionFactory>();
            string nhConfigPath = HostingEnvironment.MapPath("~/App_Data");//hibernate.cfg.xml
            try
            {
                //测试用 start
                if (string.IsNullOrEmpty(nhConfigPath))
                {
                    nhConfigPath = "E:\\workspace\\HJZX2013\\海检中心信息化\\_src\\Enterprise.UI.Web\\App_Data";
                }
                //end

                string[] files = Directory.GetFiles(nhConfigPath);//得到文件
                foreach (string file in files)//循环文件
                {
                    FileInfo fi = new FileInfo(file);//建立FileInfo对象
                    if (fi.Name.ToLower().EndsWith(".cfg.xml"))
                    {
                        Configuration cfg = new Configuration();
                        cfg.Configure(fi.FullName);
                        string sessionFactoryName = cfg.Properties["session_factory_name"];
                        _ConfigurationMap[sessionFactoryName] = cfg;
                        ISessionFactory sessionFactory = cfg.BuildSessionFactory();
                        _SessionFactoryMap[sessionFactoryName] = sessionFactory;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取对应的ISessionFactory
        /// </summary>
        /// <param name="mapKey">sessionFactoryName</param>
        /// <returns>ISessionFactory</returns>
        public static ISessionFactory BuildSessionFactory(string mapKey)
        {
            if (_SessionFactoryMap.ContainsKey(mapKey))
            {
                return _SessionFactoryMap[mapKey];
            }

            return null;
        }

    }
}
