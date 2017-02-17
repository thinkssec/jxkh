using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Service.Perfo.Sys;

namespace Enterprise.Service.Perfo.Zbk
{
	
    /// <summary>
    /// 文件名:  ZbkDfzbService.cs
    /// 功能描述: 业务逻辑层-打分指标维护数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class ZbkDfzbService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IZbkDfzbData dal = new ZbkDfzbData();

	/// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkDfzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

	/// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkDfzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 根据指标编码删除打分指标
        /// </summary>
        /// <param name="dfzbbm"></param>
        /// <returns></returns>
        public bool DeleteDFZB(string dfzbbm)
        {
            string deleteSQL = 
                @"begin
            delete from PERFO_ZBK_BDFJG where DFZBBM='" + dfzbbm + "';"
                + "delete from PERFO_ZBK_DFZ where DFZBBM='" + dfzbbm + "';"
                + "delete from PERFO_ZBK_DFZB where DFZBBM='" + dfzbbm + "';end;";
            return dal.ExecuteSQL(deleteSQL);
        }

        /// <summary>
        /// 获取指定版本的打分指标集合
        /// </summary>
        /// <param name="bbmc"></param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListByBBMC(string bbmc)
        {
            string hql = "from ZbkDfzbModel p where p.BBMC='" + bbmc + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定单位和版本下的所有有效状态的打分指标集合
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <param name="bbmc">版本名称</param>
        /// <param name="isAll">显示全部</param>
        /// <returns></returns>
        public IList<ZbkDfzbModel> GetListForValid(string jgbm, string bbmc, bool isAll)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            //string jgbms = bmjgSrv.GetJgbmsForSQL(jgbm);
            string s = ((!isAll) ? " p.DISABLE='0' and " : "");
            string hql = "from ZbkDfzbModel p where " + s + " p.BBMC='" + bbmc + "'";
            if (!string.IsNullOrEmpty(jgbm))
            {
                hql = "select p from ZbkDfzbModel p,ZbkBdfjgModel c where " + s + " p.BBMC='"
                    + bbmc + "' and p.DFZBBM=c.DFZBBM and c.JGBM in (" + jgbm + ")";
            }
            return GetListByHQL(hql).OrderBy(p => p.Zbxx.SXH).ToList();
        }

        #endregion

    }

}
