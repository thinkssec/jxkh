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

namespace Enterprise.UI.Web.Module
{
    /// <summary>
    /// 绩效考核首页面--管理员用
    /// </summary>
    public partial class KaoheIndexNew : PageBase
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
                    "<a style=\"text-decoration-line:underline;cursor:hand;\" onclick=\"javascript:parent.addTab('/Module/Kh/TongzhiView.aspx?TZID={0}','通知公告详情');\">{1}</a>{2}", lastTz.TZID, lastTz.TZBT, ((isNew) ? newImg : ""));
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
                else if (model.LXID == "LX2014C")
                {
                    e.Row.Cells[1].Text =
                        string.Format("{0}",  model.KHMC);
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
                    "<a href=\"javascript:parent.addTab('/Module/Kh/TongzhiView.aspx?TZID={0}','通知公告详情');\">{1}</a>{2}", model.TZID, model.TZBT, ((isNew) ? newImg : ""));
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
            return khglSrv.GetEjdwKaoheInfo();
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
            return khglSrv.GetJgbmKaoheInfo();
        }

        #endregion

    }
}