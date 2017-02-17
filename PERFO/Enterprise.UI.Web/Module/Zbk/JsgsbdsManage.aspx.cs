using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{

    /// <summary>
    /// 计算规则配置页面
    /// </summary>
    public partial class JsgsbdsManage : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        ZbkJsgzService jsgzSrv = new ZbkJsgzService();

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
                LnkBtn_Ins.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                LnkBtn_Upd.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
                bool isDelete = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Del);
                GridView2.Columns[GridView2.Columns.Count - 1].Visible = isDelete;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearText();
                BindDdl();
                BindGrid();
            }
        }

        protected void BindDdl()
        {
            ZbkBanbenService banbenSrv = new ZbkBanbenService();
            Ddl_BBMC.DataSource = banbenSrv.GetList();
            Ddl_BBMC.DataTextField = "BBMC";
            Ddl_BBMC.DataValueField = "BBMC";
            Ddl_BBMC.DataBind();

            Ddl_BBMC_Search.DataSource = banbenSrv.GetList();
            Ddl_BBMC_Search.DataTextField = "BBMC";
            Ddl_BBMC_Search.DataValueField = "BBMC";
            Ddl_BBMC_Search.DataBind();
        }

        protected void BindGrid()
        {
            List<ZbkJsgzModel> list = null;
            if (!string.IsNullOrEmpty(Txt_Jsgz_Search.Text))
            {
                list = jsgzSrv.GetListByBBMC(Ddl_BBMC_Search.Text).Where(p => p.GZMC.Contains(Txt_Jsgz_Search.Text)).ToList();
            }
            else
            {
                list = jsgzSrv.GetListByBBMC(Ddl_BBMC_Search.Text) as List<ZbkJsgzModel>;
            }
            GridView2.DataSource = list;
            GridView2.DataBind();
        }
        
        #region 操作事件 编辑/改变状态/删除

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ZbkJsgzModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = jsgzSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        CommonTool.SetModelDataToForm(model, Page, "Txt_", true);
                        CommonTool.SetModelDataToForm(model, Page, "Ddl_", true);
                        Txt_BDS.Text = model.GZBDS;
                        Hid_GZID.Value = model.GZID;
                    }
                    Pnl_Edit.Visible = true;
                    SetCntrlVisibility(LnkBtn_Ins, false);
                    SetCntrlVisibility(LnkBtn_Upd, true);
                    break;
                case "shanchu":
                    model = new ZbkJsgzModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.GZID = e.CommandArgument.ToString();
                    jsgzSrv.Execute(model);
                    BindGrid();
                    break;
                default:
                    break;
            }
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
            ZbkJsgzModel model = (ZbkJsgzModel)CommonTool.GetFormDataToModel(typeof(ZbkJsgzModel), Page);
            model.GZID = "GZ" + CommonTool.GetPkId();
            model.DB_Option_Action = WebKeys.InsertAction;
            model.GZBDS = Server.UrlDecode(Hid_GZBDS.Value);
            //add
            jsgzSrv.Execute(model);

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
            if (!string.IsNullOrEmpty(Hid_GZID.Value))
            {
                ZbkJsgzModel model = (ZbkJsgzModel)CommonTool.GetFormDataToModel(typeof(ZbkJsgzModel), Page);
                model.GZID = Hid_GZID.Value;
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.GZBDS = Server.UrlDecode(Hid_GZBDS.Value);
                //update
                jsgzSrv.Execute(model);
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
            //Txt_Jsgz_Search.Text = "";
            Txt_METHODNAME.Text = "";
            Txt_MINV.Text = "";
            Txt_MAXV.Text = "";
            Txt_GZMC.Text = "";
            Txt_UPPER.Text = "";
            Txt_LOWER.Text = "";
            Txt_BDS.Text = "";
            Hid_GZID.Value = "";
            Hid_GZBDS.Value = "";
            SetCntrlVisibility(LnkBtn_Ins, true);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            Pnl_Edit.Visible = false;
        }

        #endregion


        #region 隔行换色
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkJsgzModel model = e.Row.DataItem as ZbkJsgzModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //表达式 3
                e.Row.Cells[3].Text = "<div style='width: 300px;padding: 2px;overflow:auto;'>" + Server.HtmlEncode(model.GZBDS) + "</div>";
            }
        }
        #endregion

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            SetCntrlVisibility(LnkBtn_Ins, true);
            SetCntrlVisibility(LnkBtn_Upd, false);
            SetCntrlVisibility(LnkBtn_Del, false);
            BindGrid();
        }

        /// <summary>
        /// 分页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        /// <summary>
        /// 版本选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_BBMC_Search_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

    }
}