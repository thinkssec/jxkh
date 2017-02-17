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
    /// 文件名:  KhJxzrsService.cs
    /// 功能描述: 业务逻辑层-量化指标考核表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhJxzrsService
    {
        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhJxzrsData dal = new KhJxzrsData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJxzrsModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJxzrsModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 删除指定单位下的所有绩效责任书指标
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteJxzrsData(KhJxzrsModel model)
        {
            StringBuilder sqls = new StringBuilder();
            sqls.Append("delete from [PERFO_KH_JXZRSZB] where ZRSID='" + model.ZRSID + "';");
            sqls.Append("delete from [PERFO_KH_JXZRS] where ZRSID='" + model.ZRSID + "';");
            return dal.ExecuteSQL(sqls.ToString());
        }

        /// <summary>
        /// 按年度和单位编码获取责任书集合
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构</param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByNdAndBmjg(string niandu, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM in (" + bmjgSrv.GetJgbmsForSQL(jgbm) + ")";
            return GetListByHQL(hql).OrderBy(p=>p.KhKind.LXID).ThenBy(p=>p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// 按年度、单位编码和考核类型获取责任书集合
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构</param>
        /// <param name="khlx">考核类型</param>
        /// <returns></returns>
        public IList<KhJxzrsModel> GetListByNd_Bmjg_Khlx(string niandu, string jgbm, string khlx)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM in (" + bmjgSrv.GetJgbmsForSQL(jgbm) + ") and p.LXID='" + khlx + "'";
            return GetListByHQL(hql).OrderBy(p => p.Bmjg.XSXH).ToList();
        }

        /// <summary>
        /// 按年度和单位编码获取其责任书
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构</param>
        /// <returns></returns>
        public KhJxzrsModel GetModelByNdAndBmjg(string niandu, string jgbm)
        {
            SysBmjgService bmjgSrv = new SysBmjgService();
            string hql = "from KhJxzrsModel p where p.SZND='" + niandu + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql).FirstOrDefault();
        }

        /// <summary>
        /// 检测当前用户是否为绩效责任书分管部门
        /// </summary>
        /// <param name="niandu">年度</param>
        /// <param name="jgbm">机构编码</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public bool IsJxzrsFgbm(string niandu, string jgbm, SysUserModel user)
        {
            var model = GetModelByNdAndBmjg(niandu, jgbm);
            if (model != null)
            {
                return (model.FZBM == user.JGBM);
            }
            return false;
        }

        #endregion

    }
}
