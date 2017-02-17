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
    /// 机关部门考核结果显示页面
    /// </summary>
    public partial class CxtjJgbmKhjg : PageBase
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
                !string.IsNullOrEmpty(JgbmkhdfList.First().HZBZ))
            {
                Lbl_Msg.Text = "本期考核结果已发布了!";

                //机关部门
                GridView1.DataSource = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString())
                    .DistinctBy(p => p.JGBM).OrderBy(p => p.BMPM).ThenBy(p => p.Bmjg.BZ).ToList();
                GridView1.DataBind();
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
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhJgbmkhdfModel model = e.Row.DataItem as KhJgbmkhdfModel;
                //鼠标移动到某行上，该行变色
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                //鼠标移开后，恢复
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");

                //处室名称1
                e.Row.Cells[1].Text = string.Format(
                    "<a href='/M.K.CxtjJgbmMx?KH={0}&BM={1}'>{2}</a>", model.KHID, model.JGBM, model.Bmjg.JGMC); 
                
                //重点工作完成情况得分2
                var zdgz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("管理类考核指标"));
                if (zdgz != null)
                {
                    e.Row.Cells[2].Text = zdgz.KHDF.ToRequestString();
                }

                //部门履职考核得分3
                var bmlz = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("专项考核指标"));
                if (bmlz != null)
                {
                    e.Row.Cells[3].Text = bmlz.KHDF.ToRequestString();
                }

                //机关作风考核得分4
                //机关作风建设加分5
                var jgzfjx = JgbmkhdfList.Find(p => p.JGBM == model.JGBM && p.KHXMC.Contains("机关作风"));
                if (jgzfjx != null)
                {
                    e.Row.Cells[4].Text = string.Format("<a href='/M.K.JgbmJgzfHz?KH={0}'>{1}</a>", model.KHID, jgzfjx.BZSM); 
                    e.Row.Cells[5].Text = jgzfjx.KHDF.ToRequestString();
                }

                //最终考核得分6
                e.Row.Cells[6].Text = model.BMZDF.ToRequestString();
                //排名 7
                e.Row.Cells[7].Text = model.BMPM.ToRequestString();
                //考核兑现系数 8
                e.Row.Cells[8].Text = model.BMDXBS.ToRequestString();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "平均分数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].ColumnSpan = 4;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                var bmdf = JgbmkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString()).First();
                //.DistinctBy(p => p.JGBM).ToList();
                e.Row.Cells[6].Text = bmdf.BMPJF.ToRequestString();//.Average(p=>p.BMZDF).ToRequestString();
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
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
            GridView1.HeaderStyle.Height = Unit.Pixel(50);
            GridView1.RowStyle.Height = Unit.Pixel(30);
            GridView1.FooterStyle.Height = Unit.Pixel(30);
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
            int count = GridView1.Columns.Count;
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
            this.GridView1.RenderControl(htw);
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