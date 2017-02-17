using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;

namespace Enterprise.Service.Perfo.Zbk
{
	
    /// <summary>
    /// 文件名:  ZbkBanbenService.cs
    /// 功能描述: 业务逻辑层-指标版本管理数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class ZbkBanbenService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IZbkBanbenData dal = new ZbkBanbenData();

	/// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkBanbenModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetList()
        {
            return dal.GetList().OrderByDescending(p => p.QYSJ).ToList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkBanbenModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkBanbenModel model)
        {
            
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 删除指定版本的所有指标
        /// </summary>
        /// <param name="bbmc">版本名称</param>
        /// <returns></returns>
        public bool DeleteZbkDataByBBMC(string bbmc)
        {
            StringBuilder sqlSB = new StringBuilder();
            //==先删除已有数据
            sqlSB.Append("delete from PERFO_ZBK_MBZTB where LHZBBM in (select LHZBBM from PERFO_ZBK_LHZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_MBZSH where LHZBBM in (select LHZBBM from PERFO_ZBK_LHZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_WCZTB where LHZBBM in (select LHZBBM from PERFO_ZBK_LHZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_WCZSHDF where LHZBBM in (select LHZBBM from PERFO_ZBK_LHZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_BDFJG where DFZBBM in (select DFZBBM from PERFO_ZBK_DFZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_DFZ where DFZBBM in (select DFZBBM from PERFO_ZBK_DFZB where BBMC='" + bbmc + "');");
            sqlSB.Append("delete from PERFO_ZBK_DFZB where BBMC='" + bbmc + "';");
            sqlSB.Append("delete from PERFO_ZBK_LHZB where BBMC='" + bbmc + "';");
            sqlSB.Append("delete from PERFO_ZBK_JSGZ where BBMC='" + bbmc + "';");
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        #endregion
    }

}
