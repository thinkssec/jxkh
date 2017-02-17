using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{
	
    /// <summary>
    /// 文件名:  ZbkZbxxService.cs
    /// 功能描述: 业务逻辑层-指标管理数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class ZbkZbxxService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IZbkZbxxData dal = new ZbkZbxxData();

	/// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkZbxxModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkZbxxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取指定类型下的处于有效状态的指标集合
        /// </summary>
        /// <param name="zblx"></param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListForValid(string zblx)
        {
            return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' and p.ZBZT='1' order by p.SXH");
        }

        /// <summary>
        /// 获取指定类型下的所有指标集合
        /// </summary>
        /// <param name="zblx">指标类型</param>
        /// <param name="yjzbmc">一级名称</param>
        /// <returns></returns>
        public IList<ZbkZbxxModel> GetListByZblxAndYjzbmc(string zblx,string yjzbmc)
        {
            if (!string.IsNullOrEmpty(zblx) && !string.IsNullOrEmpty(yjzbmc))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' and p.YJZBMC='" + yjzbmc + "' order by p.SXH");
            }
            else if (!string.IsNullOrEmpty(zblx))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.ZBLX='" + zblx + "' order by p.SXH");
            }
            else if (!string.IsNullOrEmpty(yjzbmc))
            {
                return dal.GetListByHQL("from ZbkZbxxModel p where p.YJZBMC='" + yjzbmc + "' order by p.SXH");
            }
            else
            {
                return dal.GetList();
            }
        }

        #endregion
    }

}
