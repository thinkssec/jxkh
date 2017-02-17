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
    /// 通知编辑页面
    /// </summary>
    public partial class TongzhiEdit : PageBase
    {

        /// <summary>
        /// 服务类
        /// </summary>
        KhArticlesService articleSrv = new KhArticlesService();

        protected string TZID = (string)Utility.sink("TZID", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//通知ID

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
                LnkBtn_Upd.Visible = Utility.CheckPermission(Convert.ToInt64(e.Model.MODULEPERMISSION), (long)WebKeys.PermissionType.Upd);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearValue();
                BindData();
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            KhArticlesModel model = articleSrv.GetSingle(TZID);
            if (model != null)
            {
                CommonTool.SetModelDataToForm(model, Page, "Txt_", true);
                Hid_TZID.Value = model.TZID;
            }
            else
            {
                //新增
                Txt_TJRQ.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Txt_TZZZ.Text = userModel.USERNAME;
            }
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
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LnkBtn_Upd_Click(object sender, EventArgs e)
        {
            KhArticlesModel model = (KhArticlesModel)CommonTool.GetFormDataToModel(typeof(KhArticlesModel), Page);
            if (!string.IsNullOrEmpty(model.TZID))
            {
                model.DB_Option_Action = WebKeys.UpdateAction;
            }
            else
            {
                model.DB_Option_Action = WebKeys.InsertAction;
                model.TZID = CommonTool.GetGuidKey();
            }
            model.LLCS = 1;
            model.TZLX = "1";
            articleSrv.Execute(model);
            Utility.ShowMsg(Page, "提示", "通知公告发布成功!", 150, "show");
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void clearValue()
        {
            Hid_TZID.Value = "";
            Txt_TZBT.Text = "";
            Txt_TZNR.Text = "";
            Txt_TZZZ.Text = "";
            Txt_TJRQ.Text = "";
        }
    }
}