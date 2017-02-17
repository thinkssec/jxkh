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
    /// 文件名:  KhZzjgService.cs
    /// 功能描述: 业务逻辑层-最终结果表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhZzjgService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhZzjgData dal = new KhZzjgData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhZzjgModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZzjgModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZzjgModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 获取考核期对应的结果文件集合
        /// </summary>
        /// <returns></returns>
        public IList<KhZzjgModel> GetListByKhid(string khid)
        {
            string hql = "from KhZzjgModel p where p.KHID='" + khid + "'";
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhZzjgModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhZzjgModel model)
        {
            return dal.Execute(model);
        }

        #endregion
    }

}
