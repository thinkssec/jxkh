using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhJxzrszbService.cs
    /// 功能描述: 业务逻辑层-年度绩效责任书指标数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrszbService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhJxzrszbData dal = new KhJxzrszbData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJxzrszbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJxzrszbModel model)
        {
            return dal.Execute(model);
        }


        #endregion

        #region 自定义方法区

        /// <summary>
        /// 按年度和单位编码获取责任书考核指标集合
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetValidListBySearch(string niandu, string jgbm)
        {
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.ZJGBM='" + jgbm + "' order by p.BBMC desc, p.ZXSXH";
            var list = GetListByHQL(hql).Where(p=>p.Jxzrs.ZRSZT == "1").ToList();
            var bbmcs = list.Distinct<KhJxzrszbModel>(new FastPropertyComparer<KhJxzrszbModel>("BBMC"));
            if (bbmcs.Count() > 1)
            {
                list = list.Where(p => p.BBMC == bbmcs.First().BBMC).ToList();
            }
            return list.ToList();
        }

        /// <summary>
        /// 按年度和单位编码、指标版本获取责任书考核指标集合
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构</param>
        /// <param name="bbmc">版本名称</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListBySearch(string niandu, string jgbm, string bbmc)
        {
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.ZJGBM='" + jgbm + "' and p.BBMC='" + bbmc + "' order by p.ZXSXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 按年度和版本名称获取所有绩效指标
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="bbmc">版本名称</param>
        /// <returns></returns>
        public IList<KhJxzrszbModel> GetListByNdAndBbmc(string niandu, string bbmc)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrszbModel p where p.ZSZND='" + niandu + "' and p.BBMC='" + bbmc + "' order by p.ZJGBM,p.ZXSXH";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 删除责任书指标和筛选指标
        /// </summary>
        /// <param name="model">KhJxzrszbModel</param>
        /// <returns></returns>
        public bool DeleteJxzrsZbAndZbsx(KhJxzrszbModel model)
        {
            StringBuilder sqlSB = new StringBuilder();
            sqlSB.Append("delete from [PERFO_KH_ZBSX] where ZRSZBID='" + model.ZRSZBID + "';");
            sqlSB.Append("delete from [PERFO_KH_JXZRSZB] where ZRSZBID='" + model.ZRSZBID + "';");
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        /// <summary>
        /// 批量更新绩效指标中的归属同一量化指标的权重值
        /// </summary>
        /// <param name="model">KhJxzrszbModel</param>
        /// <returns></returns>
        public bool UpdateZbqzByLhzbbm(KhJxzrszbModel model)
        {
            StringBuilder sqlSB = new StringBuilder();
            sqlSB.Append("update [PERFO_KH_JXZRSZB] set ZZBQZ='" + model.ZZBQZ + "' where LHZBBM='" 
                + model.LHZBBM + "' and ZJGBM='" + model.ZJGBM + "' and BBMC='" + model.BBMC 
                + "' and ZSZND='" + model.ZSZND + "';");
            return dal.ExecuteSQL(sqlSB.ToString());
        }

        #endregion
    }

}
