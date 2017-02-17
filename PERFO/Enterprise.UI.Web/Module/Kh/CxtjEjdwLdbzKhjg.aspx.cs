using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;
using System.IO;

namespace Enterprise.UI.Web.Kh
{

    /// <summary>
    /// 二级单位领导班子考核结果查询页面
    /// </summary>
    public partial class CxtjEjdwLdbzKhjg : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhEjdwkhdfService ejdwkhdfSrv = new KhEjdwkhdfService();//二级单位考核得分表
        KhKhglService khglSrv = new KhKhglService();//考核管理
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID
        protected KhKhglModel Kaohe = null;//考核期
        protected List<KhEjdwkhdfModel> EjdwkhdfList = null;//考核得分数据集

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
            //数据集
            EjdwkhdfList = ejdwkhdfSrv.GetListByKhid(Ddl_Kaohe.SelectedValue) as List<KhEjdwkhdfModel>;
            var dataList = EjdwkhdfList.Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() || p.ISHBJF == "1")
                    .DistinctBy(p => p.JGMC).OrderBy(p => p.GSDWMC).ThenBy(p => p.FZRDFLB).ThenBy(p => p.DWPM).ToList();
            if (dataList.Count > 0 &&
                !string.IsNullOrEmpty(dataList.First().HZBZ))
            {
                Lbl_Msg.Text = "本期考核结果已发布了!";

                //领导班子
                GridView1.DataSource = dataList;
                GridView1.DataBind();
                Utility.GroupRows(GridView1, 0);
                Utility.GroupRows(GridView1, 1);
                Utility.GroupRows(GridView1, 2);
                
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
        /// 生成二级单位表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //判断创建的行是否为表头行
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                //"类别#归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#经营难度系数#加减分情况#最终得分#排名#兑现倍数"
                string header = "类别#归属单位#序号#单位名称#效益类指标|标准分,考核得分#管理类指标|标准分,考核得分#综合得分#经营难度系数#加减分情况#最终得分#排名#兑现倍数";
                dHelper.SplitTableHeader(e.Row, header);
            }
        }

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhEjdwkhdfModel model = e.Row.DataItem as KhEjdwkhdfModel;

                if (model.ISHBJF == "1")
                {
                    //合并计分单位汇总------------------------------
                    //类别 0 
                    e.Row.Cells[0].Text = model.FZRDFLB;
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = (model.Hbjf != null) ? model.Hbjf.HBJFMC : "";
                    //综合得分8
                    e.Row.Cells[8].Text = model.KHDF.ToRequestString();
                    //经营难度系数 9
                    e.Row.Cells[9].Text = model.NDXS.ToRequestString();
                    //加减分情况 10
                    e.Row.Cells[10].Text = model.KHBZF.ToRequestString();
                    //最终得分11
                    e.Row.Cells[11].Text = model.FZRZDF.ToRequestString();
                    //排名12
                    e.Row.Cells[12].Text = model.FZRPM.ToRequestString();
                    //兑现倍数 13
                    e.Row.Cells[13].Text = model.FZRDXBS.ToRequestString();
                }
                else if (!string.IsNullOrEmpty(model.HBJFID))
                {
                    //---合并计算得分的单位之一----------------------
                    //类别 0 
                    e.Row.Cells[0].Text = model.FZRDFLB;
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = string.Format("<a href=\"/M.K.CxtjEjdwLdbzMX?KH={0}&BM={1}\">{2}</a>", model.KHID, model.JGBM, model.JGMC);

                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "效益类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分4
                        e.Row.Cells[4].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分5
                        e.Row.Cells[5].Text = xylzb.KHDF.ToRequestString();
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }

                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分6
                        e.Row.Cells[6].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分7
                        e.Row.Cells[7].Text = gllzb.KHDF.ToRequestString();
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分8
                    e.Row.Cells[8].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";

                    //加减分情况 10
                    string jjfV = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "加减分因素" || p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToRequestString();
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "约束性指标")).FirstOrDefault();
                    e.Row.Cells[10].Text = jjfV;
                    //e.Row.Cells[10].ToolTip = (ysxzb != null) ? ysxzb.BZSM.ToRequestString() : "";
                }
                else
                {
                    //---独立计算得分的单位----------------------------
                    //类别 0 
                    e.Row.Cells[0].Text = model.FZRDFLB;
                    //归属单位 1
                    e.Row.Cells[1].Text = model.GSDWMC;
                    //序号 2
                    e.Row.Cells[2].Text = (model.FZRPM != null) ? model.FZRPM.ToString() : (e.Row.RowIndex + 1).ToString();
                    //单位名称3
                    e.Row.Cells[3].Text = string.Format("<a href=\"/M.K.CxtjEjdwLdbzMX?KH={0}&BM={1}\">{2}</a>", model.KHID, model.JGBM, model.JGMC);

                    //按类型提取数据
                    decimal dlzbZhdf = 0M;
                    var xylzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "效益类").FirstOrDefault();
                    if (xylzb != null)
                    {
                        //效益类标准分4
                        e.Row.Cells[4].Text = xylzb.KHBZF.ToRequestString();
                        //效益类得分5
                        e.Row.Cells[5].Text = xylzb.KHDF.ToRequestString();
                        dlzbZhdf += xylzb.KHDF.ToDecimal();
                    }

                    var gllzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.二级单位).ToString() && p.JGBM == model.JGBM
                            && p.KHXMC == "管理类").FirstOrDefault();
                    if (gllzb != null)
                    {
                        //管理类 标准分6
                        e.Row.Cells[6].Text = gllzb.KHBZF.ToRequestString();
                        //管理类得分7
                        e.Row.Cells[7].Text = gllzb.KHDF.ToRequestString();
                        dlzbZhdf += gllzb.KHDF.ToDecimal();
                    }
                    //综合得分8
                    e.Row.Cells[8].Text = (dlzbZhdf > 0) ? dlzbZhdf.ToString() : "";
                    //经营难度系数 9
                    e.Row.Cells[9].Text = model.NDXS.ToRequestString();
                    //加减分情况 10
                    string jjfV = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "加减分因素" || p.KHXMC == "约束性指标")).Sum(p => p.KHDF).ToRequestString();
                    var ysxzb = EjdwkhdfList.
                        Where(p => p.KHLX == ((int)WebKeys.KaoheType.领导班子).ToString() && p.JGBM == model.JGBM
                            && (p.KHXMC == "约束性指标")).FirstOrDefault();
                    e.Row.Cells[10].Text = jjfV;
                    //e.Row.Cells[10].ToolTip = (ysxzb != null) ? ysxzb.BZSM.ToRequestString() : "";
                   
                    //最终得分11
                    e.Row.Cells[11].Text = model.FZRZDF.ToRequestString();
                    //排名12
                    e.Row.Cells[12].Text = model.FZRPM.ToRequestString();
                    //兑现倍数 13
                    e.Row.Cells[13].Text = model.FZRDXBS.ToRequestString();
                }
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

        #region 数据导出

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