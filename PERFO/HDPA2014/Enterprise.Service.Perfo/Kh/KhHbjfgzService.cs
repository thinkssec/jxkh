using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhHbjfgzService.cs
    /// 功能描述: 业务逻辑层-合并计分规则表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/12/2 13:41:05
    /// </summary>
    public class KhHbjfgzService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhHbjfgzData dal = new KhHbjfgzData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhHbjfgzModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhHbjfgzModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 提取与考核期相对应的合并计分规则信息
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhHbjfgzModel> GetListByKhid(string khid)
        {
            string hql = "from KhHbjfgzModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        #endregion

    }

}
