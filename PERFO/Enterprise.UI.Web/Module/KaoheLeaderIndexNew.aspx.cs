using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Enterprise.Service.Perfo.Sys;
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;
using Enterprise.Component.Cache;

namespace Enterprise.UI.Web.Module
{
    /// <summary>
    /// 绩效考核首页面--领导用
    /// </summary>
    public partial class KaoheLeaderIndexNew : PageBase
    {

        #region 初始化参数区

        /// <summary>
        /// 日报时间
        /// </summary>
        public DateTime ReportDay { get; set; }
        /// <summary>
        /// by pengwei
        /// 当前项目已有日报的天数
        /// </summary>
        public string ReportedDays = "";
        /// <summary>
        /// 通知服务类
        /// </summary>
        protected KhArticlesService articleSrv = new KhArticlesService();
        /// <summary>
        /// 考核管理服务类
        /// </summary>
        protected KhKhglService khglSrv = new KhKhglService();
        /// <summary>
        /// 考核制度汇编服务类
        /// </summary>
        protected KhKindhbService khzdhbSrv = new KhKindhbService();
        /// <summary>
        /// 待办事务服务类
        /// </summary>
        protected KhMessageService msgSrv = new KhMessageService();
        protected int MessageCount = 0;//待办数量
        /// <summary>
        /// 数据上报服务类
        /// </summary>
        protected KhSjsbService sjsbSrv = new KhSjsbService();

        /// <summary>
        /// 分管的单位
        /// </summary>
        protected string fgdw = (string)Utility.sink("fg", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindCondition();
                InitData();
                if (fgdw == "1" || fgdw == "2" || fgdw == "3" || fgdw == "4")
                {
                    userModel.BZ = fgdw;
                    userModel.DB_Option_Action = WebKeys.UpdateAction;
                    userService.Execute(userModel);
                }
            }
        }

        #region 页面方法区

        /// <summary>
        /// 绑定条件
        /// </summary>
        private void bindCondition()
        {
            //通知公告
            List<KhArticlesModel> tongzhiLst = articleSrv.GetTzListByYear(DateTime.Now.Year.ToString()) as List<KhArticlesModel>;

            //提取最新的一条通知
            var lastTz = tongzhiLst.FirstOrDefault();
            if (lastTz != null)
            {
                string newImg = "<img src=\"/Resources/OA/site_skin/images/new.gif\" />";
                bool isNew = (lastTz.TJRQ.ToDateYMDFormat() == DateTime.Now.ToDateYMDFormat());
                Lbl_TopTongzhi.Text = string.Format(
                    "<a style=\"text-decoration-line:underline;cursor:hand;\" onclick=\"javascript:parent.addTab('/M.K.Tzview?TZID={0}','通知公告详情');\">{1}</a>{2}", lastTz.TZID, lastTz.TZBT, ((isNew) ? newImg : ""));
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            //1==提取状态为进行中和考核结束时间为当前年度的所有考核
            List<KhKhglModel> kaoheLst = khglSrv.GetKhListForIndex() as List<KhKhglModel>;
            GridView_Kaohe.DataSource = kaoheLst;
            GridView_Kaohe.DataBind();
            //2==提取当前用户的所有处于“进行中”的考核相关的待办事务
            List<KhMessageModel> msgLst = msgSrv.GetUntreatedMsgForUser(userModel.LOGINID) as List<KhMessageModel>;
            MessageCount = msgLst.Count;
            GridView_MyMessage.DataSource = msgLst;
            GridView_MyMessage.DataBind();
            //3==考核制度汇编
            List<KhKindhbModel> khzdhbLst = khzdhbSrv.GetList() as List<KhKindhbModel>;
            GridView_Khzdhb.DataSource = khzdhbLst;
            GridView_Khzdhb.DataBind();
        }

        #endregion

        #region 数据绑定事件

        /// <summary>
        /// 考核制度汇编行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_Khzdhb_RowDataBound(object sender, GridViewRowEventArgs e)
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
        /// 考核数据行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_Kaohe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhKhglModel model = e.Row.DataItem as KhKhglModel;
                if (model.LXID == "LX2014A")
                {
                    e.Row.Cells[1].Text =
                        string.Format("<a href=\"javascript:parent.addTab('/M.K.CxtjEjdw?KH={0}','二级单位考核结果');\">{1}</a>", model.KHID, model.KHMC);
                }
                else if (model.LXID == "LX2014B")
                {
                    e.Row.Cells[1].Text =
                        string.Format("<a href=\"javascript:parent.addTab('/M.K.CxtjJgbm?KH={0}','机关部门考核结果');\">{1}</a>", model.KHID, model.KHMC);
                }
            }
        }

        /// <summary>
        /// 通知公告行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_Tongzhi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhArticlesModel model = e.Row.DataItem as KhArticlesModel;
                string newImg = "<img src=\"/Resources/OA/site_skin/images/new.gif\" />";
                bool isNew = (model.TJRQ.ToDateYMDFormat() == DateTime.Now.ToDateYMDFormat());
                e.Row.Cells[1].Text = string.Format(
                    "<a href=\"javascript:parent.addTab('/M.K.Tzview?TZID={0}','通知公告详情');\">{1}</a>{2}", model.TZID, model.TZBT, ((isNew) ? newImg : ""));
            }
        }

        /// <summary>
        /// 待办事务行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_MyMessage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhMessageModel model = e.Row.DataItem as KhMessageModel;
                //事项名称 1
                e.Row.Cells[1].Text = (model.Kaohe != null) ? (model.Kaohe.KHMC + "," + model.DBMC) : model.DBMC;
                //待办说明 2
                e.Row.Cells[2].Text = string.Format(
                    "<a href=\"javascript:parent.addTab('{0}','{1}');\">{2}</a>", model.DBLJ, model.DBMC, model.DBSM);
            }
        }

        /// <summary>
        /// 待办事务行命令操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_MyMessage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var model = msgSrv.GetSingle(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "bianji":
                    if (model != null)
                    {
                        model.DB_Option_Action = WebKeys.UpdateAction;
                        model.DQZT = "1";
                        msgSrv.Execute(model);
                    }
                    InitData();
                    TabTitle = string.Format("我的待办箱【{0}】", MessageCount);
                    break;
            }
        }

        #endregion

        #region 专用方法区

        /// <summary>
        /// 获取当前最大的考核年份下的所有二级单位及领导班子考核成绩
        /// </summary>
        /// <param name="pJgbm">局级单位编码</param>
        /// <returns></returns>
        protected string GetEjdwKaoheInfo(string pJgbm)
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(khglSrv.CacheKey + "_GetEjdwKaoheJieguoInfo_" + pJgbm);
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                /*
                 1、从考核管理表中提取当前最大的考核年份和对应的各期考核信息
                 2、从数据上报表中提取每期考核对应的组织机构；
                 3、从考核成绩表中提取各单位各期的考核成绩
                 */
                KhEjdwkhdfService ejdwkhdfSrv = new KhEjdwkhdfService();
                List<KhEjdwkhdfModel> khdfList = new List<KhEjdwkhdfModel>();
                var allKaoheLst = khglSrv.GetList();
                if (allKaoheLst.Count == 0) return "";
                //1最大考核年份
                int maxKhnd = allKaoheLst.Max(p => p.KHND.ToInt()).ToInt();
                //2各期考核信息
                var maxKhndKaoheLst = allKaoheLst.Where(p => p.KHND == maxKhnd.ToString() && p.LXID == "LX2014A").OrderBy(p => p.KSSJ).ToList();
                //3指定局级单位下的所有单位信息
                var khbmjgLst = sjsbSrv.GetSjsbListByKhnd(maxKhnd).Where(p => p.Bmjg.XSXH.StartsWith(pJgbm) && p.Bmjg.JGLX.Contains("二级")).
                    OrderBy(p => p.Bmjg.BZ).ToList();

                sb.Append("<tr>");
                sb.Append("<th colspan=\"" + (maxKhndKaoheLst.Count + 2) + "\" class=\"td-bold\">二级单位及领导班子历次考核成绩【<span class=\"label label-info\">" + maxKhnd + "年</span>】</th>");
                sb.Append("</tr>");
                sb.Append("<tr style=\"font-weight: bold;\">");
                sb.Append("<td style=\"width: 50px;\">序号</td>");
                sb.Append("<td style=\"width: 220px;\">单位名称</td>");
                foreach (var khinfo in maxKhndKaoheLst)
                {
                    sb.Append("<td>" + khinfo.KSSJ.Value.ToString("yyyy/MM") + "【" + khinfo.KHZQ + "考核】</td>");
                    khdfList.AddRange(ejdwkhdfSrv.GetEjdwdfListByKhid(khinfo.KHID.ToString()));
                }

                sb.Append("</tr>");

                //3
                int idx = 0;
                foreach (var m in khbmjgLst)
                {
                    ++idx;
                    sb.Append("<tr " + ((idx % 2 == 0) ? "style='background-color: #F7F7F7;'" : "") + ">");
                    sb.Append("<td>" + idx + "</td>");
                    sb.Append("<td>" + m.Bmjg.JGMC + "</td>");
                    //二级单位及领导班子考核，查询其相应考核成绩并显示
                    foreach (var khinfo in maxKhndKaoheLst)
                    {
                        var khdf = khdfList.FirstOrDefault(p => p.JGBM == m.JGBM);
                        if (khdf != null)
                        {
                            string urlEjdw =
                                string.Format("javascript:parent.addTab('/M.K.CxtjEjdw?KH={0}&BM={1}','{2}结果表【二级单位】');",
                                khdf.KHID, khdf.JGBM, khinfo.KHMC);
                            string urlEjdwLdbz =
                                string.Format("javascript:parent.addTab('/M.K.CxtjEjdwLdbz?KH={0}&BM={1}','{2}结果表【领导班子】');",
                                khdf.KHID, khdf.JGBM, khinfo.KHMC);
                            string colorName = getColor(khdf.DWPM.ToInt(), khdf.FZRDFLB);
                            string dfStr = "";
                            if (!string.IsNullOrEmpty(khdf.HZBZ))
                            {
                                dfStr += "<a href=\"" + urlEjdw + "\"><span class=\"" + colorName + "\">" + khdf.DWZDF + "&nbsp;</span></a>";
                            }
                            if (khdf.HZBZ == "2")
                            {
                                dfStr += "|<a href=\"" + urlEjdwLdbz + "\"><span class=\"" + colorName + "\">&nbsp;" + khdf.FZRDFLB + "</span></a>";
                            }
                            sb.Append("<td class=\"show\">" + dfStr + "</td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"show\">&nbsp;</td>");
                        }
                    }
                    sb.Append("</tr>");
                }
                khInfo = sb.ToString();
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                //数据存入缓存系统
                CacheHelper.Add(khglSrv.CacheKey + "_GetEjdwKaoheJieguoInfo_" + pJgbm, khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// 获取当前最大的考核年份下的所有机关部门的考核成绩
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(khglSrv.CacheKey + "_GetJgbmKaoheJieguoInfo");
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                /*
                 1、从考核管理表中提取当前最大的考核年份和对应的各期考核信息
                 2、从数据上报表中提取每期考核对应的组织机构；
                 3、从考核成绩表中提取各单位各期的考核成绩
                 */
                KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();
                List<KhJgbmkhdfModel> khdfList = new List<KhJgbmkhdfModel>();
                var allKaoheLst = khglSrv.GetList();
                if (allKaoheLst.Count == 0) return "";
                //1最大考核年份
                int maxKhnd = allKaoheLst.Max(p => p.KHND.ToInt()).ToInt();
                //2各期考核信息
                var maxKhndKaoheLst = allKaoheLst.Where(p => p.KHND == maxKhnd.ToString() && p.LXID == "LX2014B").OrderBy(p => p.KSSJ).ToList();
                //3指定局级单位下的所有单位信息
                var khbmjgLst = sjsbSrv.GetSjsbListByKhnd(maxKhnd).Where(p => p.Bmjg.JGLX.Contains("职能")).
                    OrderBy(p => p.Bmjg.BZ).ToList();

                sb.Append("<tr>");
                sb.Append("<th colspan=\"" + (maxKhndKaoheLst.Count + 2) + "\" class=\"td-bold\">机关部门及负责人历次考核成绩【<span class=\"label label-info\">" + maxKhnd + "年</span>】</th>");
                sb.Append("</tr>");
                sb.Append("<tr style=\"font-weight: bold;\">");
                sb.Append("<td style=\"width: 50px;\">序号</td>");
                sb.Append("<td style=\"width: 220px;\">单位名称</td>");
                foreach (var khinfo in maxKhndKaoheLst)
                {
                    sb.Append("<td>" + khinfo.KSSJ.Value.ToString("yyyy/MM") + "【" + khinfo.KHZQ + "考核】</td>");
                    khdfList.AddRange(jgbmkhdfSrv.GetJgbmdfListByKhid(khinfo.KHID.ToString()));
                }
                sb.Append("</tr>");

                //3
                int idx = 0;
                foreach (var m in khbmjgLst)
                {
                    ++idx;
                    sb.Append("<tr " + ((idx % 2 == 0) ? "style='background-color: #F7F7F7;'" : "") + ">");
                    sb.Append("<td>" + idx + "</td>");
                    sb.Append("<td>" + m.Bmjg.JGMC + "</td>");
                    //机关部门及负责人考核，查询其相应考核成绩并显示
                    foreach (var khinfo in maxKhndKaoheLst)
                    {
                        var khdf = khdfList.FirstOrDefault(p => p.JGBM == m.JGBM);
                        if (khdf != null)
                        {
                            string urlJgbm =
                                string.Format("javascript:parent.addTab('/M.K.CxtjJgbm?KH={0}&BM={1}','{2}结果表【部门】');",
                                khdf.KHID, khdf.JGBM, khinfo.KHMC);
                            string urlJgbmLdbz =
                                string.Format("javascript:parent.addTab('/M.K.CxtjJgbmFzr?KH={0}&BM={1}','{2}结果表【负责人】');",
                                khdf.KHID, khdf.JGBM, khinfo.KHMC);
                            string colorName = getColor(khdf.BMPM.ToInt(), "");
                            string dfStr = "";
                            if (!string.IsNullOrEmpty(khdf.HZBZ))
                            {
                                dfStr += "<a href=\"" + urlJgbm + "\"><span class=\"" + colorName + "\">" + khdf.BMZDF + "&nbsp;</span></a>";
                            }
                            if (khdf.HZBZ == "2")
                            {
                                dfStr += "|<a href=\"" + urlJgbmLdbz + "\"><span class=\"" + colorName + "\">&nbsp;" + khdf.FZRDXBS + "</span></a>";
                            }
                            sb.Append("<td class=\"show\">" + dfStr + "</td>");
                        }
                        else
                        {
                            sb.Append("<td class=\"show\">&nbsp;</td>");
                        }
                    }
                    sb.Append("</tr>");
                }
                khInfo = sb.ToString();
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                //数据存入缓存系统
                CacheHelper.Add(khglSrv.CacheKey + "_GetJgbmKaoheJieguoInfo", khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// 根据用户的分管单位进行优先显示
        /// </summary>
        /// <returns></returns>
        protected string GetFgdwKhjgInfo()
        {
            StringBuilder sb = new StringBuilder();

            switch (userModel.BZ)
            {
                case "2":
                    sb.Append("<div title=\"华东分公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table2\">");
                    sb.Append(GetEjdwKaoheInfo("0102"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油工程公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table8\">");
                    sb.Append(GetEjdwKaoheInfo("0103"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"机关部门\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table9\">");
                    sb.Append(GetJgbmKaoheInfo());
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油局\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
                    sb.Append(GetEjdwKaoheInfo("0101"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    break;
                case "3":
                    sb.Append("<div title=\"华东石油工程公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table8\">");
                    sb.Append(GetEjdwKaoheInfo("0103"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"机关部门\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table9\">");
                    sb.Append(GetJgbmKaoheInfo());
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油局\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
                    sb.Append(GetEjdwKaoheInfo("0101"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东分公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table2\">");
                    sb.Append(GetEjdwKaoheInfo("0102"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    break;
                case "4":
                    sb.Append("<div title=\"机关部门\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table9\">");
                    sb.Append(GetJgbmKaoheInfo());
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油局\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
                    sb.Append(GetEjdwKaoheInfo("0101"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东分公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table2\">");
                    sb.Append(GetEjdwKaoheInfo("0102"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油工程公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table8\">");
                    sb.Append(GetEjdwKaoheInfo("0103"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    break;
                default:
                    sb.Append("<div title=\"华东石油局\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table1\">");
                    sb.Append(GetEjdwKaoheInfo("0101"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东分公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table2\">");
                    sb.Append(GetEjdwKaoheInfo("0102"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"华东石油工程公司\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table8\">");
                    sb.Append(GetEjdwKaoheInfo("0103"));
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div title=\"机关部门\" data-options=\"iconCls:'icon-xlsx'\" style=\"padding: 4px\">");
                    sb.Append("<div class=\"main-gridview\">");
                    sb.Append("<table style=\"width: 100%; text-align: center;\" id=\"Table9\">");
                    sb.Append(GetJgbmKaoheInfo());
                    sb.Append("</table>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    break;
            }
            return sb.ToString();
        }

        #endregion

        #region 专用方法区

        /// <summary>
        /// 根据名次或等级来选定不同颜色标识
        /// </summary>
        /// <param name="mc">名次</param>
        /// <param name="grade">等级</param>
        /// <returns></returns>
        private string getColor(int mc, string grade)
        {
            string colorName = "green";
            switch (grade)
            {
                case "A":
                    colorName = "green";
                    break;
                case "B":
                    colorName = "blue";
                    break;
                case "C":
                    colorName = "yellow";
                    break;
                case "D":
                    colorName = "red";
                    break;
                default:
                    if (mc <= 3) colorName = "green";
                    else colorName = "blue";
                    break;
            }

            return colorName;
        }

        #endregion

    }
}