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
    /// 文件名:  KhLhzbjcsjService.cs
    /// 功能描述: 业务逻辑层-量化考核基础数据表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhLhzbjcsjService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhLhzbjcsjData dal = new KhLhzbjcsjData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhLhzbjcsjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhLhzbjcsjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        /// <summary>
        /// 执行批量添加、修改、删除操作
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(IList<KhLhzbjcsjModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        /// <summary>
        /// 根据考核期和机构编码获取量化指标的关联基础指标数据
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhLhzbjcsjModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhLhzbjcsjModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "' order by p.JCZBID";
            return GetListByHQL(hql);
        }
    }

}
