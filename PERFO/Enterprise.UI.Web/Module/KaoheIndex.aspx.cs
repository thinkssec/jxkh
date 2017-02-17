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

namespace Enterprise.UI.Web
{

    /// <summary>
    /// 绩效考核首页面--管理员用
    /// </summary>
    public partial class KaoheIndex : PageBase
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bindCondition();
                InitData();
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
            GridView_Tongzhi.DataSource = tongzhiLst;
            GridView_Tongzhi.DataBind();
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
            //1==提取状态为进行中和考核开始时间为当前年度的所有考核
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
        /// 获取最新一期的二级单位及领导班子考核信息
        /// </summary>
        /// <returns></returns>
        protected string GetEjdwKaoheInfo()
        {
            /*
            参评单位 2 
            考核值确认 2 
            数据填报 1 
            文件提交 1 
            已审核 1 
            已审定 0 
             */
            StringBuilder sb = new StringBuilder();
            //最近一期考核
            var lastKaohe = khglSrv.GetLastKaoheInfo("LX2014A");//二级单位及领导班子考核
            if (lastKaohe != null)
            {
                sb.Append("<tr>");
                sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-进展统计</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"width: 25%\">参评单位</td>");
                sb.Append(
                    "<td style=\"width: 25%\" class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"red\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "参评单位") + "</span></a></td>");
                sb.Append("<td style=\"width: 25%\">考核值确认</td>");
                sb.Append("<td style=\"width: 25%\" class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"red\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "考核值确认") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>数据填报</td>");
                sb.Append("<td class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"green\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "数据填报") + "</span></a></td>");
                sb.Append("<td>文件提交</td>");
                sb.Append("<td class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"green\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "文件提交") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>已审核</td>");
                sb.Append("<td class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"yellow\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "已审核") + "</span></a></td>");
                sb.Append("<td>已审定</td>");
                sb.Append("<td class=\"show\">"
                    + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计1')\">"
                    + "<span class=\"blue\">"
                    + khglSrv.GetKaoheProcessResult(lastKaohe, "已审定") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>考核进度</td>");
                sb.Append("<td colspan=\"3\">");
                sb.Append("<div class=\"progress progress-striped progress-success active\">");
                int processV = khglSrv.GetKaoheProcessValue(lastKaohe);
                sb.Append("<div class=\"progress-bar\" style=\"width: " + processV 
                    + "%;\" onclick=\"ShowProgress('LX2014A','" + processV + "');\">" + processV + "%</div>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取最新一期的机关部门及负责人考核信息
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmKaoheInfo()
        {
            /*
            参评部门 2 
            自评完成 2 
            上级测评 3 
            同级测评 3 
            文件提交 1 
            结果审定 0 
             */
            StringBuilder sb = new StringBuilder();
            //最近一期考核
            var lastKaohe = khglSrv.GetLastKaoheInfo("LX2014B");//机关部门及负责人考核
            if (lastKaohe != null)
            {
                sb.Append("<tr>");
                sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "-进展统计</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style=\"width: 25%\">参评部门</td>");
                sb.Append("<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"red\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "参评部门") + "</span></a></td>");
                sb.Append("<td style=\"width: 25%\">自评完成</td>");
                sb.Append("<td style=\"width: 25%\" class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"red\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "自评完成") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>上级测评</td>");
                sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"green\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "上级测评") + "</span></a></td>");
                sb.Append("<td>同级测评</td>");
                sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"green\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "同级测评") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>文件提交</td>");
                sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"yellow\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "文件提交") + "</span></a></td>");
                sb.Append("<td>结果审定</td>");
                sb.Append("<td class=\"show\">"
                        + "<a href=\"javascript:parent.addTab('/Module/KaoheShowInfo.aspx?KH=" + lastKaohe.KHID + "','考核进展情况统计2')\">"
                        + "<span class=\"blue\">"
                        + khglSrv.GetKaoheProcessResult(lastKaohe, "结果审定") + "</span></a></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td>考核进度</td>");
                sb.Append("<td colspan=\"3\">");
                sb.Append("<div class=\"progress progress-striped progress-success active\">");
                int processV = khglSrv.GetKaoheProcessValue(lastKaohe);
                sb.Append("<div class=\"progress-bar\" style=\"width: " + processV
                    + "%;\" onclick=\"ShowProgress('LX2014B','" + processV + "');\">" + processV + "%</div>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        #endregion

    }
}