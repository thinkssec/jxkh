using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Kh
{

    /// <summary>
    /// 文件名:  KhJgbmdfbService.cs
    /// 功能描述: 业务逻辑层-机关部门打分表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:45
    /// </summary>
    public class KhJgbmdfbService
    {

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly IKhJgbmdfbData dal = new KhJgbmdfbData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KhJgbmdfbModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(KhJgbmdfbModel model)
        {
            return dal.Execute(model);
        }

        /// <summary>
        /// 执行批量添加、修改、删除操作
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public bool ExecuteByList(IList<KhJgbmdfbModel> models)
        {
            bool isOk = false;
            foreach (var m in models)
            {
                isOk = dal.Execute(m);
            }
            return isOk;
        }

        #endregion

        #region 自定义方法区

        /// <summary>
        /// 获取指定考核期下的所有指标的打分信息
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期下的所有指标的打分信息
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhidAndJgbm(string khid, string jgbm)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.JGBM='" + jgbm + "'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期下的所有量化指标的打分信息
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetDlzbListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.ZBBM like 'LH%'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期下的所有打分指标的打分信息
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetDfzbListByKhid(string khid)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and p.ZBBM like 'DF%'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定考核期和审核人可打分的指标集合
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="user">用户MODEL</param>
        /// <returns></returns>
        public IList<KhJgbmdfbModel> GetListByKhidAndDfz(string khid, SysUserModel user)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and (p.DFZ='" + user.JGBM + "' or p.DFZ='" + user.LOGINID + "')";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 检测当前用户是否具有打分权限
        /// 绩效管理员默认有权限
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="zbbm">指标编码</param>
        /// <param name="user">用户MODEL</param>
        /// <returns></returns>
        public bool CheckUserPermessionForDfzb(string khid, string zbbm, SysUserModel user)
        {
            var dfzbLst = GetListByKhidAndDfz(khid, user);
            return (dfzbLst.FirstOrDefault(p => p.ZBBM == zbbm) != null) || user.Role.ROLEID == "paadmin";
        }

        #endregion

        #region 静态方法区

        /// <summary>
        /// 检测当前用户是否有权限操作指定的考核期和机构的数据
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="jgbm">补考核机构</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public static bool IsDfzUser(string khid, int jgbm, SysUserModel user)
        {
            KhJgbmdfbService srv = new KhJgbmdfbService();
            var dfzbLst = srv.GetListByKhidAndDfz(khid, user);
            return (dfzbLst.FirstOrDefault(p => p.JGBM == jgbm) != null);
        }

        /// <summary>
        /// 检测当前用户是否具有指标的审核权限
        /// </summary>
        /// <param name="khid">考核期</param>
        /// <param name="khid">机构编码</param>
        /// <param name="zbbm">指标编码</param>
        /// <param name="user">用户MODEL</param>
        /// <returns></returns>
        public static bool IsDfzForJgbmAndZbbm(string khid,int jgbm, string zbbm, SysUserModel user)
        {
            string hql = "from KhJgbmdfbModel p where p.KHID='" + khid + "' and (p.DFZ='" + user.JGBM + "' or p.DFZ='" + user.LOGINID + "')";
            var dfzbLst = dal.GetListByHQL(hql);
            return (dfzbLst.FirstOrDefault(p =>p.JGBM == jgbm && p.ZBBM == zbbm) != null);
        }

        #endregion

    }

}
