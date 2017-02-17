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
    /// 绩效考核首页面--部门用
    /// </summary>
    public partial class KaoheBumenIndexNew : PageBase
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
        /// 机关部门考核得分服务类
        /// </summary>
        protected KhJgbmkhdfService jgbmkhdfSrv = new KhJgbmkhdfService();
        protected List<KhJgbmkhdfModel> JgbmkhdfList = null;//考核得分数据集
        protected List<KhKhglModel> KaoheLst = null;//考核信息集合
        KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//定量指标服务类
        KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标服务类
        KhJgbmdfbService jgbmdfSrv = new KhJgbmdfbService();//量化及打分情况服务类

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
            //GridView_Tongzhi.DataSource = tongzhiLst;
            //GridView_Tongzhi.DataBind();
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
            KaoheLst = khglSrv.GetKhListByLxidForIndex("LX2014B") as List<KhKhglModel>;
            GridView_Kaohe.DataSource = KaoheLst;
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
                //机关部门
                e.Row.Cells[1].Text =
                        string.Format("<a href=\"javascript:parent.addTab('/M.K.CxtjJgbm?KH={0}','机关部门考核结果');\">{1}</a>", model.KHID, model.KHMC);
                //当前状态 3
                if (model.KHZT == "1")
                {
                    e.Row.Cells[3].Text = "<span class=\"label-success label label-default\">已完成</span>";
                }
                else if (model.KHZT == "0")
                {
                    int processV = khglSrv.GetKaoheProcessValue(model);
                    string khdt =
                        "<div class=\"progress progress-striped progress-success active\">"
                        + "<div class=\"progress-bar\" style=\"width: " + processV + "%;\" onclick=\"ShowProgress('" + model.LXID + "','" + processV + "');\">" + processV + "%</div>"
                        + "</div>";
                    e.Row.Cells[3].Text = khdt;
                }

                //部门得分 4
                JgbmkhdfList = jgbmkhdfSrv.GetListByKhid(model.KHID.ToString()).Where(p => !string.IsNullOrEmpty(p.HZBZ)).ToList();
                var bmdf = JgbmkhdfList.FirstOrDefault(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString() && p.JGBM == userModel.JGBM);
                if (bmdf != null)
                {
                    e.Row.Cells[4].Text = "<div class=\"show\"><span class=\"blue\">" + bmdf.BMZDF + "</span></div>";
                }
                //负责人得分 5
                var bmfzrdf = JgbmkhdfList.FirstOrDefault(p => p.KHLX == ((int)WebKeys.KaoheType.部门负责人).ToString() && p.JGBM == userModel.JGBM);
                if (bmfzrdf != null)
                {
                    e.Row.Cells[5].Text = "<div class=\"show\"><span class=\"yellow\">" + bmdf.FZRZDF + "</span></div>";
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
        /// 获取最新一期的二级单位及领导班子考核中需要该用户的单位审核的指标
        /// </summary>
        /// <returns></returns>
        protected string GetEjdwKaoheInfo()
        {
            return khglSrv.GetEjdwKaoheInfoByJgbm(userModel);
        }

        /// <summary>
        /// 获取最新一期的机关部门及负责人考核信息
        /// </summary>
        /// <returns></returns>
        protected string GetJgbmKaoheInfo()
        {
            string khInfo = string.Empty;
            if (WebKeys.EnableCaching)
            {
                khInfo = (string)CacheHelper.GetCache(khglSrv.CacheKey + "_GetJiguanbumenKaoheInfo_" + userModel.JGBM);
            }
            if (string.IsNullOrEmpty(khInfo))
            {
                StringBuilder sb = new StringBuilder();
                //最近一期考核
                var lastKaohe = khglSrv.GetLastKaoheInfo("LX2014B");//机关部门及负责人考核
                if (lastKaohe != null)
                {
                    sb.Append("<tr>");
                    sb.Append("<th class=\"td-bold\" colspan=\"4\">" + lastKaohe.KHMC + "</th>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style=\"width: 15%; font-weight: bold;\">序号</td>");
                    sb.Append("<td style=\"width: 35%; font-weight: bold;\">指标类型</td>");
                    sb.Append("<td style=\"width: 25%; font-weight: bold;\">指标数量</td>");
                    sb.Append("<td style=\"width: 25%; font-weight: bold;\">考核权重</td>");
                    sb.Append("</tr>");

                    //提取考核期内的定量和定性指标
                    var dlzbLst = dlzbmxSrv.GetListByKhidAndJgbm(lastKaohe.KHID.ToString(), userModel.JGBM.ToString());
                    if (dlzbLst.Count == 0)
                    {
                        //当前部门未参与
                        return string.Format("<tr><td align='Left'><font color='red'>{0}</font></td></tr>", "未查询到您单位的相关信息!");
                    }
                    var zbmcLst = dlzbLst.DistinctBy(p => p.LhzbModel.Zbxx.ZBMC).ToList();
                    int index = 0;
                    for (int i = 0; i < zbmcLst.Count; i++)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + (i + 1) + "</td>");
                        sb.Append("<td>" + zbmcLst[i].LhzbModel.Zbxx.ZBMC + "</td>");
                        sb.Append("<td class=\"show\"><span class=\"green\">" + dlzbLst.Count(p => p.ZBBM == zbmcLst[i].ZBBM) + "</span></td>");
                        sb.Append("<td class=\"show\"><span class=\"red\">" + zbmcLst[i].ZbsxModel.SXQZ.ToDecimal().ToString("P") + "</span></td>");
                        sb.Append("<tr>");
                        index = i + 1;
                    }

                    var dfzbLst = dfzbmxSrv.GetListByKhidAndJgbm(lastKaohe.KHID.ToString(), userModel.JGBM.ToString());
                    if (dfzbLst.Count > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + (++index) + "</td>");
                        sb.Append("<td>机关作风建设</td>");
                        sb.Append("<td class=\"show\"><span class=\"green\">" + (dfzbLst.Count(p => p.DfzbModel.Zbxx.YJZBMC.Contains("机关作风"))) + "</span></td>");
                        sb.Append("<td class=\"show\"><span class=\"red\">+</span></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td>" + (++index) + "</td>");
                        sb.Append("<td>连带指标</td>");
                        sb.Append("<td class=\"show\"><span class=\"green\">" + (dfzbLst.Count(p => p.DfzbModel.Zbxx.YJZBMC.Contains("连带"))) + "</span></td>");
                        sb.Append("<td class=\"show\"><span class=\"red\">+/-</span></td>");
                        sb.Append("</tr>");
                    }
                    khInfo = sb.ToString();
                }
            }
            if (WebKeys.EnableCaching && !string.IsNullOrEmpty(khInfo))
            {
                //数据存入缓存系统
                CacheHelper.Add(khglSrv.CacheKey + "_GetJiguanbumenKaoheInfo_" + userModel.JGBM, khInfo);
            }
            return khInfo;
        }

        /// <summary>
        /// 生成图表相关数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetChartData(int type)
        {
            List<string> datas = new List<string>();
            //string datas = string.Empty;
            var jgbmKh = KaoheLst.Where(p => p.LXID == "LX2014B").OrderBy(p => p.KSSJ).ToList();
            foreach (var kh in jgbmKh)
            {
                JgbmkhdfList = jgbmkhdfSrv.GetListByKhid(kh.KHID.ToString()).
                    Where(p => !string.IsNullOrEmpty(p.HZBZ)).ToList();
                switch (type)
                {
                    case 1://部门得分
                        var bmdf = JgbmkhdfList.FirstOrDefault(p => p.KHLX == ((int)WebKeys.KaoheType.机关部门).ToString() && p.JGBM == userModel.JGBM);
                        if (bmdf != null)
                        {
                            datas.Add(bmdf.BMZDF.ToDecimal().ToString());
                        }
                        break;
                    case 2://负责人得分
                        var bmfzrdf = JgbmkhdfList.FirstOrDefault(p => p.KHLX == ((int)WebKeys.KaoheType.部门负责人).ToString() && p.JGBM == userModel.JGBM);
                        if (bmfzrdf != null)
                        {
                            datas.Add(bmfzrdf.FZRZDF.ToDecimal().ToString());
                        }
                        break;
                    case 3://标题
                        datas.Add("'" + kh.KSSJ.Value.ToString("yyyy/MM") + "'");
                        break;
                }
            }
            if (datas.Count < 4)
            {
                for (int i = datas.Count; i < 4; i++)
                {
                    if (type == 3)
                        datas.Add("' '");
                    else
                        datas.Add("0");
                }
            }
            return string.Join(",", datas);
        }

        #endregion

    }
}