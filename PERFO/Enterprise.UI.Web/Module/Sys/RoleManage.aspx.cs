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
    /// 角色管理页面
    /// </summary>
    public partial class RoleManage : PageBase
    {

        /// <summary>
        /// 角色服务类
        /// </summary>
        SysRoleService roleSrv = new SysRoleService();

        /// <summary>
        /// 数据集合
        /// </summary>
        List<SysRoleModel> roleList = new List<SysRoleModel>();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearText();
                BindGrid();
                BindPicList(".jpg");
            }
        }

       
        /// <summary>
        /// 绑定角色图片
        /// </summary>
        /// <param name="ext"></param>
        protected void BindPicList(string ext)
        {
            string path = Server.MapPath(@"/Resources/Images/role/");
            IEnumerable<FileInfo> filelist = FileControl.GetFiles(path);
            var files = from file in filelist
                        where file.Extension == ext
                        select file.Name;
            foreach (var f in files)
            {
                ListItem list = new ListItem();
                list.Text = f.ToString();
                list.Value = f.ToString();
                Ddl_ROLEPICTURE.Items.Add(list);
            }
            Ddl_ROLEPICTURE.SelectedValue = "error.gif";
            txtimgshow.ImageUrl = @"/Resources/Images/role/error.gif";
        }


        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid()
        {
            roleList = roleSrv.GetList().ToList();
            GridView1.DataSource = roleList;
            GridView1.DataBind();
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btn = (ImageButton)e.Row.Cells[0].Controls[1];
                btn.ImageUrl = e.Row.Cells[2].Text;
            }
        }


        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }


        protected void Txtpicddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtimgshow.ImageUrl = @"/Resources/Images/role/" + Ddl_ROLEPICTURE.SelectedValue;
        }

        /// <summary>
        /// 选择记录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectId = GridView1.SelectedRow.Cells[3].Text;
            SysRoleModel zhcnModel = roleSrv.GetSingle(selectId);
            if (zhcnModel != null)
            {
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Txt_", true);
                CommonTool.SetModelDataToForm(zhcnModel, Page, "Ddl_", true);
                Chk_ROLEDISABLE.Checked = (zhcnModel.ROLEDISABLE != "0") ? true : false;
                txtimgshow.ImageUrl = @"/Resources/Images/role/" + zhcnModel.ROLEPICTURE;
            }

            //角色编码不可修改
            Pnl_Edit.Visible = true;
            Txt_ROLEID.Enabled = false;
            LnkBtn_Ins.Visible = false;
            LnkBtn_Upd.Visible = true;
            LnkBtn_Del.Visible = true;
        }


        #region 按钮事件处理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            //中文
            SysRoleModel zhcnModel = (SysRoleModel)CommonTool.GetFormDataToModel(typeof(SysRoleModel), Page);
            zhcnModel.DB_Option_Action = WebKeys.InsertAction;
            zhcnModel.ROLEDISABLE = (Chk_ROLEDISABLE.Checked) ? "1" : "0";
            //add
            roleSrv.Execute(zhcnModel);

            clearText();
            BindGrid();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_ROLEID.Text))
            {
                //中文
                SysRoleModel zhcnModel = (SysRoleModel)CommonTool.GetFormDataToModel(typeof(SysRoleModel), Page);
                zhcnModel.DB_Option_Action = WebKeys.UpdateAction;
                zhcnModel.ROLEDISABLE = (Chk_ROLEDISABLE.Checked) ? "1" : "0";
                //update
                roleSrv.Execute(zhcnModel);
            }

            clearText();
            BindGrid();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_ROLEID.Text))
            {
                SysRoleModel zhcnModel = new SysRoleModel();
                zhcnModel.ROLEID = Txt_ROLEID.Text;
                zhcnModel.DB_Option_Action = WebKeys.DeleteAction;
                roleSrv.Execute(zhcnModel);
            }
            clearText();
            BindGrid();
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
            Txt_ROLENAME.Text = "";
            Txt_ROLEID.Text = "";
            Txt_ROLEID.Enabled = true;
            Chk_ROLEDISABLE.Checked = false;
            LnkBtn_Ins.Visible = true;
            LnkBtn_Upd.Visible = false;
            LnkBtn_Del.Visible = false;
            Pnl_Edit.Visible = false;
        }

        #endregion

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            clearText();
            Pnl_Edit.Visible = true;
            BindGrid();
        }

    }
}
