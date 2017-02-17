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
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{
    /// <summary>
    /// 考核制度汇编维护页面
    /// </summary>
    public partial class KaoheZdhbList : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhKindhbService hbSrv = new KhKindhbService();

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
                bool isInsert = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Ins);
                Btn_Add.Visible = isInsert;
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isInsert;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            List<KhKindhbModel> list = null;
            if (!string.IsNullOrEmpty(Txt_Wjmc_Search.Text))
            {
                list = hbSrv.GetList().Where(p => p.WJMC.Contains(Txt_Wjmc_Search.Text)).ToList();
            }
            else
            {
                list = hbSrv.GetList() as List<KhKindhbModel>;
            }
            GridView1.DataSource = list;
            GridView1.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核类型
            KhKindService kindSrv = new KhKindService();
            var kindList = kindSrv.GetList();
            Ddl_LXID.DataSource = kindList;
            Ddl_LXID.DataTextField = "LXMC";
            Ddl_LXID.DataValueField = "LXID";
            Ddl_LXID.DataBind();
        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {
            Hid_WJID.Value = "";
            Txt_WJMC.Text = "";
            Txt_WJFJ.Text = Txt_WJFJ.Value = "";
            Txt_ZXLL.Text = Txt_ZXLL.Value = "";
            Ddl_LXID.ClearSelection();
            Pnl_Edit.Visible = false;
        }

        #endregion

        #region 事件处理区

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhKindhbModel model = e.Row.DataItem as KhKindhbModel;
                //考核文件名称1
                e.Row.Cells[1].Text =
                    string.Format("<a href=\"/C/F/PdfView?fn={0}\" target=\"_blank\">{1}</a>",
                    ((!string.IsNullOrEmpty(model.ZXLL)) ? Base64.Base64Encode(model.ZXLL) : ""), model.WJMC);
                //考核类型2
                e.Row.Cells[2].Text = model.Kind.LXMC;
                //文件下载3
                e.Row.Cells[3].Text = model.WJFJ.ToAttachHtmlByOne();
                //在线浏览4
                e.Row.Cells[4].Text =
                    string.Format("<a href=\"/C/F/PdfView?fn={0}\" target=\"_blank\">{1}</a>",
                    ((!string.IsNullOrEmpty(model.ZXLL)) ? Base64.Base64Encode(model.ZXLL) : ""), "〖浏览〗");
            }
        }

        /// <summary>
        /// 换页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;;
            BindGrid();
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var model = hbSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        Hid_WJID.Value = model.WJID.ToString();
                        Txt_WJMC.Text = model.WJMC;
                        Txt_WJFJ.Text = Txt_WJFJ.Value = model.WJFJ;
                        Txt_ZXLL.Text = Txt_ZXLL.Value = model.ZXLL;
                        Ddl_LXID.SelectedValue = model.LXID;
                        Pnl_Edit.Visible = true;
                    }
                    break;
                case "shanchu":
                    if (model != null)
                    {
                        model.DB_Option_Action = WebKeys.DeleteAction;
                        hbSrv.Execute(model);
                    }
                    BindGrid();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 显示编辑面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

        #endregion

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Ins_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_WJID.Value))
            {
                var model = hbSrv.GetSingle(Hid_WJID.Value);
                if (model != null)
                {
                    model.DB_Option_Action = WebKeys.UpdateAction;
                    model.LXID = Ddl_LXID.SelectedValue;
                    model.WJMC = Txt_WJMC.Text;
                    model.WJZT = "1";
                    model.ZXLL = Txt_ZXLL.Value;
                    model.WJFJ = Txt_WJFJ.Value;
                    model.TJRQ = DateTime.Now;
                    hbSrv.Execute(model);
                }
            }
            else
            {
                KhKindhbModel model = new KhKindhbModel();
                model.DB_Option_Action = WebKeys.InsertAction;
                model.LXID = Ddl_LXID.SelectedValue;
                model.WJMC = Txt_WJMC.Text;
                model.WJZT = "1";
                model.ZXLL = Txt_ZXLL.Value;
                model.WJFJ = Txt_WJFJ.Value;
                model.TJRQ = DateTime.Now;
                hbSrv.Execute(model);
            }
            Utility.ShowMsg(Page, "提示", "考核制度发布成功!", 100, "show");
            Clear();
            BindGrid();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = false;
            BindGrid();
        }

    }
}