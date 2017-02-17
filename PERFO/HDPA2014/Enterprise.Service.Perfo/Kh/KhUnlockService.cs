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
    /// 文件名:  KhUnlockService.cs
    /// 功能描述: 业务逻辑层-数据解锁数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhUnlockService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhUnlockData dal = new KhUnlockData();

	    /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhUnlockModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhUnlockModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	    /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhUnlockModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期的所有数据提交单位
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public IList<KhUnlockModel> GetListByKhid(string khid)
        {
            string hql = "from KhUnlockModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql).OrderBy(p => p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// 删除指定考核期的数据
        /// </summary>
        /// <param name="khid"></param>
        /// <returns></returns>
        public bool DeleteByKhid(string khid)
        {
            string sql = "delete from [PERFO_KH_UNLOCK] where KHID='" + khid + "'";
            return dal.ExecuteSQL(sql);
        }

        #endregion

    }

}
