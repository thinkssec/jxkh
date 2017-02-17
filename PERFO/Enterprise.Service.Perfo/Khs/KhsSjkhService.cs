using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khs;
using Enterprise.Model.Perfo.Khs;

namespace Enterprise.Service.Perfo.Khs
{
	
    /// <summary>
    /// 文件名:  KhsSjkhService.cs
    /// 功能描述: 业务逻辑层-上级考核评分数据处理
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/8 23:49:46
    /// </summary>
    public class KhsSjkhService
    {
        #region 代码生成器
        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhsSjkhData dal = new KhsSjkhData();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsSjkhModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhsSjkhModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhsSjkhModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhsSjkhModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
