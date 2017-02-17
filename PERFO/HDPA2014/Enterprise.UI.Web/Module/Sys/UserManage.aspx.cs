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
    /// 用户管理页面
    /// </summary>
    public partial class UserManage : PageBase
    {

        /// <summary>
        /// 用户服务类
        /// </summary>
        SysUserService userSrv = new SysUserService();
        /// <summary>
        /// 角色服务类
        /// </summary>
        SysRoleService roleSrv = new SysRoleService();
        /// <summary>
        /// 部门机构服务类
        /// </summary>
        SysBmjgService bmjgSrv = new SysBmjgService();
        /// <summary>
        /// 分管机构服务类
        /// </summary>
        SysFgbmjgService fgbmjgSrv = new SysFgbmjgService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRoleddl();
                BindDanweiddl();
                BindListView();
                clearText();
            }
        }

        /// <summary>
        /// 绑定角色下拉控件
        /// </summary>
        protected void BindRoleddl()
        {
            List<SysRoleModel> roleList = roleSrv.GetList().ToList();
            Ddl_ROLEID.DataSource = roleList;
            Ddl_ROLEID.DataTextField = "ROLENAME";
            Ddl_ROLEID.DataValueField = "ROLEID";
            Ddl_ROLEID.DataBind();
        }

        /// <summary>
        /// 绑定单位下拉控件
        /// </summary>
        protected void BindDanweiddl()
        {
            List<SysBmjgModel> bmjgList = bmjgSrv.GetList().OrderBy(p => p.XSXH).ToList();
            Rdl_JGBM.Items.Clear();
            foreach (var q in bmjgList)
            {
                string txt = "";
                for (int i = 0; i < q.XSXH.Length / 2 - 1; i++)
                {
                    txt += "﹄";
                }
                ListItem item = new ListItem(txt + q.JGMC, q.JGBM.ToString());
                Rdl_JGBM.Items.Add(item);
            }
            if (Rdl_JGBM.Items.Count > 0)
                Rdl_JGBM.SelectedIndex = 0;

            //分管机构
            Chk_FGJGBM.DataSource = bmjgList.Where(p => p.XSXH.Length > 4).OrderBy(p => p.XSXH).ToList();
            Chk_FGJGBM.DataTextField = "JGMC";
            Chk_FGJGBM.DataValueField = "JGBM";
            Chk_FGJGBM.DataBind();
        }


        /// <summary>
        /// 绑定用户列表
        /// </summary>
        protected void BindListView()
        {
            List<SysUserModel> userList = null;
            if (!string.IsNullOrEmpty(Txt_User_Search.Text))
            {
                userList = userSrv.GetList().Where(p => p.LOGINID.Contains(Txt_User_Search.Text) || p.USERNAME.Contains(Txt_User_Search.Text)).ToList();
            }
            else
            {
                userList = userSrv.GetList() as List<SysUserModel>;
            }
            ListView1.DataSource = userList;
            ListView1.DataBind();
        }


        #region ListView事件处理

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //用户相关
            SysUserModel model = e.Item.DataItem as SysUserModel;
            Image img = (Image)e.Item.FindControl("lbdisable");
            if (img.ImageUrl == "1")
            {
                img.ImageUrl = "/Resources/Images/userdisable.gif";
            }
            else
            {
                img.ImageUrl = "/Resources/Images/userable.gif";
            }
            Label jgmc = (Label)e.Item.FindControl("lbjgmc");
            jgmc.Text = model.Bmjg.JGMC;
            //角色相关
            Image rolePic = (Image)e.Item.FindControl("lbpic");
            rolePic.ImageUrl = string.Format("/Resources/Images/role/{0}", model.Role.ROLEPICTURE);
            Label roleName = (Label)e.Item.FindControl("lbrolename");
            roleName.Text = model.Role.ROLENAME;
        }

        protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectId = ListView1.SelectedDataKey.Value.ToString();
            SysUserModel zhcnModel = userSrv.GetSingle(selectId);
            if (zhcnModel != null)
            {
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Txt_", true);
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Ddl_", true);
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Rdl_", true);
                //分管机构
                List<SysFgbmjgModel> fgjgLst = userSrv.GetFgjgbmList(zhcnModel.LOGINID);
                foreach (ListItem item in Chk_FGJGBM.Items)
                {
                    if (fgjgLst.Exists(p => p.JGBM == Convert.ToInt32(item.Value)))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }

                Txt_PASSWORD.Attributes["value"] = DESEncrypt.Decrypt(zhcnModel.PASSWORD);
                Hid_LOGINID.Value = zhcnModel.LOGINID;
            }

            LnkBtn_Ins.Visible = false;
            LnkBtn_Upd.Visible = true;
            LnkBtn_Del.Visible = true;
            Pnl_Edit.Visible = true;
        }


        protected void ListView1_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            ListView1.SelectedIndex = e.NewSelectedIndex;
            BindListView();
        }

        #endregion


        #region 按钮事件处理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //中文
            SysUserModel zhcnModel = (SysUserModel)CommonTool.GetFormDataToModel(typeof(SysUserModel), Page);
            zhcnModel.DB_Option_Action = WebKeys.InsertAction;
            zhcnModel.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
            zhcnModel.PASSWORD = DESEncrypt.Encrypt(Txt_PASSWORD.Text);
            zhcnModel.ADDDATE = DateTime.Now;
            //add
            userSrv.Execute(zhcnModel);

            //添加分管机构
            userSrv.DeleteFgjgbm(zhcnModel.LOGINID);
            foreach (ListItem item in Chk_FGJGBM.Items)
            {
                if (item.Selected)
                {
                    SysFgbmjgModel fgjgModel = new SysFgbmjgModel();
                    fgjgModel.DB_Option_Action = WebKeys.InsertAction;
                    fgjgModel.LOGINID = zhcnModel.LOGINID;
                    fgjgModel.JGBM = item.Value.ToInt();
                    fgbmjgSrv.Execute(fgjgModel);
                }
            }
            
            clearText();
            BindDanweiddl();
            BindListView();
            
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_LOGINID.Value))
            {
                //中文
                SysUserModel zhcnModel = (SysUserModel)CommonTool.GetFormDataToModel(typeof(SysUserModel), Page);
                zhcnModel.DB_Option_Action = WebKeys.UpdateAction;
                zhcnModel.LOGINID = Hid_LOGINID.Value;
                zhcnModel.DISABLE = (Chk_DISABLE.Checked) ? "1" : "0";
                zhcnModel.PASSWORD = DESEncrypt.Encrypt(Txt_PASSWORD.Text);
                zhcnModel.ADDDATE = DateTime.Now;
                //update
                userSrv.Execute(zhcnModel);

                //更新分管机构
                userSrv.DeleteFgjgbm(zhcnModel.LOGINID);
                foreach (ListItem item in Chk_FGJGBM.Items)
                {
                    if (item.Selected)
                    {
                        SysFgbmjgModel fgjgModel = new SysFgbmjgModel();
                        fgjgModel.DB_Option_Action = WebKeys.InsertAction;
                        fgjgModel.LOGINID = zhcnModel.LOGINID;
                        fgjgModel.JGBM = item.Value.ToInt();
                        fgbmjgSrv.Execute(fgjgModel);
                    }
                }
            }
            clearText();
            BindDanweiddl();
            BindListView();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_LOGINID.Value))
            {
                SysUserModel userM = userSrv.GetSingle(Hid_LOGINID.Value);
                userM.DB_Option_Action = WebKeys.DeleteAction;
                //删除分管机构
                userSrv.DeleteFgjgbm(userM.LOGINID);
                //delete
                userSrv.Execute(userM);
            }
            clearText();
            BindDanweiddl();
            BindListView();
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            if (currentModule != null)
            {
                string urlPrefix = "~/";
                if (!string.IsNullOrEmpty(currentModule.WEBURL.Trim()))
                {
                    Response.Redirect(urlPrefix + currentModule.WEBURL.TrimStart(urlPrefix.ToCharArray()));
                }
                else
                {
                    Response.Redirect(urlPrefix + currentModule.MURL.TrimStart(urlPrefix.ToCharArray()));
                }
            }
        }

        #endregion



        #region 私有方法

        /// <summary>
        /// 清空输入框内容
        /// </summary>
        private void clearText()
        {
            Ddl_DUTY.ClearSelection();
            Chk_FGJGBM.ClearSelection();
            Txt_User_Search.Text = "";
            Txt_LOGINID.Text = "";
            Txt_PASSWORD.Text = "";
            Txt_PASSWORD.Attributes["value"] = "";
            ListView1.SelectedIndex = -1;
            Txt_USERNAME.Text = "";
            Txt_IPADDR.Text = "";
            Chk_DISABLE.Checked = false;
            Hid_LOGINID.Value = "";
            LnkBtn_Ins.Visible = true;
            LnkBtn_Upd.Visible = false;
            LnkBtn_Del.Visible = false;
            Pnl_Edit.Visible = false;
        }

        #endregion

        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindListView();
        }

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            clearText();
            Pnl_Edit.Visible = true;
            BindListView();
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindListView();
        }

    }

}
