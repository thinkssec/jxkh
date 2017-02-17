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
    /// 权限设置页面
    /// </summary>
    public partial class RolePermissionSet : PageBase
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        object rroleid;
        /// <summary>
        /// 权限类型
        /// </summary>
        List<SysPermissiontypeModel> permissonTypeList = new List<SysPermissiontypeModel>();
        /// <summary>
        /// 角色权限
        /// </summary>
        List<SysRolepermissionModel> pmList = new List<SysRolepermissionModel>();
        /// <summary>
        /// 选择的模块
        /// </summary>
        SysModuleModel model;
        /// <summary>
        /// 权限服务类
        /// </summary>
        SysPermissiontypeService permissionSrv = new SysPermissiontypeService();
        /// <summary>
        /// 角色服务类
        /// </summary>
        SysRoleService roleSrv = new SysRoleService();
        /// <summary>
        /// 角色和权限设置服务类
        /// </summary>
        SysRolepermissionService rolepermissionSrv = new SysRolepermissionService();
        /// <summary>
        /// 模块服务类
        /// </summary>
        SysModuleService moduleSrv = new SysModuleService();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                rroleid = Request["roleid"];
                BindRoleDdl();
                if (rroleid != null)
                {
                    Roleddl.SelectedValue = rroleid.ToString();
                }
                BindRP();
            }

        }


        /// <summary>
        /// 绑定角色列表
        /// </summary>
        protected void BindRoleDdl()
        {
            List<SysRoleModel> roleLst = roleSrv.GetList().ToList();
            foreach (SysRoleModel role in roleLst)
            {
                ListItem rlist = new ListItem();
                rlist.Text = role.ROLENAME;
                rlist.Value = role.ROLEID;
                Roleddl.Items.Add(rlist);
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
            //获取系统所有权限
            pmList = rolepermissionSrv.GetList().ToList();
            
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
            //获取当前角色、当前模块的权限值
            SysRolepermissionModel rolePermisson = pmList.Find(p => p.ROLEID == Roleddl.SelectedValue && p.MID == model.MID);
            if (rolePermisson != null && 
                Utility.CheckPermission(Convert.ToInt64(rolePermisson.MODULEPERMISSION), Convert.ToInt64(per.ACTIONKEY)))
            {
                chk.Checked = true;
            }
            else
            {
                chk.Checked = false;
            }
            chk.ID = model.MID + "_" + per.ACTIONKEY;
        }


        protected void Roleddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRP();
        }


        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            //先删除选定角色的所有权限
            rolepermissionSrv.DeleteRolePermission(Roleddl.SelectedValue);
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
                SysRolepermissionModel rp = new SysRolepermissionModel();
                rp.DB_Option_Action = WebKeys.InsertAction;
                rp.ROLEID = Roleddl.SelectedValue;
                rp.MID = p.Key.ToString();//模块ID
                //如果集合中没有该模块ID则计算提交的模块权限
                foreach (var t in b)
                {
                    if (t.Split('$').Last().Split('_').First().ToString() == rp.MID)
                    {
                        if (Request[t.ToString()].ToString() == "on")
                            rp.MODULEPERMISSION = rp.MODULEPERMISSION.ToInt() + (int)Math.Pow(2, Convert.ToDouble(t.Split('$').Last().Split('_').Last().ToString()));
                    }
                }
                //提交
                rolepermissionSrv.Execute(rp);
            }
            //重新加载路由表
            Global.LoadUrlRoute();
            BindRP();
        }
    }
}
