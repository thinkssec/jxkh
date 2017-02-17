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
    /// 通知公告列表页面
    /// </summary>
    public partial class TongzhiList : PageBase
    {

        /// <summary>
        /// 通知公告服务类
        /// </summary>
        KhArticlesService articleSrv = new KhArticlesService();

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
            List<KhArticlesModel> list = null;
            if (!string.IsNullOrEmpty(Txt_Tzbt_Search.Text))
            {
                list = articleSrv.GetTzListByYear(Ddl_Niandu.SelectedValue).Where(p => p.TZBT.Contains(Txt_Tzbt_Search.Text)).ToList();
            }
            else
            {
                list = articleSrv.GetTzListByYear(Ddl_Niandu.SelectedValue) as List<KhArticlesModel>;
            }
            GridView1.DataSource = list;
            GridView1.DataBind();
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {

            //年度
            Ddl_Niandu.Items.Clear();
            for (int i = DateTime.Now.Year + 1; i >= 2014; i--)
            {
                Ddl_Niandu.Items.Add(new ListItem(i + "年", i.ToString()));
            }
            Ddl_Niandu.Items.Insert(0,new ListItem("", ""));

        }

        /// <summary>
        /// 初始化清空
        /// </summary>
        protected void Clear()
        {

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
                KhArticlesModel model = e.Row.DataItem as KhArticlesModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                //标题
                e.Row.Cells[1].Text = string.Format("<a href='/M.K.Tzview?TZID={0}' target='_self'>{1}</a>", model.TZID, model.TZBT);
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
            string tzid = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "bianji":
                    //跳转
                    Response.Redirect("~/Module/Kh/TongzhiEdit.aspx?TZID=" + tzid);
                    Response.End();
                    break;
                case "shanchu":
                    articleSrv.DeleteArticleById(tzid);
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
            Response.Redirect("~/Module/Kh/TongzhiEdit.aspx");
            Response.End();
        }

        /// <summary>
        /// 年度切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Niandu_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}