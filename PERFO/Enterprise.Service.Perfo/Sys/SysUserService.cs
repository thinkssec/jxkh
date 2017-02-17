using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

using Enterprise.Component.Infrastructure;
using Enterprise.Data.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;

namespace Enterprise.Service.Perfo.Sys
{

    /// <summary>
    /// 文件名:  SysUserService.cs
    /// 功能描述: 业务逻辑层-用户表数据处理
    /// 创建人：代码生成器
    /// 创建时间 ：2014/11/1 0:35:46
    /// </summary>
    public class SysUserService
    {

        /// <summary>
        /// 分管部门机构-服务类
        /// </summary>
        SysFgbmjgService fgbmjgSrv = new SysFgbmjgService();

        #region 代码生成器

        /// <summary>
        /// 得到数据访问类实例
        /// </summary>
        private static readonly ISysUserData dal = new SysUserData();

        /// <summary>
        /// 根据主键获取唯一记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SysUserModel GetSingle(string key)
        {
            return dal.GetSingle(key);
        }

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetList()
        {
            return dal.GetList();
        }

        /// <summary>
        /// 根据条件获取数据集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetListByHQL(string hql)
        {
            return dal.GetListByHQL(hql);
        }

        /// <summary>
        /// 返回原生SQL的查询列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<SysUserModel> GetListBySQL(string sql)
        {
            return dal.GetListBySQL(sql);
        }

        /// <summary>
        /// 执行添加、修改、删除操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Execute(SysUserModel model)
        {
            return dal.Execute(model);
        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 获取指定用户的所有分管机构集合
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public List<SysFgbmjgModel> GetFgjgbmList(string loginId)
        {
            return fgbmjgSrv.GetListByHQL("from SysFgbmjgModel p where p.LOGINID='" + loginId + "'").ToList();
        }

        /// <summary>
        /// 删除指定用户所有分管机构
        /// </summary>
        /// <param name="roleId"></param>
        public bool DeleteFgjgbm(string loginId)
        {
            string sql = "delete from PERFO_SYS_FGBMJG where LOGINID='" + loginId + "'";
            return dal.ExecuteSQL(sql);
        }

        /// <summary>
        /// 获取所有处于有效状态的用户信息集合
        /// </summary>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListForValid()
        {
            string hql = "from SysUserModel p where p.DISABLE='0'";
            return GetListByHQL(hql);
        }

        /// <summary>
        /// 获取指定单位的分管领导用户MODEL
        /// </summary>
        /// <param name="jgbm">机构编码</param>
        /// <returns></returns>
        public SysUserModel GetUserByFgjgbm(int jgbm)
        {
            SysUserModel user = null;
            var fgbmLst = fgbmjgSrv.GetListByHQL("from SysFgbmjgModel p where p.JGBM='" + jgbm + "'").ToList();
            if (fgbmLst != null && fgbmLst.Count > 0)
            {
                user = GetSingle(fgbmLst.First().LOGINID);
            }
            return user;
        }

        /// <summary>
        /// 获取指定的同职务名称的所有用户信息集合
        /// </summary>
        /// <param name="dutyName">职务名称</param>
        /// <returns></returns>
        public IList<SysUserModel> GetUserListByDuty(string dutyName)
        {
            return GetUserListForValid().Where(p => p.DUTY == dutyName).ToList();
        }

        #endregion

        #region 静态方法区

        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="loginId">登录名</param>
        /// <returns></returns>
        public static string GetUserNameByLoginId(string loginId)
        {
            var u = dal.GetSingle(loginId);
            return (u != null) ? u.USERNAME : "";
        }

        #endregion

        #region 生成菜单相关

        private string CaiDanHtml;
        private SysRolepermissionService roleService = new SysRolepermissionService();
        private SysUserpermissionService userPmService = new SysUserpermissionService();
        private SysModuleService moduleService = new SysModuleService();
        //获取该用户有权限的所有模块
        List<SysModuleModel> moduleList = new List<SysModuleModel>();

        /// <summary>
        /// 根据用户权限加载折叠式菜单
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public static string LoadAccordionMenu(SysUserModel userModel)
        {
            SysUserService userSrv = new SysUserService();
            return userSrv.CreateZheDieCaiDanHtml(userModel);
        }

        /// <summary>
        /// 根据用户权限加载树型菜单
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public static string LoadTreeMenu(SysUserModel userModel)
        {
            SysUserService userSrv = new SysUserService();
            return userSrv.CreateTreeMenu(userModel);
        }

        #region 收拉型菜单

        /// <summary>
        /// 折叠菜单
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        private string CreateZheDieCaiDanHtml(SysUserModel userModel)
        {
            if (userModel != null)
            {
                CaiDanHtml = "";
                //获取该用户的角色权限
                List<SysRolepermissionModel> pmList = roleService.GetList().
                    Where(p => p.ROLEID == userModel.ROLEID).ToList();
                //提取当前用户的特有权限
                List<SysUserpermissionModel> userPmList = userPmService.GetList().
                    Where(p => p.LOGINID == userModel.LOGINID).ToList();
                //提取所有模块信息
                var moduleLst = moduleService.GetList();
                //根据角色权限提取相应的模块信息
                foreach (SysRolepermissionModel pm in pmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == pm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //根据用户特用权限提取相应的模块信息
                foreach (SysUserpermissionModel upm in userPmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == upm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                CreateCaiDan("0");
            }
            return CaiDanHtml;
        }

        /// <summary>
        /// 建立一级菜单
        /// </summary>
        /// <param name="Parentid"></param>
        private void CreateCaiDan(string Parentid)
        {
            var q = from root in moduleList
                    where root.MPARENTID == Parentid
                    select root;
            int i = 0;
            foreach (var t in q)
            {
                int a = moduleList.Count(x => x.MPARENTID == t.MID);
                //如果没有任何子节点 则说明没有权限 不绑定
                if (a != 0)
                {
                    string iconCss = (!string.IsNullOrEmpty(t.BZ)) ? t.BZ : "icon-sipc";
                    if (i == 0)
                        CaiDanHtml += "<div title=\"" + t.MNAME + "\" selected=\"true\"  style=\"padding:10px;overflow-x: hidden;\" iconCls=\"" + iconCss + "\">";
                    else
                        CaiDanHtml += "<div title=\"" + t.MNAME + "\" style=\"padding:10px;overflow-x: hidden;\" iconCls=\"" + iconCss + "\">";
                    CreateZiCaiDan(t.MID);
                    CaiDanHtml += "</div>";
                }
                i++;
            }
        }

        /// <summary>
        /// 建立二级菜单
        /// </summary>
        /// <param name="IntParent"></param>
        private void CreateZiCaiDan(string IntParent)
        {
            var q = from root in moduleList
                    where root.MPARENTID == IntParent
                    orderby root.XSXH
                    select root;
            foreach (var t in q)
            {
                CaiDanHtml += "<span><a style=\"line-height:30px;text-decoration: none;\" class=\"leftMenu\" href=\"javascript:addTab('/"
                    + t.WEBURL + "','" + t.MNAME + "')\">" + t.MNAME + "</a></span>";
            }
        }

        #endregion
        
        #region 树型菜单

        private string CreateTreeMenu(SysUserModel userModel)
        {
            if (userModel != null)
            {
                CaiDanHtml = "";
                //获取该用户的角色权限
                List<SysRolepermissionModel> pmList = roleService.GetList().
                    Where(p => p.ROLEID == userModel.ROLEID).ToList();
                //提取当前用户的特有权限
                List<SysUserpermissionModel> userPmList = userPmService.GetList().
                    Where(p => p.LOGINID == userModel.LOGINID).ToList();
                //提取所有模块信息
                var moduleLst = moduleService.GetList();
                //根据角色权限提取相应的模块信息
                foreach (SysRolepermissionModel pm in pmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == pm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //根据用户特用权限提取相应的模块信息
                foreach (SysUserpermissionModel upm in userPmList)
                {
                    List<SysModuleModel> mList = moduleLst.Where(p => (p.MID == upm.MID || p.MPARENTID == "0") && p.DISABLE == "0").
                        OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                    foreach (SysModuleModel model in mList)
                    {
                        if (!moduleList.Exists(p => p.MID == model.MID))
                        {
                            moduleList.Add(model);
                        }
                    }
                }
                //moduleList = moduleService.GetList().OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
                CreateTree("0");
            }
            return CaiDanHtml;
        }

        private void CreateTree(string parentID)
        {
            var q = from root in moduleList
                    where root.MPARENTID == parentID
                    orderby root.XSXH
                    select root;
            foreach (var t in q)
            {  
                int a = moduleList.Count(x => x.MPARENTID == t.MID);
                //mod by qw 2014.12.24 限制节点为两级
                if (t.MID.Length > 4 || (a == 0 && parentID == "0")) continue;

                string node = (string.IsNullOrEmpty(t.WEBURL) || parentID == "0") ? "<span>" + t.MNAME + "</span>" : 
                    "<span><a style=\"text-decoration: none;\" href=\"javascript:addTab('/" + t.WEBURL + "','" 
                    + t.MNAME + "');\">" + t.MNAME + "</a></span>";
                //CaiDanHtml += "<li>"+node;
                //如果有子节点
                if (a != 0)
                {
                    CaiDanHtml += "<li ";
                    if (t.MID.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(t.BZ))
                        {
                            CaiDanHtml += " state=\"closed\" iconCls=\"" + t.BZ + "\" ";
                        }
                        else
                        {
                            //CaiDanHtml += " iconCls=\"icon-sipc\" ";
                        }
                    }
                    CaiDanHtml += ">" + node;
                    //有子节点
                    CaiDanHtml += "<ul>";
                    CreateTree(t.MID);
                    CaiDanHtml += "</ul>";
                }
                else
                {
                    CaiDanHtml += "<li>" + node;
                    CaiDanHtml += "</li>";
                }
            }
        }       

        #endregion

        #endregion

    }
}
