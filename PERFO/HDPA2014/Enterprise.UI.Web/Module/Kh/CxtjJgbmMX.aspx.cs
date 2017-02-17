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
using Enterprise.Model.Perfo;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///机关部门考核明细表显示页面
    /// </summary>
    public partial class CxtjJgbmMX : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        ///// <summary>
        ///// 打分指标明细-服务类
        ///// </summary>
        //KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected List<KhDlzbmxModel> DlzbmxList = null;//定量指标明细集合
        protected decimal HjDefen = 0M;//合计得分

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
                LnkBtn_Report.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Rpt);
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
            //1==考核指标
            DlzbmxList = dlzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            ////2==打分指标
            //List<KhDfzbmxModel> dfzbmxList = dfzbmxSrv.GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue)
            //    .Where(p => p.DfzbModel.Zbxx.YJZBMC.Contains("机关作风建设")).ToList();
            //合成数据集
            List<PerfoSuperModel> dataList = new List<PerfoSuperModel>();
            dataList.AddRange(DlzbmxList);
            //dataList.AddRange(dfzbmxList);
            
            if (DlzbmxList.Count > 0)
            {
                if (DlzbmxList.Count(p => p.WCZSDRQ == null) == 0)
                {
                    Lbl_Msg.Text = "考核结果已发布!";

                    GridView1.DataSource = dataList;
                    GridView1.DataBind();
                    Utility.GroupRows(GridView1, 1);
                }
                else
                {
                    Lbl_Msg.Text = "考核结果还未发布!";
                }
            }
        }

        /// <summary>
        /// 绑定条件
        /// </summary>
        protected void BindDdl()
        {
            //单位
            List<SysBmjgModel> parentBmjgLst = bmjgService.GetSameLevelBmjg(4) as List<SysBmjgModel>;
            int[] jgbms = (from c in parentBmjgLst select c.JGBM).ToArray();
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).
                Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("二级")).ToList();
            bmjgService.BindSSECDropDownListForBmjg(Ddl_Danwei, bmjgTreeList, jgbms);
            if (Jgbm > 0)
            {
                Ddl_Danwei.SelectedValue = Jgbm.ToString();
            }
            else
            {
                Ddl_Danwei.SelectedValue = userModel.JGBM.ToString();
            }

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

                PerfoSuperModel supperM = e.Row.DataItem as PerfoSuperModel;
                if (supperM.GetType().IsAssignableFrom(typeof(KhDlzbmxModel)))
                {
                    KhDlzbmxModel model = supperM as KhDlzbmxModel;
                    //指标类别 1
                    e.Row.Cells[1].Text = model.LhzbModel.Zbxx.ZBMC;//以名称为类别
                    //其考核权重直接引用指标筛选时的权重
                    e.Row.Cells[1].Text += "<br/>〖" + Convert.ToDecimal(model.ZbsxModel.SXQZ * 100).ToString("f1") + "%〗";
                    //考核主要内容 2
                    e.Row.Cells[2].Text = "<div style='width: 150px;padding: 2px;overflow:auto;'>"
                        + model.ZbsxModel.JxzrsZb.JGKHNR + "</div>";
                    //分值3
                    e.Row.Cells[3].Text = model.ZbsxModel.SXFZ.ToRequestString();
                    //考核目标4
                    e.Row.Cells[4].Text =
                        "<div style='width: 200px;padding: 2px;overflow:auto;cursor:hand;text-decoration:underline;' onclick=\"showInfo('评分标准','"
                        + model.ZbsxModel.JxzrsZb.JGWCSJ + "<hr/>" + model.ZbsxModel.JxzrsZb.JGPFBZ + "');\">"
                        + model.ZbsxModel.JxzrsZb.JGKHMB + "</div>";
                    //得分5
                    e.Row.Cells[5].Text = model.SJDF.ToRequestString();
                    //合计
                    HjDefen += model.SJDF.ToDecimal();
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 5;
                e.Row.Cells[0].Text = "∑合计得分=";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Text = string.Format("<font color='red'>{0}</font>", HjDefen);
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 行命令处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        /// <summary>
        /// 考核期选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Kaohe_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 单位选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_Danwei_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

        #region 数据导出

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Report_Click(object sender, EventArgs e)
        {
            GridView1.HeaderStyle.Height = Unit.Pixel(50);
            GridView1.RowStyle.Height = Unit.Pixel(30);
            GridView1.FooterStyle.Height = Unit.Pixel(30);
            string title = Ddl_Kaohe.SelectedItem.Text + "(" + Ddl_Danwei.SelectedItem.Text + ")" +"考核明细表";
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

        #region 专用方法区

        #endregion

    }
}