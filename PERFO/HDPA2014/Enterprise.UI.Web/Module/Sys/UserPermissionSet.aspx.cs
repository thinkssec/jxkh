using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Text;
using System.IO;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Sys
{
    /// <summary>
    /// 用户权限设置页面
    /// </summary>
    public partial class UserPermissionSet : PageBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string userLoginId = string.Empty;
        /// <summary>
        /// 权限类型
        /// </summary>
        List<SysPermissiontypeModel> permissonTypeList = new List<SysPermissiontypeModel>();
        /// <summary>
        /// 用户特定权限集合
        /// </summary>
        List<SysUserpermissionModel> userPmList = new List<SysUserpermissionModel>();
        /// <summary>
        /// 用户权限服务类
        /// </summary>
        SysUserpermissionService userPmSrv = new SysUserpermissionService();
        /// <summary>
        /// 选择的模块
        /// </summary>
        SysModuleModel model;
        /// <summary>
        /// 权限服务类
        /// </summary>
        SysPermissiontypeService permissionSrv = new SysPermissiontypeService();
        /// <summary>
        /// 角色所设定的权限
        /// </summary>
        List<SysRolepermissionModel> rolePmList = new List<SysRolepermissionModel>();
        /// <summary>
        /// 角色和权限设置服务类
        /// </summary>
        SysRolepermissionService rolePmSrv = new SysRolepermissionService();
        /// <summary>
        /// 模块服务类
        /// </summary>
        SysModuleService moduleSrv = new SysModuleService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                userLoginId = Request["Uid"];
                BindDdl();
                if (!string.IsNullOrEmpty(userLoginId))
                {
                    Ddl_LoginId.SelectedValue = userLoginId;
                }
                BindRP();
            }

        }


        /// <summary>
        /// 绑定用户列表
        /// </summary>
        protected void BindDdl()
        {
            List<SysUserModel> userLst = userService.GetList() as List<SysUserModel>;
            foreach (SysUserModel user in userLst)
            {
                ListItem item = new ListItem();
                item.Text = string.Format("账号:〖{0}〗☆用户名:〖{1}〗☆单位:〖{2}〗☆角色:〖{3}〗☆职务:〖{4}〗",
                    user.LOGINID, user.USERNAME, user.Bmjg.JGMC, user.Role.ROLENAME, user.DUTY);
                item.Value = user.LOGINID;
                Ddl_LoginId.Items.Add(item);
            }
        }


        /// <summary>
        /// 绑定Reapter.模块列表
        /// </summary>
        protected void BindRP()
        {
            //获取系统所有模块
            List<SysModuleModel> modulelist = moduleSrv.GetList().OrderBy(p => p.MID).ThenBy(p => p.XSXH).ToList();
            //权限类型
            permissonTypeList = permissionSrv.GetList().ToList();
            //获取用户具有的特定权限
            userPmList = userPmSrv.GetList() as List<SysUserpermissionModel>;
            //获取用户的角色所具有的权限
            rolePmList = rolePmSrv.GetList() as List<SysRolepermissionModel>;
            
            rp_module.DataSource = modulelist;
            rp_module.DataBind();
        }

        /// <summary>
        /// 模块列表数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RP_Module_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            model = (SysModuleModel)e.Item.DataItem;
            if (model != null)
            {
                Label lbl = (Label)e.Item.FindControl("Label1");
                if (lbl != null)
                {
                    if (model.MID.Length > 2)
                    {
                        for (int i = 0; i < model.MID.Length / 2; i++)
                        {
                            lbl.Text += "﹄";
                        }
                    }
                }
                if (model.MPARENTID != "0")
                {
                    //绑定所有权限到模块
                    Repeater rppm = new Repeater();
                    rppm = (Repeater)e.Item.FindControl("rp_pm");
                    if (rppm != null)
                    {
                        rppm.DataSource = permissonTypeList;
                        rppm.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// 权限值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RP_PM_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpmodul = new Repeater();
            CheckBox chk = (CheckBox)e.Item.FindControl("chk_pm");
            SysPermissiontypeModel per = (SysPermissiontypeModel)e.Item.DataItem;
            /*
             1=获取选定用户所具有角色的所有权限，对复选控件进行状态控制（角色具有的为不可操作）
             2=提取选定用户所设定的特定权限
             */
            SysUserModel user = userService.GetSingle(Ddl_LoginId.SelectedValue);
            //1==用户角色本身具有的权限
            SysRolepermissionModel rolePermisson = rolePmList.Find(p => p.ROLEID == user.ROLEID && p.MID == model.MID);
            //2==用户设定的专有权限
            SysUserpermissionModel userPermission = userPmList.Find(p=>p.LOGINID == user.LOGINID && p.MID == model.MID);
            //进行判断
            if (rolePermisson != null && 
                Utility.CheckPermission(Convert.ToInt64(rolePermisson.MODULEPERMISSION), Convert.ToInt64(per.ACTIONKEY)))
            {
                chk.Enabled = false;
                chk.Checked = true;
            }
            else if (userPermission != null &&
                Utility.CheckPermission(Convert.ToInt64(userPermission.MODULEPERMISSION), Convert.ToInt64(per.ACTIONKEY)))
            {
                chk.Enabled = true;
                chk.Checked = true;
            }
            else
            {
                chk.Enabled = true;
                chk.Checked = false;
            }
            chk.ID = model.MID + "_" + per.ACTIONKEY;
        }


        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            //先删除选定角色的所有权限
            userPmSrv.DeleteUserPmByLoginId(Ddl_LoginId.SelectedValue);
            //重新添加
            List<string> requestkeys = Request.Form.AllKeys.ToList();
            //Request.Form.AllKeys为何多了一个null?
            requestkeys.Remove(null);
            var q = from s in requestkeys
                    where s.Contains("rp_module$ctl")
                    group s by s.Split('$').Last().Split('_').First() into g
                    select g;
            var b = from s in requestkeys
                    where s.Contains("rp_module$ctl")
                    select s;
            foreach (var p in q)
            {
                SysUserpermissionModel up = new SysUserpermissionModel();
                up.DB_Option_Action = WebKeys.InsertAction;
                up.LOGINID = Ddl_LoginId.SelectedValue;
                up.MID = p.Key.ToString();//模块ID
                //如果集合中没有该模块ID则计算提交的模块权限
                foreach (var t in b)
                {
                    if (t.Split('$').Last().Split('_').First().ToString() == up.MID)
                    {
                        if (Request[t.ToString()].ToString() == "on")
                            up.MODULEPERMISSION = up.MODULEPERMISSION.ToInt() + (int)Math.Pow(2, Convert.ToDouble(t.Split('$').Last().Split('_').Last().ToString()));
                    }
                }
                //提交
                userPmSrv.Execute(up);
            }
            //重新加载路由表
            Global.LoadUrlRoute();
            BindRP();
        }

        /// <summary>
        /// 用户账号切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_LoginId_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRP();
        }
    }
}
