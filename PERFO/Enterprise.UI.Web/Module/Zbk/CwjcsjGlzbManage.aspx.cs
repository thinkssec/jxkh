using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Text;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{
    /// <summary>
    /// 财务基础数据与关联指标对应页面
    /// </summary>
    public partial class CwjcsjGlzbManage : PageBase
    {

        /// <summary>
        /// 基础数据与关联指标对应服务类
        /// </summary>
        ZbkCwjcsjglzbService cwjcsjGlzbSrv = new ZbkCwjcsjglzbService();
        KhCwjcsjService cwjcsjSrv = new KhCwjcsjService();//财务基础数据服务类

        #region 权限检查
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            base.PermissionHandler += Page_PermissionHandler;
        }

        void Page_PermissionHandler(PageBase.PermissionEventArgs e)
        {
            if (e.Model != null)
            {
                Btn_Add.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                LnkBtn_Upd.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                bool isDelete = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isDelete;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearText();
                BindDDL();
                BindGrid();
            }
        }

        #region DDL

        protected void BindDDL()
        {
            //财务基础数据项
            Ddl_CWZB_Search.Items.Clear();
            Ddl_JCSJZB.Items.Clear();
            foreach (string zb in KhCwjcsjService.JcsjZbs)
            {
                Ddl_CWZB_Search.Items.Add(new ListItem(zb, zb));
                Ddl_JCSJZB.Items.Add(new ListItem(zb, zb));
            }
            Ddl_CWZB_Search.Items.Insert(0, new ListItem("", ""));
            Ddl_JCSJZB.Items.Insert(0, new ListItem("", ""));
        }

        #endregion

        #region 绑定表格
        protected void BindGrid()
        {
            List<ZbkCwjcsjglzbModel> list = null;
            if (!string.IsNullOrEmpty(Ddl_CWZB_Search.SelectedValue))
            {
                list = cwjcsjGlzbSrv.GetList().Where(p => p.JCSJZB == Ddl_CWZB_Search.SelectedValue).ToList();
            }
            else if (!string.IsNullOrEmpty(Txt_ZBXMC_Search.Text))
            {
                list = cwjcsjGlzbSrv.GetList().Where(p => p.ZBXMC.Contains(Txt_ZBXMC_Search.Text)).ToList();
            }
            else
            {
                list = cwjcsjGlzbSrv.GetList() as List<ZbkCwjcsjglzbModel>;
            }
            GridView1.DataSource = list;
            GridView1.DataBind();
        }
        #endregion

        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkCwjcsjglzbModel dataModel = e.Row.DataItem as ZbkCwjcsjglzbModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
               
                //数据类型 3
                string lx = string.Empty;
                if (dataModel.JCSJLX == "1")
                    lx = "当年数据";
                else if (dataModel.JCSJLX == "0")
                    lx = "上一年数据";
                e.Row.Cells[3].Text = lx;
            }
        }
        #endregion


        #region 操作事件

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            clearText();
            ZbkCwjcsjglzbModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = cwjcsjGlzbSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null) 
                    {
                        CommonTool.SetModelDataToForm(model, Page, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, Page, "Ddl_", true);
                        Hid_ID.Value = model.ID;
                        Lbl_ZBXMC.Text = model.ZBXMC;
                    }
                    SetCntrlVisibility(LnkBtn_Upd, true);
                    SetCntrlVisibility(LnkBtn_Del, false);
                    Pnl_Edit.Visible = true;
                    break;
                case "shanchu":
                    model = cwjcsjGlzbSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        model.DB_Option_Action = WebKeys.DeleteAction;
                        cwjcsjGlzbSrv.Execute(model);
                    }
                    Pnl_Edit.Visible = false;
                    BindGrid();
                    break;
            }
        }

        //页面翻页
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        #endregion


        #region 按钮事件处理

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_ID.Value))
            {
                ZbkCwjcsjglzbModel model = (ZbkCwjcsjglzbModel)CommonTool.GetFormDataToModel(typeof(ZbkCwjcsjglzbModel), Page);
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.ID = Hid_ID.Value;
                model.ZBXMC = Lbl_ZBXMC.Text;
                cwjcsjGlzbSrv.Execute(model);
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
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            GobackPageUrl("");
        }

        #endregion


        #region 私有方法

        /// <summary>
        /// 清空输入框内容
        /// </summary>
        private void clearText()
        {
            Hid_ID.Value = "";
            Ddl_JCSJZB.ClearSelection();
            Lbl_ZBXMC.Text = Txt_BY1.Text = Txt_BY2.Text = "";
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;
        }

        #endregion

        /// <summary>
        /// 显示添加面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            //数据初始化
            cwjcsjGlzbSrv.InitCwjcsjGlzbData();
            BindGrid();
        }

        /// <summary>
        /// 指标查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

        /// <summary>
        /// 财务数据项选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_CWZB_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

    }
}