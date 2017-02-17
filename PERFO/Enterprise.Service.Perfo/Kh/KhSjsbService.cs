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
    /// 文件名:  KhSjsbService.cs
    /// 功能描述: 业务逻辑层-数据上报数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhSjsbService
    {

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhSjsbData dal = new KhSjsbData();

	    /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhSjsbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhSjsbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhSjsbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期的数据集合
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetListByKhid(string khid)
        {
            string hql = "from KhSjsbModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.Bmjg.BZ).ToList();
        }

        /// <summary>
        /// 获取指定考核年份下的所有参与单位，确保唯一性
        /// </summary>
        /// <param name="maxKhnd"></param>
        /// <returns></returns>
        public IList<KhSjsbModel> GetSjsbListByKhnd(int maxKhnd)
        {
            string hql = "select p from KhSjsbModel p, KhKhglModel c where p.KHID=c.KHID and c.KHND='" + maxKhnd + "'";
            var lst = GetListByHQL(hql).DistinctBy(p => p.JGBM).ToList();
            return lst;
        }

        #endregion

    }

}
