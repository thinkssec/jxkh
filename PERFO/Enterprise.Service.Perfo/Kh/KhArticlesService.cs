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
    /// 文件名:  KhArticlesService.cs
    /// 功能描述: 业务逻辑层-通知公告数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:29:31
    /// </summary>
    public class KhArticlesService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhArticlesData dal = new KhArticlesData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhArticlesModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhArticlesModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhArticlesModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhArticlesModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhArticlesModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取指定年度下的所有通知公告信息
        /// </summary>
        /// <param name="yy"></param>
        /// <returns></returns>
        public IList<KhArticlesModel> GetTzListByYear(string yy)
        {
            List<KhArticlesModel> list = new List<KhArticlesModel>();
            string hql = "from KhArticlesModel p ";
            if (!string.IsNullOrEmpty(yy))
            {
                hql += " where year(p.TJRQ)='" + yy + "' ";
            }
            hql += " order by p.TJRQ desc";
            list = GetListByHQL(hql) as List<KhArticlesModel>;
            if (list.Count == 0) 
            {
                var q = GetList().FirstOrDefault();
                if (q != null)
                    list.Add(q);
            }
            return list;
        }

        /// <summary>
        /// 删除指定的通知记录
        /// </summary>
        /// <param name="tzid">ID</param>
        /// <returns></returns>
        public bool DeleteArticleById(string tzid)
        {
            string sql = "delete from [PERFO_KH_SIGNIN] where TZID='" + tzid
                + "';delete from [PERFO_KH_ARTICLES] where TZID='" + tzid + "';";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
