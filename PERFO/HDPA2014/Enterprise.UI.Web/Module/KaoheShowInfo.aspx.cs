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

namespace Enterprise.UI.Web
{

    /// <summary>
    /// 考核进展情况统计
    /// </summary>
    public partial class KaoheShowInfo : PageBase
    {

        /// <summary>
        /// 考核管理服务类
        /// </summary>
        protected KhKhglService khglSrv = new KhKhglService();
        protected KhSjsbService sjsbSrv = new KhSjsbService();//数据上报
        protected KhDlzbmxService dlzbmxSrv = new KhDlzbmxService();//定量指标明细
        protected KhDfzbmxService dfzbmxSrv = new KhDfzbmxService();//打分指标明细
        protected KhJgbmdfbService jgbmdfbSrv = new KhJgbmdfbService();//详细打分表
        protected string Khid = (string)Utility.sink("KH", Utility.MethodType.Get, 0, 0, Utility.DataType.Str);//考核ID

        private List<KhDlzbmxModel> dlzbLst = null;
        private List<KhJgbmdfbModel> jgbmdfLst = null;

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
                
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }


        #region 绑定表格

        protected void BindGrid()
        {
            var kaohe = khglSrv.GetSingle(Khid);
            if (kaohe != null)
            {
                var list = sjsbSrv.GetListByKhid(Khid);
                dlzbLst = dlzbmxSrv.GetListByKhid(Khid).ToList();
                jgbmdfLst = jgbmdfbSrv.GetListByKhid(Khid).ToList();
                if (kaohe.LXID == "LX2014A")
                {
                    Lbl_EjdwKaohe.Text = kaohe.KHMC;
                    GridView1.DataSource = list;
                    GridView1.DataBind();
                    DIV_Ejdw.Visible = true;
                    DIV_Jgbm.Visible = false;
                }
                else if (kaohe.LXID == "LX2014B")
                {
                    Lbl_JgbmKaohe.Text = kaohe.KHMC;
                    GridView2.DataSource = list;
                    GridView2.DataBind();
                    DIV_Ejdw.Visible = false;
                    DIV_Jgbm.Visible = true;
                }
            }
        }

        #endregion


        #region 事件处理

        /// <summary>
        /// 行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhSjsbModel model = e.Row.DataItem as KhSjsbModel;
                //参评单位1
                e.Row.Cells[1].Text = model.Bmjg.JGMC;
                //考核值确认2
                e.Row.Cells[2].Text = 
                    (dlzbLst.Where(p => p.MBZQRRQ != null && p.JGBM == model.JGBM).Count() > 0)?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //数据填报3
                e.Row.Cells[3].Text =
                    (dlzbLst.Where(p => p.WCZSQRQ != null && p.JGBM == model.JGBM).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //完成值审核4
                e.Row.Cells[4].Text =
                    (dlzbLst.Where(p => p.WCZSHRQ != null && p.JGBM == model.JGBM).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //结果审定5
                e.Row.Cells[5].Text =
                    (dlzbLst.Where(p => p.WCZSDRQ != null && p.JGBM == model.JGBM).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //文件提交6
                e.Row.Cells[6].Text = (model.SBSJ != null)?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
            }
        }

        /// <summary>
        /// 行数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KhSjsbModel model = e.Row.DataItem as KhSjsbModel;
                //参评部门1
                e.Row.Cells[1].Text = model.Bmjg.JGMC;
                //自评情况2
                e.Row.Cells[2].Text = (jgbmdfLst.
                    Where(p => p.JGBM == model.JGBM && p.JGBM.ToString() == p.DFZ && p.KHDF != null).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //上级测评3
                e.Row.Cells[3].Text = (jgbmdfLst.
                    Where(p => p.JGBM == model.JGBM && p.DFZLX == ((int)WebKeys.DFUserType.上级领导).ToString() && p.KHDF != null).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //同级测评4
                e.Row.Cells[4].Text = (jgbmdfLst.
                    Where(p => p.JGBM == model.JGBM && 
                        p.DFZLX == ((int)WebKeys.DFUserType.同级二级单位).ToString() && p.KHDF != null).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //结果审定5
                e.Row.Cells[5].Text =
                    (dlzbLst.Where(p => p.WCZSDRQ != null && p.JGBM == model.JGBM).Count() > 0) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
                //文件提交6
                e.Row.Cells[6].Text = (model.SBSJ != null) ?
                    "<img border='0' src='/Resources/Images/complete.png' />" : "";
            }
        }

        #endregion

        

    }
}