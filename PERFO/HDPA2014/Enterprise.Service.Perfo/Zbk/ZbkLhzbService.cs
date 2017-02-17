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
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Zbk
{

    /// <summary>
    /// 文件名:  ZbkLhzbService.cs
    /// 功能描述: 业务逻辑层-量化指标维护数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class ZbkLhzbService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IZbkLhzbData dal = new ZbkLhzbData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ZbkLhzbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(ZbkLhzbModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 更新集合中的全部指标的序号
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public bool UpdateZbxhByList(List<ZbkLhzbModel> lst)
        {
            StringBuilder sqls = new StringBuilder();
            foreach (var m in lst)
            {
                sqls.Append("update [PERFO_ZBK_LHZB] set ZBXH='" + m.ZBXH + "' where LHZBBM='" + m.LHZBBM + "';");
            }
            return dal.ExecuteSQL(sqls.ToString());
        }

        /// <summary>
        /// 获取指定单位和版本下的所有有效状态的量化指标集合
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <param name="bbmc">版本名称</param>
        /// <param name="isAll">显示全部</param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListForValid(string jgbm, string bbmc, bool isAll)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            //string jgbms = bmjgSrv.GetJgbmsForSQL(jgbm);
            string s = ((!isAll) ? " p.SFJY='0' and " : "");
            string hql = "from ZbkLhzbModel p where " + s + " p.BBMC='" + bbmc + "'";
            if (!string.IsNullOrEmpty(jgbm))
            {
                hql = "select p from ZbkLhzbModel p,ZbkWcztbModel c where " + s + " p.BBMC='"
                    + bbmc + "' and p.LHZBBM=c.LHZBBM and c.JGBM in (" + jgbm + ")";
            }
            return GetListByHQL(hql).OrderBy(p => p.Zbxx.SXH).ToList();
        }

        /// <summary>
        /// 获取指定单位和版本下的所有有效状态的量化指标集合,按量化指标序号排序
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <param name="bbmc">版本名称</param>
        /// <param name="isAll">显示全部</param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListForValidOrderByZbxh(string jgbm, string bbmc, bool isAll)
        {
            return GetListForValid(jgbm, bbmc, isAll).OrderBy(p => p.ZBXH).ToList();
        }

        /// <summary>
        /// 获取指定版本的定量指标集合
        /// </summary>
        /// <param name="bbmc"></param>
        /// <returns></returns>
        public IList<ZbkLhzbModel> GetListByBBMC(string bbmc)
        {
            string hql = "from ZbkLhzbModel p where p.BBMC='" + bbmc + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 删除与指标相关的填报与审核信息
        /// </summary>
        /// <param name="lhzbbm">定量指标编码</param>
        /// <returns></returns>
        public bool DeleteGljg(string lhzbbm)
        {
            string deleteSQL =
                "delete from [PERFO_ZBK_MBZTB] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_MBZSH] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_WCZTB] where LHZBBM='" + lhzbbm + "';"
                + "delete from [PERFO_ZBK_WCZSHDF] where LHZBBM='" + lhzbbm + "'";
            return dal.ExecuteSQL(deleteSQL);
        }

     
        #endregion

    }

}
