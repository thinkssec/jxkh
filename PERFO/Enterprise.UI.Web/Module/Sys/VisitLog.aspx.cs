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
using Enterprise.Model.Perfo.Sys;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Sys
{
    /// <summary>
    /// 访问日志管理页面
    /// </summary>
    public partial class VisitLog : PageBase
    {
        
        /// <summary>
        /// 访问日志服务类
        /// </summary>
        SysVisitlogService visitSrv = new SysVisitlogService();

        #region 权限检查

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
           
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearPreviousLog();
                RQ_Start.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Year, CommonTool.BuZero_2(DateTime.Now.Month), "01");
                RQ_End.Text = string.Format("{0}-{1}-{2}", DateTime.Now.Year, CommonTool.BuZero_2(DateTime.Now.Month), CommonTool.GetMonthLastDay(DateTime.Now).Day);
                BindGrid();
            }
        }

        /// <summary>
        /// 删除早先的数据，只保留90天
        /// </summary>
        private void ClearPreviousLog()
        {
            DateTime dt = DateTime.Now.AddDays(-90);
            string sql = "delete from PERFO_SYS_VISITLOG where OPERATIONDATE <= to_date('" + dt.ToDateYMDFormat() + "','yyyy-mm-dd')";
            visitSrv.ExecuteSQL(sql);
        }

        #region 绑定表格

        protected void BindGrid()
        {
            string hql = "from SysVisitlogModel p where p.OPERATIONDATE >= cast('" + RQ_Start.Text + "' as datetime) and p.OPERATIONDATE <= cast('" + RQ_End.Text + " 23:59:59' as datetime)";
            GridView1.DataSource = visitSrv.GetListByHQL(hql);
            GridView1.DataBind();
        }

        #endregion


        #region 事件处理

        /// <summary>
        /// 行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //序号
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();

                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }

        /// <summary>
        /// 行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "shanchu":
                    SysVisitlogModel model = new SysVisitlogModel();
                    model.DB_Option_Action = WebKeys.DeleteAction;
                    model.ID = e.CommandArgument.ToString();
                    visitSrv.Execute(model);
                    BindGrid();
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region 按钮事件处理

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Search_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

        /// <summary>
        /// 生成访问日志的线型图所需数据
        /// egg:['1-May-14', 1], ['2-May-14', 4], ['3-May-14', 12];
        /// </summary>
        /// <returns></returns>
        protected string GetLineData()
        {
            StringBuilder lineData = new StringBuilder();
            DateTime rqStart = RQ_Start.Text.ToDateTime();
            DateTime rqEnd = rqStart.AddMonths(1);
            string hql = "from SysVisitlogModel p where p.OPERATIONDATE >= cast('" + rqStart.ToDateYMDFormat() + "' as datetime) and p.OPERATIONDATE <= cast('" + rqEnd.ToDateYMDFormat() + " 23:59:59' as datetime)";
            List<SysVisitlogModel> visitLst = visitSrv.GetListByHQL(hql).ToList();
            if (visitLst==null) visitLst = new List<SysVisitlogModel>(); 
            for (int i = 0; i < 30; i++)
            {
                rqEnd = rqStart.AddDays(i);
                int visitCountForDay = visitLst.Count(p=>p.OPERATIONDATE.ToDateYMDFormat() == rqEnd.ToDateYMDFormat());
                lineData.Append(string.Format("['{0}-{1}-{2}', {3}],", rqEnd.Day,
                    CommonTool.GetMonthNameAbbr(rqEnd.Month), rqEnd.Year.ToString().Substring(2, 2), visitCountForDay));
            }
            return lineData.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 分页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;//分页BindData();
            BindGrid();
        }

    }
}