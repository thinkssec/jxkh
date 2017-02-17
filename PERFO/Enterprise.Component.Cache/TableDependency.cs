using System.Web.Caching;
using System.Configuration;

namespace Enterprise.Component.Cache
{

    /// <summary>
    /// 该类是具体要求产品类 Category,Item,Product 的共同父类，其中实现了类构造函数和GetDependency方法
    /// </summary>
    public abstract class TableDependency : ICacheDependency {

        // 为分析web.config文件中有关数据表的配置，建立分隔区
        protected char[] configurationSeparator = new char[] { ',' };

        protected AggregateCacheDependency dependency = new AggregateCacheDependency();

        //缓存数据表访问类实例
       // private static readonly SYS_CacheDependency_Data cacheDAL = new SYS_CacheDependency_Data();

        /// <summary>
        /// 实现类构造函数，设置聚合缓存依赖对象
        /// </summary>
        /// <param name="configKey">配置信息</param>
        protected TableDependency(string configKey) {

            //1=从web.config中读取
            //string dbName = ConfigurationManager.AppSettings["CacheDatabaseName"];
            //string tableConfig = ConfigurationManager.AppSettings[configKey];
            //string[] tables = tableConfig.Split(configurationSeparator);

            //2=从缓存表中提取指定名称的缓存依赖项
            //SysCacheModel cacheModel = cacheDAL.GetModelByCacheName(configKey);
            //if (cacheModel != null && 
            //    !string.IsNullOrEmpty(cacheModel.DBName) && 
            //    !string.IsNullOrEmpty(cacheModel.TableNames))
            //{
            //    string[] tables = cacheModel.TableNames.Split(configurationSeparator);
            //    foreach (string tableName in tables)
            //    {
            //        //数据表依赖
            //        dependency.Add(new SqlCacheDependency(cacheModel.DBName, tableName));
            //    }
            //}
        }

        
        /// <summary>
        /// 实现GetDependency方法，返回聚合缓存依赖对象Dependency 
        /// </summary>
        /// <returns></returns>
        public AggregateCacheDependency GetDependency() {
            return dependency;
        }
    }
}
