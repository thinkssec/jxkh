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
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Zbk
{

    /// <summary>
    /// 指标版本管理页面
    /// </summary>
    public partial class ZbbbManage : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        ZbkBanbenService banbenSrv = new ZbkBanbenService();
        /// <summary>
        /// 考核服务类
        /// </summary>
        KhKhglService kaoheSrv = new KhKhglService();

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
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = isDelete;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #region 绑定表格

        /// <summary>
        /// 绑定表格
        /// </summary>
        protected void BindGrid()
        {
            List<ZbkBanbenModel> zbbbList = null;
            //指标名称查询
            if (!string.IsNullOrEmpty(Txt_ZBBB_Search.Text))
            {
                zbbbList = banbenSrv.GetList().Where(p => p.BBMC.Contains(Txt_ZBBB_Search.Text)).ToList();
            }
            else
            {
                zbbbList = banbenSrv.GetList().ToList();
            }
            if (zbbbList != null)
            {
                GridView1.DataSource = zbbbList;
                GridView1.DataBind();
            }
        }
        #endregion


        #region 隔行换色
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ZbkBanbenModel model = e.Row.DataItem as ZbkBanbenModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                ////序号
                //e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

                //提取历次考核的信息
                Label lbl = e.Row.FindControl("Label1") as Label;
                lbl.Text = getHistoryKaohe(model.BBMC);
            }
        }
        #endregion


        /// <summary>
        /// 获取查询版本的历次考核信息
        /// </summary>
        /// <param name="zbbb"></param>
        /// <returns></returns>
        private string getHistoryKaohe(string zbbb)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
            List<KhKhglModel> khList = kaoheSrv.GetKhListByZbbb(zbbb) as List<KhKhglModel>;
            foreach (var q in khList)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + q.KHMC + "</td>");
                sb.Append("<td>" + q.KSSJ.ToDateYMDFormat() + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }


        #region 保存操作

        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e)
        {
            ZbkBanbenModel model = banbenSrv.GetSingle(Txt_BBMC.Text);
            if (model != null)
            {
                model.DB_Option_Action = WebKeys.UpdateAction;
                model.BBMC = Txt_BBMC.Text;
                if (Chk_QYSJ.Checked)
                {
                    model.QYSJ = DateTime.Now;
                }
                else
                {
                    model.QYSJ = null;
                }
            }
            else
            {
                model = new ZbkBanbenModel();
                model.DB_Option_Action = WebKeys.InsertAction;
                model.BBMC = Txt_BBMC.Text;
                if (Chk_QYSJ.Checked)
                {
                    model.QYSJ = DateTime.Now;
                }
                else
                {
                    model.QYSJ = null;
                }
            }
            //submit
            banbenSrv.Execute(model);
            clearText();
            BindGrid();
        }

        /// <summary>
        /// 清空内容
        /// </summary>
        private void clearText()
        {
            Txt_BBMC.Text = "";
            Txt_ZBBB_Search.Text = "";
            Pnl_Edit.Visible = false;
        }

        #endregion


        #region 操作事件 编辑/改变状态/删除
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ZbkBanbenModel model = null;
            switch (e.CommandName)
            {
                case "bianji":
                    model = banbenSrv.GetSingle(e.CommandArgument.ToString());
                    if (model != null)
                    {
                        Txt_BBMC.Text = model.BBMC;
                        Chk_QYSJ.Checked = (model.QYSJ == null) ? false : true;
                        Pnl_Edit.Visible = true;
                    }
                    break;
                case "shanchu":
                    model = new ZbkBanbenModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.BBMC = e.CommandArgument.ToString();
                    banbenSrv.Execute(model);
                    BindGrid();
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
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


        //页面翻页
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

        /// <summary>
        /// 单位查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Find_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 显示添加界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Pnl_Edit.Visible = true;
            BindGrid();
        }

    }
}