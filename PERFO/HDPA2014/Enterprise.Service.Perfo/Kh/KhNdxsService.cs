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
    /// 文件名:  KhNdxsService.cs
    /// 功能描述: 业务逻辑层-经营难度系数数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhNdxsService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhNdxsData dal = new KhNdxsData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhNdxsModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhNdxsModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhNdxsModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期的所有单位的难度系数集合
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhNdxsModel> GetListByKhid(string khid)
        {
            string hql = "from KhNdxsModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p=>p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// 获取指定考核期和单位的难度系数
        /// </summary>
        /// <param name="khid">考核ID</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public decimal GetNdxsByKhidAndJgbm(string khid, int jgbm)
        {
            var q = GetListByKhid(khid).FirstOrDefault(p=>p.JGBM == jgbm);
            return (q != null) ? q.NDXS.ToDecimal() : 1.0M;
        }

        /// <summary>
        /// 完成难度系数数据的初始化
        /// </summary>
        /// <param name="kaohe">考核期对象</param>
        /// <param name="jxzrsList">绩效责任书集合</param>
        /// <returns></returns>
        public IList<KhNdxsModel> InitKhNdxsByKaoheAndJxzrs(KhKhglModel kaohe, IList<KhJxzrsModel> jxzrsList)
        {
            foreach (var zrs in jxzrsList)
            {
                KhNdxsModel model = new KhNdxsModel();
                model.KHID = kaohe.KHID;
                model.JGBM = zrs.JGBM.Value;
                model.NDXS = 1.0M;
                model.DB_Option_Action = WebKeys.InsertOrUpdateAction;
                Execute(model);
            }
            return GetListByKhid(kaohe.KHID.ToString());
        }

        #endregion
    }

}
