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
    /// 文件名:  KhCjqjService.cs
    /// 功能描述: 业务逻辑层-成绩区间设置数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhCjqjService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhCjqjData dal = new KhCjqjData();

	    /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhCjqjModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhCjqjModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhCjqjModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取与考核期对应的成绩区间分布
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByKhid(string khid)
        {
            string hql = "from KhCjqjModel p where p.KHID='" + khid + "' order by p.QJDJ";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取与考核期对应的成绩区间分布（维护用）
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <returns></returns>
        public IList<KhCjqjModel> GetListByKhidForEdit(string khid)
        {
            string hql = "from KhCjqjModel p where p.KHID='" + khid + "' order by p.QJDJ";
            var lst = GetListByHQL(hql);
            List<KhCjqjModel> cjqjLst = new List<KhCjqjModel>();
            cjqjLst.AddRange(lst);
            //总是追加一条
            KhCjqjModel model = new KhCjqjModel();
            model.KHID = khid.ToInt();
            model.QJDJ = "";
            cjqjLst.Add(model);
            return cjqjLst;
        }

        #endregion
    }

}
