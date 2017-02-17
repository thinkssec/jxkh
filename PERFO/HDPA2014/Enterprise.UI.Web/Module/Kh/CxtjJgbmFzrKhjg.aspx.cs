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
using System.Text;
using System.IO;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    /// 机关部门负责人考核结果显示页面
    /// </summary>
    public partial class CxtjJgbmFzrKhjg : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();//考核得分
        KhKhglService khglSrv = new KhKhglService();//考核管理
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhJgbmkhdfModel> JgbmkhdfList = null;//考核得分数据集

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
               LnkBtn_Report.Visible
                   = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Rpt);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDdl();
                BindGrid();
                TabTitle = "机关部门";
            }
        }

        #region 专用方法区

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindGrid() 
        {
            //提取数据集
            JgbmkhdfList = jgbmkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhJgbmkhdfModel>;
            if (JgbmkhdfList.Count > 0 &&
                !string.IsNullOrEmpty(JgbmkhdfList.First().HZBZ) && JgbmkhdfList.First().HZBZ == "2")
            {
                Lbl_Msg.Text = "本期考核结果已发布了!";

                //负责人
                GridView2.DataSource = JgbmkhdfList.DistinctBy(p => p.JGBM).OrderBy(p => p.FZRPM).
                    ThenBy(p => p.Bmjg.BZ).ToList();
                GridView2.DataBind();
            }
            else
            {
                Lbl_Msg.Text = "未查询到相应的考核结果!";
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //考核期
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014B" && p.SFKC == "1").ToList();
            khglSrv.BindSSECDropDownListForKaohe(Ddl_Kaohe, kaohe);
            if (!string.IsNullOrEmpty(Khid))
            {
                Ddl_Kaohe.SelectedValue = Khid;
            }
            else if (kaohe.Count > 0)
            {
                Ddl_Kaohe.SelectedValue = kaohe.First().KHID.ToString();//最近一次考核
            }

            Lbl_Msg.Text = "";
        }

        #endregion


        #region 事件处理区

        /// <summary>
        /// 负责人行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJgbmkhdfModel model = e.Row.DataItem as KhJgbmkhdfModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //处室名称1
                e.Row.Cells[1].Text = SysBmjgService.GetBmjgName(model.JGBM);

                //重点工作完成情况得分2
                var zdgz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("重点"));
                if (zdgz != null)
                {
                    e.Row.Cells[2].Text = zdgz.KHDF.ToRequestString();
                }

                //部门履职考核得分3
                var bmlz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("部门履职"));
                if (bmlz != null)
                {
                    e.Row.Cells[3].Text = bmlz.KHDF.ToRequestString();
                }

                //机关作风建设加分4
                var jgzfjx = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("机关作风"));
                if (jgzfjx != null)
                {
                    e.Row.Cells[4].Text = string.Format("<a href='/M.K.JgbmJgzfHz?KH={0}'>{1}</a>", model.KHID, jgzfjx.KHDF);
                }

                //费用控制情况 5
                var fykzqk = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("费用控制"));
                if (fykzqk != null)
                {
                    e.Row.Cells[5].Text = fykzqk.KHDF.ToRequestString();
                }

                //部门得分6
                e.Row.Cells[6].Text = model.BMZDF.ToRequestString();
                //连带指标得分7
                //原因说明8
                var ldzb = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("连带"));
                if (ldzb != null)
                {
                    e.Row.Cells[7].Text = ldzb.KHDF.ToRequestString();
                    e.Row.Cells[8].Text = "<div style='width: 120px;padding: 2px;overflow:auto;'>" + ldzb.BZSM + "</div>";
                }

                //最终结果9
                e.Row.Cells[9].Text = model.FZRZDF.ToRequestString();
                //排名10
                e.Row.Cells[10].Text = model.FZRPM.ToRequestString();
                //兑现倍数11
                e.Row.Cells[11].Text = model.FZRDXBS.ToRequestString();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "平均分数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ColumnSpan = 9;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                var fzrdf = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString()).First();
                //.DistinctBy(p => p.JGBM).ToList();
                e.Row.Cells[9].Text = fzrdf.FZRPJF.ToRequestString();//.Average(p=>p.BMZDF).ToRequestString();
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 考核期切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion


        #region 导出数据

        /// <summary>
        /// 导出XLS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Report_Click(object sender, EventArgs e)
        {
            GridView2.HeaderStyle.Height = Unit.Pixel(50);
            GridView2.RowStyle.Height = Unit.Pixel(30);
            GridView2.FooterStyle.Height = Unit.Pixel(30);
            string title = Ddl_Kaohe.SelectedItem.Text + "考核结果";
            toExcel(title);
        }

        /// <summary>
        /// 转存为EXCLE
        /// </summary>
        /// <param name="title"></param>
        private void toExcel(string title)
        {
            Encoding encContent = Encoding.GetEncoding("utf-8");
            string tableName = title;
            int count = GridView2.Columns.Count;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.Charset = encContent.WebName;
            Response.ContentEncoding = encContent;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(string.Format("<meta http-equiv=Content-Type content=text/html;charset={0}>", encContent.WebName));
            //string rename = HttpUtility.UrlEncode(tableName, Encoding.UTF8);
            Response.AppendHeader("content-disposition", string.Format("attachment;filename={0}.xls", "jgbm_data"));
            //设置表头样式
            StringBuilder heardStyle = new StringBuilder();
            heardStyle.Append(@"<style> .heard {font-family:Times New Roman;font-size:10.0pt;");
            heardStyle.Append("font-weight:600;color:black;");
            heardStyle.Append("text-align:center;border:.5pt solid windowtext;");
            heardStyle.Append("background:#CCCCFF;mso-pattern:auto none;} </style> ");
            //设置文本单元格样式
            StringBuilder cellStyle = new StringBuilder();
            cellStyle.Append(@"<style> .cell { font-family:Times New Roman;font-size:10.0pt;");
            cellStyle.Append("text-align:left;vertical-align:top;border:.5pt solid windowtext;");
            cellStyle.Append("white-space:normal;} </style> ");
            //设置表脚样式
            StringBuilder footerStyle = new StringBuilder();
            footerStyle.Append(@"<style> .footer {font-family:Times New Roman;font-size:10.0pt;");
            footerStyle.Append("font-weight:600;color:black;");
            footerStyle.Append("text-align:left;border:.5pt solid windowtext;");
            footerStyle.Append("background:#CCCCFF;mso-pattern:auto none;} </style> ");
            //设置表名格式
            StringBuilder titleName = new StringBuilder();
            string colspanCount = string.Format("<table><tr><td colspan=\"{0}\"", count);
            titleName.Append(colspanCount);
            titleName.Append("style=\"font-family:Times New Roman;font-size:11.0pt;font-weight:700;");
            titleName.Append("text-align:center;vertical-align:middle;height:45px;\">");
            titleName.Append(string.Format("{0}</td></tr></table>", tableName));
            EnableViewState = false;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            this.GridView2.RenderControl(htw);
            Response.Write(AddExcelHead());
            Response.Write(heardStyle.ToString());
            Response.Write(cellStyle.ToString());
            Response.Write(footerStyle.ToString());
            Response.Write(titleName.ToString());
            Response.Write(sw.ToString());
            Response.Write(AddExcelbottom());
            sw.Close();
            Response.End();
        }


        private string AddExcelHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.Append(" <head>");
            sb.Append(" <!--[if gte mso 9]><xml>");
            sb.Append("<x:ExcelWorkbook>");
            sb.Append("<x:ExcelWorksheets>");
            sb.Append("<x:ExcelWorksheet>");
            sb.Append("<x:Name></x:Name>");
            sb.Append("<x:WorksheetOptions>");
            sb.Append("<x:Print>");
            sb.Append("<x:ValidPrinterInfo />");
            sb.Append(" </x:Print>");
            sb.Append("</x:WorksheetOptions>");
            sb.Append("</x:ExcelWorksheet>");
            sb.Append("</x:ExcelWorksheets>");
            sb.Append("</x:ExcelWorkbook>");
            sb.Append("</xml>");
            sb.Append("<![endif]-->");
            sb.Append(" </head>");
            sb.Append("<body>");
            return sb.ToString();
        }

        private string AddExcelbottom()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("</body>");
            sb.Append("</html>");
            return sb.ToString();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #endregion
    }
}