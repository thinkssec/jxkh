using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysCacheService.cs
    /// 功能描述: 业务逻辑层-系统缓存管理数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/10/31 10:02:25
    /// </summary>
    public class SysCacheService
    {

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly ISysCacheData dal = new SysCacheData();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysCacheModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysCacheModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysCacheModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysCacheModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 返回所有缓存信息集合
        /// </summary>
        /// <returns></returns>
        public static IList<SysCacheModel> GetAllCacheList()
        {
            return dal.GetList();
        }

        #endregion

    }

}
