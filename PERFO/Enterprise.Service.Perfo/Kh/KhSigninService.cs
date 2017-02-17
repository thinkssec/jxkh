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
    /// 文件名:  KhSigninService.cs
    /// 功能描述: 业务逻辑层-通知签收表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhSigninService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhSigninData dal = new KhSigninData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhSigninModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhSigninModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhSigninModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhSigninModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhSigninModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region

        /// <summary>
        /// 获取通知ID对应的签收信息
        /// </summary>
        /// <param name="tzid">通知ID</param>
        /// <returns></returns>
        public IList<KhSigninModel> GetListByTZID(string tzid)
        {
            string hql = "from KhSigninModel p where p.TZID='" + tzid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 完成签收操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SignInTongzhi(KhSigninModel model)
        {
            string hql = "from KhSigninModel p where p.TZID='" + model.TZID + "' and p.QSR='" + model.QSR + "'";
            var q = GetListByHQL(hql).FirstOrDefault();
            if (q == null)
            {
                return Execute(model);
            }
            return true;
        }

        #endregion

    }

}
