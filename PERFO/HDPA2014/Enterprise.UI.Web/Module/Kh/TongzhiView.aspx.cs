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
using Enterprise.Service.Perfo.Zbk;
using Enterprise.Model.Perfo.Zbk;
using Enterprise.Model.Perfo.Sys;
using Enterprise.Service.Perfo.Kh;
using Enterprise.Model.Perfo.Kh;
using Enterprise.Component.Infrastructure;

namespace Enterprise.UI.Web.Kh
{
    /// <summary>
    /// 通知显示页面
    /// </summary>
    public partial class TongzhiView : PageBase
    {

        /// <summary>
        /// 通知对象
        /// </summary>
        public KhArticlesModel ThisArticle { get; set; }

        protected string TZID = (string)Utility.sink("TZID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//通知ID

        /// <summary>
        /// 服务类
        /// </summary>
        KhArticlesService artSrv = new KhArticlesService();
        KhSigninService signinSrv = new KhSigninService();

        #region 权限检查
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            base.PermissionHandler += new PermissionEventHandler(Page_PermissionHandler);
        }

        /// <summary>
        /// 进行具体的权限设置
        /// </summary>
        /// <param name="e"></param>
        void Page_PermissionHandler(PermissionEventArgs e)
        {
            if (e.Model != null)
            {
                if (Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION),(long)WebKeys.PermissionType.Upd))
                {
                    adminPanel.Visible = true;
                }
                else
                {
                    adminPanel.Visible = false;
                }
            }
        }
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ThisArticle = artSrv.GetSingle(TZID);
                if (ThisArticle == null)
                {
                    ThisArticle = new KhArticlesModel();
                }
                else
                {
                    //记录浏览信息
                    KhSigninModel mm = new KhSigninModel();
                    mm.TZID = ThisArticle.TZID;
                    mm.QSR = userModel.USERNAME;
                    mm.QSRQ = DateTime.Now;
                    mm.QSDW = userModel.Bmjg.JGMC;
                    mm.DB_Option_Action = WebKeys.InsertAction;
                    signinSrv.SignInTongzhi(mm);
                    ThisArticle.SigninLst = signinSrv.GetListByTZID(ThisArticle.TZID);
                    ThisArticle.DB_Option_Action = WebKeys.UpdateAction;
                    ThisArticle.LLCS += 1;
                    artSrv.Execute(ThisArticle);
                }
            }
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_del_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Hid_TZID.Value))
            {
                artSrv.DeleteArticleById(Hid_TZID.Value);
            }
            Response.Redirect("~/Module/Kh/TongzhiList.aspx");
            Response.End();
        }


        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Module/Kh/TongzhiList.aspx");
            Response.End();
        }

        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_edit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Module/Kh/TongzhiEdit.aspx?TZID=" + TZID);
            Response.End();
        }

        /// <summary>
        /// 获取浏览用户信息
        /// </summary>
        /// <returns></returns>
        protected string GetViewInfo()
        {
            StringBuilder sb = new StringBuilder();
            if (ThisArticle != null && ThisArticle.SigninLst != null)
            {
                foreach(var q in ThisArticle.SigninLst) {
                    string ss = string.Format("<span class=\"label label-info\" title=\"{0}\">{1}</span>&nbsp;", q.QSDW + "," + q.QSRQ.ToDateYMDFormat(), q.QSR);
                    sb.Append(ss);
                }
            }
            return sb.ToString();
        }

    }
}