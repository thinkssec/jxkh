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
    /// 文件名:  KhZbsxService.cs
    /// 功能描述: 业务逻辑层-考核指标汇总表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhZbsxService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhZbsxData dal = new KhZbsxData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhZbsxModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZbsxModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhZbsxModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 根据考核ID获取所有筛选数据的集合
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByKaohe(string khid)
        {
            string hql = "from KhZbsxModel p where p.KHID='"+khid+"' order by p.SXXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 根据考核ID和机构编码获取所有筛选数据的集合
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhZbsxModel> GetListByKaoheAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhZbsxModel p where p.KHID='" + khid + "' and p.SXJGBM='" + jgbm + "' order by p.SXXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 删除指定条件下的指标筛选数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public bool DeleteByKhidAndJgbm(string khid, string jgbm)
        {
            string sql = "delete from PERFO_KH_ZBSX where KHID='" + khid + "' and SXJGBM='" + jgbm + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
