using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Khx;
using Enterprise.Model.Perfo.Khx;

namespace Enterprise.Service.Perfo.Khx
{
	
    /// <summary>
    /// 文件名:  KhxResultService.cs
    /// 功能描述: 业务逻辑层-考核得分数据处理
    /// 创建人：代码生成器
	/// 创建时间 ：2015/11/12 19:14:36
    /// </summary>
    public class KhxResultService
    {
        #region 代码生成器
        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhxResultData dal = new KhxResultData();

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhxResultModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhxResultModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhxResultModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhxResultModel model)
        {
            return dal.Execute(model);
        }
        #endregion
    }

}
