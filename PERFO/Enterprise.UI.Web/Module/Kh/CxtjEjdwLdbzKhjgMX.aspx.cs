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
using Enterprise.Model.Perfo;
using System.IO;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    ///二级单位领导班子考核结果明细页面
    /// </summary>
    public partial class CxtjEjdwLdbzKhjgMX : PageBase
    {

        /// <summary>
        /// 定量指标明细-服务类
        /// </summary>
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标服务类
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//机关部门打分表
        KhKhglService khglSrv = new KhKhglService();//考核管理
        KhNdxsService ndxsSrv = new KhNdxsService();//经营难度系数

        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected int Jgbm = (int)Utility.sink("BM", Utility.MethodType.Get, 0, 0, Utility.DataType.Int);//机构编码
        protected decimal EjdwHeji = 0M;//二级单位合计得分
        protected decimal LdbzLhzbHeji = 0M;//领导班子量化指标合计得分
        protected decimal LdbzDfzbHeji = 0M;//领导班子量化指标合计得分

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
                LnkBtn_Report.Visible = 
                    Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Rpt);
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
            //定量
            List<KhDlzbmxModel> dlzbList = dlzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDlzbmxModel>;
            var ejdwDlzbLst = dlzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.二级单位).ToString());
            //打分
            List<KhDfzbmxModel> dfzbList = dfzbmxSrv.
                GetListByKhidAndJgbm(Ddl_Kaohe.SelectedValue, Ddl_Danwei.SelectedValue) as List<KhDfzbmxModel>;

            //2==领导班子考核指标
            //定量
            var ldbzDlzbLst = dlzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.二级单位).ToString());
            //打分
            var ldbzDfzbLst = dfzbList.Where(p => p.KHDX == ((int)WebKeys.KaoheType.领导班子).ToString());
            //合成数据集
            List<PerfoSuperModel> ldbzDataList = new List<PerfoSuperModel>();
            ldbzDataList.AddRange(ldbzDlzbLst);
            ldbzDataList.AddRange(ldbzDfzbLst);
            
            if (dlzbList.Count > 0 && dlzbList.Count(p => p.WCZSDRQ == null) == 0)
            {
                Lbl_Msg.Text = "考核结果已发布!";
                GridView1.Visible = true;
                GridView1.DataSource = ldbzDataList;
                GridView1.DataBind();
                Utility.GroupRows(GridView1, 1);
            }
            else
            {
                Lbl_Msg.Text = "未查询到该单位的考核结果!";
                GridView1.Visible = false;
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
            List<SysBmjgModel> bmjgTreeList = bmjgService.GetBmjgTreeLst(false).Where(p => p.XSXH.Length > 2 && !p.JGLX.Contains("职能")).ToList();
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
            var kaohe = khglSrv.GetKhListForValid().Where(p => p.LXID == "LX2014A" && p.SFKC == "1").ToList();
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
                ////鼠标移动到某行上，该行变色
                //e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#eeeeee'");
                ////鼠标移开后，恢复
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
                HiddenField hid = e.Row.FindControl("Hid_ID") as HiddenField;
                PerfoSuperModel supperM = e.Row.DataItem as PerfoSuperModel;
                if (supperM.GetType().IsAssignableFrom(typeof(KhDlzbmxModel)))
                {
                    //定量指标
                    KhDlzbmxModel model = supperM as KhDlzbmxModel;
                    hid.Value = model.ID;

                    //得分累计
                    if (model.ZbsxModel.JxzrsZb.ZZBXZ == "主指标")
                        LdbzLhzbHeji += model.SJDF.ToDecimal();

                    //指标类别 1
                    e.Row.Cells[1].Text = model.LhzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = ((model.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? 
                        model.LhzbModel.GradeSymbol + model.LhzbModel.Zbxx.ZBMC : model.LhzbModel.Zbxx.ZBMC);
                    //指标性质 考核权重 3
                    e.Row.Cells[3].Text = model.ZbsxModel.JxzrsZb.ZZBXZ + "<br/>〖"
                        + (model.ZbsxModel.SXQZ.ToDecimal() * 100).ToString("f1") + "%〗";
                    //考核目标值4
                    if (model.MBZ != null)
                    {
                        e.Row.Cells[4].Text = model.MBZ.Value.ToString("f2") + model.LhzbModel.JSDW;
                        e.Row.Cells[4].ToolTip = model.MBZBZ;
                    }
                    //审核完成值5
                    e.Row.Cells[5].Text = (model.WCZSHZ != null) ? model.WCZSHZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //审核说明6
                    e.Row.Cells[6].Text = model.WCZSHBZ;
                    //审定完成值 7
                    e.Row.Cells[7].Text = (model.WCZ != null) ? model.WCZ.Value.ToString("f2") + model.LhzbModel.JSDW : "";
                    //审定说明 8
                    e.Row.Cells[8].Text = model.WCZBZ;
                    //本项得分9
                    e.Row.Cells[9].Text = model.SJDF.ToRequestString();
                }
                else if (supperM.GetType().IsAssignableFrom(typeof(KhDfzbmxModel)))
                {
                    //打分指标
                    KhDfzbmxModel dfModel = supperM as KhDfzbmxModel;
                    hid.Value = dfModel.DFZBID;
                    //得分累计
                    LdbzDfzbHeji += dfModel.SJDF.ToDecimal();
                    //指标类别 1
                    e.Row.Cells[1].Text = dfModel.DfzbModel.Zbxx.YJZBMC;
                    //指标名称 2
                    e.Row.Cells[2].Text = ((dfModel.ZbsxModel.JxzrsZb.ZZBXZ == "辅助指标") ? 
                        "﹄﹄" + dfModel.DfzbModel.Zbxx.ZBMC : dfModel.DfzbModel.Zbxx.ZBMC);
                    //打分理由 3
                    e.Row.Cells[3].Text = dfModel.DFBZ;
                    //本项得分9
                    e.Row.Cells[9].Text = dfModel.SJDF.ToRequestString();
                    e.Row.Cells[3].ColumnSpan = 3;//6;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Justify;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //合计
                var ndxsModel = ndxsSrv.GetListByKhid(Ddl_Kaohe.SelectedValue).
                    FirstOrDefault(p => p.JGBM == Ddl_Danwei.SelectedValue.ToInt());
                decimal jyndxs = 1.0M;
                if (ndxsModel != null)
                {
                    jyndxs = ndxsModel.NDXS.ToDecimal();
                    e.Row.Cells[2].Text = jyndxs.ToRequestString();
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells[0].Text = "经营难度系数：";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].ColumnSpan = 3;//6;
                e.Row.Cells[3].Text = "得分小计：";
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                decimal ldbzZdf = LdbzLhzbHeji * jyndxs + LdbzDfzbHeji;
                e.Row.Cells[9].Text = string.Format("<font color='red'>{0}</font>", ldbzZdf.ToString("f2"));
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }
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


        #region 专用方法区

        /// <summary>
        /// 导出操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Report_Click(object sender, EventArgs e)
        {
            GridView1.HeaderStyle.Height = Unit.Pixel(50);
            GridView1.RowStyle.Height = Unit.Pixel(30);
            GridView1.FooterStyle.Height = Unit.Pixel(30);
            string title = Ddl_Kaohe.SelectedItem.Text + "(" + Ddl_Danwei.SelectedItem.Text + ")" + "考核明细表";
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